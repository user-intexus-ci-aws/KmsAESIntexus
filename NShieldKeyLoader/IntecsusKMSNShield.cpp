// NShieldKeyLoader.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include "IntecsusKMSNShield.h"
#include "cryptoki.h"
#include <string.h>
#include "SYS\TIMEB.H"
#include "time.h"
#include <iostream>
#include <string>
#include <process.h>


#define LOG_PATH "C:\\WINDOWS\\Temp\\NShieldDUKPTValidator"
static bool initialized = false;
using namespace std;

/*
Module vars
*/
static CK_BBOOL private_val;
static CK_BBOOL sensitive;
static CK_BBOOL extractable;
static CK_BBOOL wrap;
static CK_BBOOL ncrypt;
static CK_BBOOL sign;
static CK_BBOOL derive;

//privates
static int hex2str(char *to, char *from, int len);
static int str2hex(char *to, char *from, int len);
static CK_OBJECT_CLASS class_secret = CKO_SECRET_KEY;
static CK_BBOOL true_val = 1;
static CK_BBOOL false_val = 0;
static CK_KEY_TYPE key_type_des2 = CKK_DES2;

static CK_CHAR_PTR passphrase = NULL;

static CK_MECHANISM zmk_mechanism = { CKM_CAC_TK_DERIVATION, NULL_PTR, 0 };
static CK_ATTRIBUTE zmk_des_template[] = {
	{ CKA_CLASS, &class_secret, sizeof(class_secret) },
	{ CKA_TOKEN, &true_val, sizeof(CK_BBOOL) },
	{ CKA_PRIVATE, &false_val, sizeof(CK_BBOOL) },
	{ CKA_MODIFIABLE, &true_val, sizeof(CK_BBOOL) },
	{ CKA_KEY_TYPE, &key_type_des2, sizeof(key_type_des2) },
	{ CKA_LABEL, NULL, 0 },
	{ CKA_ENCRYPT, &true_val, sizeof(CK_BBOOL) },
	{ CKA_DECRYPT, &true_val, sizeof(CK_BBOOL) },
	{ CKA_WRAP, &true_val, sizeof(CK_BBOOL) },
	{ CKA_UNWRAP, &true_val, sizeof(CK_BBOOL) },
	{ CKA_SIGN, &true_val, sizeof(CK_BBOOL) },
	{ CKA_VERIFY, &true_val, sizeof(CK_BBOOL) },
	{ CKA_DERIVE, &true_val, sizeof(CK_BBOOL) },
	{ CKA_SENSITIVE, &false_val, sizeof(CK_BBOOL) },
	{ CKA_EXTRACTABLE, &true_val, sizeof(CK_BBOOL) },
	{ CKA_TKC1, NULL, 0 },
	{ CKA_TKC2, NULL, 0 },
	{ CKA_TKC3, NULL, 0 }
};

static FILE *FLOG = 0;

static int str2hex(char *to, char *from, int len)
{
	int i;
	char str[32];

	//printf(">>str2hex()>>\n");
	for (i = 0; i < len / 2; i++) {
		str[0] = from[2 * i];
		str[1] = from[2 * i + 1];
		str[2] = '\0';

		to[i] = (char)strtol(str, 0, 16);
	}
	//printf("<<str2hex()<<\n");
	return 0;
}
static int str2hex64(char *to, char *from, int len)
{
	int i;
	char str[64];

	//printf(">>str2hex()>>\n");
	for (i = 0; i < len / 2; i++) {
		str[0] = from[2 * i];
		str[1] = from[2 * i + 1];
		str[2] = '\0';

		to[i] = (char)strtol(str, 0, 16);
	}
	//printf("<<str2hex()<<\n");
	return 0;
}

static int open_log()
{
	char LOG_PATH_C[128];
	if (FLOG == 0) {
		//sprintf(LOG_PATH_C, "%s-%d.log", LOG_PATH, getpid());
		FLOG = fopen(LOG_PATH_C, "a");
		if (FLOG) setvbuf(FLOG, NULL, _IONBF, 0);
	}
	return 0;
}

static int close_log()
{
	if (FLOG != 0) {
		fclose(FLOG);
		FLOG = 0;
	}
	return 0;
}

static CK_RV find_key(CK_SESSION_HANDLE hSession, char *key_label, CK_OBJECT_HANDLE_PTR out_hKey) {
	CK_RV rv = CKR_OK;
	CK_OBJECT_HANDLE hObject;
	CK_ULONG ulObjectCount;
	CK_ULONG key_found = 0;
	char pObjectLabel[1024];
	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };

	//list token objects
	rv = C_FindObjectsInit(hSession, NULL_PTR, 0);
	if (rv != CKR_OK) {
		rv = NOKEY_ERROR;
		goto err;
	}

	while (1) {
		rv = C_FindObjects(hSession, &hObject, 1, &ulObjectCount);
		if (rv != CKR_OK || ulObjectCount == 0) break;

		//print object label attribute
		get_label_template[0].pValue = NULL_PTR;
		get_label_template[0].ulValueLen = 0;
		C_GetAttributeValue(hSession, hObject, get_label_template, NUM(get_label_template));

		if (rv != CKR_OK || ulObjectCount == 0) break;

		memset(pObjectLabel, 0x00, sizeof(pObjectLabel));
		get_label_template[0].pValue = (CK_VOID_PTR)pObjectLabel;
		C_GetAttributeValue(hSession, hObject, get_label_template, NUM(get_label_template));

		if (rv != CKR_OK || ulObjectCount == 0) break;

		pObjectLabel[get_label_template[0].ulValueLen] = '\0';

		if (strcmp(pObjectLabel, key_label) == 0) {
			*out_hKey = hObject;
			key_found = 1;
			break;
		}
	}

	rv = C_FindObjectsFinal(hSession);

	if (key_found <= 0) {
		rv = NOKEY_ERROR;
		goto err;
	}

err:
	return rv;
}

static int parse_component(const char *ini_comp, char *comp) {
	int i;
	char hex[3];
	for (i = 0; i < 32; i += 2) {
		memset(hex, 0x00, sizeof(hex));
		memcpy(hex, ini_comp + i, 2);
		hex[2] = '\0';
		comp[i / 2] = (char)strtol(hex, NULL, 16);
		//printf("0x%02x, ", (unsigned char)comp[i / 2]);
	}
	//printf("\n");
	return 0;
}

static int hex2str(char *to, char *from, int len)
{
	int i;
	char str[32];

	to[0] = '\0';
	for (i = 0; i < len; i++) {
		sprintf(str, "%02x", 0x00FF & from[i]);
		strcat(to, str);
	}

	return 0;
}

static int hex2str64(char *to, char *from, int len)
{
	int i;
	char str[64];

	to[0] = '\0';
	for (i = 0; i < len; i++) {
		sprintf(str, "%02x", 0x00FF & from[i]);
		strcat(to, str);
	}

	return 0;
}


static int rtrim(char *str) {
	if (str == 0)
		return 1;

	for (int index = strlen(str) - 1; index >= 0; index--) {
		if (!isspace(str[index])) {
			str[index + 1] = '\0';
			break;
		}
	}

	return 0;
}

//-----------------------------------------------------------------------
// escribir en archivo
//-----------------------------------------------------------------------
static int Escribir(char *argMensaje, FILE *argFile)
{
	//get time intance
	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char s[1024];

	//format time
	strftime(s, sizeof(s), "[%Y-%m-%d %H:%M:%S] - ", p);

	try
	{
		fwrite(s, sizeof(char), strlen(s), argFile);
		fwrite(argMensaje, sizeof(char), strlen(argMensaje), argFile);
		fwrite("\n", sizeof(char), 1, argFile);
		return NO_ERROR_;
	}
	catch (...)
	{
		return ERR_ESCRIBIENDO_ARCHIVO;
	}
}

//-----------------------------------------------------------------------
// liberar memoria
//-----------------------------------------------------------------------
static int LiberarMemoria(CK_BYTE *arg1, CK_BYTE *arg2, CK_BYTE *arg3)
{
	try
	{
		if (arg1 != NULL)	free(arg1); arg1 = NULL;
		if (arg2 != NULL)	free(arg2); arg2 = NULL;
		if (arg3 != NULL)	free(arg3); arg3 = NULL;

		return NO_ERROR_;
	}
	catch (...)
	{
		return ERR_LIBERANDO_MEMORIA;
	}
}

//-----------------------------------------------------------------------
// cerrar archivos
//-----------------------------------------------------------------------
static int CerrarArchivo(FILE *arg1, FILE *arg2, FILE *arg3)
{
	try
	{
		if (arg1 != NULL)	fclose(arg1);
		if (arg2 != NULL)	fclose(arg2);
		if (arg3 != NULL)	fclose(arg3);
		return NO_ERROR_;
	}
	catch (...)
	{
		return ERR_CERRANDO_ARCHIVO;
	}
}

static int CerrarArchivoLog(FILE *arg1)
{
	try
	{
		if (arg1 != NULL)	fclose(arg1);
		return NO_ERROR_;
	}
	catch (...)
	{
		return ERR_CERRANDO_ARCHIVO;
	}
}


/*====================================================================
Debido a que el Bco trabaja en un mainframe los datos vienen en un
foRmato llamado EBCDIC de IBM, por lo tanto es necesario pasar
los datos a simbologia normal,
es decir, ASCII. Esta funcion posee la tabla de conversion de EBCDIC a
ASCII recibe como parametro que recibe el string a convertir y el Total
de Registros
====================================================================*/

//BANCO MERCANTIL
int translate(char *cadena, int NroReg, int TamReg){

	char c;
	short int aux;
	int i;


	int ebcdic[] = {
		/*
		0x00,0x01,0x02,0x03,0x9C,0x09,0x86,0x7F,0x97,0x8D,0x8E,0x0B,0x0C,0x0D,0x0E,0x0F,    //	'.','.','.','.','ú','.','Ü','','ó','ç','é','.','.','.','.','.',
		0x10,0x11,0x12,0x13,0x9D,0x85,0x08,0x87,0x18,0x19,0x92,0x8F,0x1C,0x1D,0x1E,0x1F,    //	'.','.','.','.','ù','Ö','.','á','.','.','í','è','.','.','.','.',
		0x80,0x81,0x82,0x83,0x84,0x0A,0x17,0x1B,0x88,0x89,0x8A,0x8B,0x8C,0x05,0x06,0x07,    //	'Ä','Å','Ç','É','Ñ','.','.','.','à','â','ä','ã','å','.','.','.',
		0x90,0x91,0x16,0x93,0x94,0x95,0x96,0x04,0x98,0x99,0x9A,0x9B,0x14,0x15,0x9E,0x1A,    //	'ê','ë','.','ì','î','ï','ñ','.','ò','ô','ö','õ','.','.','û','.',
		0x20,0xA0,0xD2,0xA2,0xA3,0xA4,0xA5,0xA6,0xA7,0xA8,0x5B,0x2E,0x3C,0x28,0x2B,0x7C,    //	'.',' ','°','¢','£','§','•','¶','ß','.','.','.','<','(','+','|',//pos 6 al reves, cambio D5 por 5B //pos 3 cambio A1 por D2
		0x26,0xA9,0xAA,0xAB,0xAC,0xAD,0xAE,0xAF,0xB0,0xB1,0x5D,0x24,0x2A,0x29,0x3B,0xE2,    //	'&','©','™','´','¨','≠','Æ','Ø','∞','±','!','$','*',')',';','^',//5E cambio por E2 ultima pos //pos 6 al reves 21 por E3 //tempos 6 al reves E3 por 5D
		0x2D,0x2F,0xB2,0xB3,0xB4,0xB5,0xB6,0xB7,0xB8,0x23,0xE5,0x2C,0x25,0x5F,0x3E,0x3F,    //	'-','/','≤','≥','¥','µ','∂','∑','∏','π','.',',','%','_','>','?',//pos 7 al reves cambio B9 por 23
		0xBA,0xBB,0xBC,0xBD,0xBE,0xD3,0xC0,0xC1,0xC2,0x60,0x3A,0xB9,0x40,0x27,0x3D,0x22,    //	'∫','ª','º','Ω','æ','ø','.','.','.','`',':','#','@',''','=','"',//pos 5 al reves cambio 23 por B9
		0xC3,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0xC4,0xC5,0xC6,0xC7,0xC8,0xC9,    //	'.','a','b','c','d','e','f','g','h','i','.','.','.','.','.','.',
		0xCA,0x6A,0x6B,0x6C,0x6D,0x6E,0x6F,0x70,0x71,0x72,0xCB,0xCC,0xCD,0xCE,0xCF,0xD0,    //	'.','j','k','l','m','n','o','p','q','r','.','.','.','.','.','.',
		0xD1,0xE3,0x73,0x74,0x75,0x76,0x77,0x78,0x79,0x7A,0xA1,0xBF,0xD4,0xD5,0xD6,0xD7,    //	'.','~','s','t','u','v','w','x','y','z','.','.','.','[','.','.',//2da posicion era 7E //pos 3 al reves 5B por D5 //Temp pos 2 cambio de 5D por // pos 6 al reves cambio D2 por A1
		0xD8,0xD9,0xDA,0xDB,0xDC,0xDD,0xDE,0xDF,0xE0,0xE1,0x5E,0x21,0xE4,0x7E,0xE6,0xE7,    //	'.','.','.','.','.','.','.','.','.','.','.','.','.',']','.','.',//3ra pos al reves era 5D, E2 por 5e pos 6 al reves //pos 5 al reves E3 por 21
		0x7B,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0xE8,0xE9,0xEA,0xEB,0xEC,0xED,    //	'{','A','B','C','D','E','F','G','H','I','.','.','.','.','.','.',
		0x7D,0x4A,0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0xEE,0xEF,0xF0,0xF1,0xF2,0xF3,    //	'}','J','K','L','M','N','O','P','Q','R','.','.','.','.','.','.',
		0x5C,0x9F,0x53,0x54,0x55,0x56,0x57,0x58,0x59,0x5A,0xF4,0xF5,0xF6,0xF7,0xF8,0xF9,    //	'\','.','S','T','U','V','W','X','Y','Z','.','.','.','.','.','.',
		0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0xFA,0xFB,0xFC,0xFD,0xFE,0xFF};   //	'0','1','2','3','4','5','6','7','8','9','.','.','.','.','.','.'};
		*/

		/*	0x00,0x01,0x02,0x03,0x9C,0x09,0x86,0x7F,0x97,0x8D,0x8E,0x0B,0x0C,0x0D,0x0E,0x0F,    //	'.','.','.','.','ú','.','Ü','','ó','ç','é','.','.','.','.','.',
		0x10,0x11,0x12,0x13,0x9D,0x85,0x08,0x87,0x18,0x19,0x92,0x8F,0x1C,0x1D,0x1E,0x1F,    //	'.','.','.','.','ù','Ö','.','á','.','.','í','è','.','.','.','.',
		0x80,0x81,0x82,0x83,0x84,0x0A,0x17,0x1B,0x88,0x89,0x8A,0x8B,0x8C,0x05,0x06,0x07,    //	'Ä','Å','Ç','É','Ñ','.','.','.','à','â','ä','ã','å','.','.','.',
		0x90,0x91,0x16,0x93,0x94,0x95,0x96,0x04,0x98,0x99,0x9A,0x9B,0x14,0x15,0x9E,0x1A,    //	'ê','ë','.','ì','î','ï','ñ','.','ò','ô','ö','õ','.','.','û','.',
		0x20,0xA0,0xA1,0xA2,0xA3,0xA4,0xA5,0xA6,0xA7,0xA8,0x5B,0x2E,0x3C,0x28,0x2B,0x7C,    //	'.',' ','°','¢','£','§','•','¶','ß','.','.','.','<','(','+','|',//pos 6 al reves, cambio D5 por 5B
		0x26,0xA9,0xAA,0xAB,0xAC,0xAD,0xAE,0xAF,0xB0,0xB1,0xE3,0x24,0x2A,0x29,0x3B,0xE2,    //	'&','©','™','´','¨','≠','Æ','Ø','∞','±','!','$','*',')',';','^',//5E cambio por E2 ultima pos //pos 6 al reves 21 por E3 //tempos 6 al reves E3 por 5D
		0x2D,0x2F,0xB2,0xB3,0xB4,0xB5,0xB6,0xB7,0xB8,0x23,0xE5,0x2C,0x25,0x5F,0x3E,0x3F,    //	'-','/','≤','≥','¥','µ','∂','∑','∏','π','.',',','%','_','>','?',//pos 7 al reves cambio B9 por 23
		0xBA,0xBB,0xBC,0xBD,0xBE,0xBF,0xC0,0xC1,0xC2,0x60,0x3A,0xB9,0x40,0x27,0x3D,0x22,    //	'∫','ª','º','Ω','æ','ø','.','.','.','`',':','#','@',''','=','"',//pos 5 al reves cambio 23 por B9
		0xC3,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0xC4,0xC5,0xC6,0xC7,0xC8,0xC9,    //	'.','a','b','c','d','e','f','g','h','i','.','.','.','.','.','.',
		0xCA,0x6A,0x6B,0x6C,0x6D,0x6E,0x6F,0x70,0x71,0x72,0xCB,0xCC,0xCD,0xCE,0xCF,0xD0,    //	'.','j','k','l','m','n','o','p','q','r','.','.','.','.','.','.',
		0xD1,0x5D,0x73,0x74,0x75,0x76,0x77,0x78,0x79,0x7A,0xD2,0xD3,0xD4,0xD5,0xD6,0xD7,    //	'.','~','s','t','u','v','w','x','y','z','.','.','.','[','.','.',//2da posicion era 7E //pos 3 al reves 5B por D5
		0xD8,0xD9,0xDA,0xDB,0xDC,0xDD,0xDE,0xDF,0xE0,0xE1,0x5E,0x21,0xE4,0x7E,0xE6,0xE7,    //	'.','.','.','.','.','.','.','.','.','.','.','.','.',']','.','.',//3ra pos al reves era 5D, E2 por 5e pos 6 al reves //pos 5 al reves E3 por 21
		0x7B,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0xE8,0xE9,0xEA,0xEB,0xEC,0xED,    //	'{','A','B','C','D','E','F','G','H','I','.','.','.','.','.','.',
		0x7D,0x4A,0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0xEE,0xEF,0xF0,0xF1,0xF2,0xF3,    //	'}','J','K','L','M','N','O','P','Q','R','.','.','.','.','.','.',
		0x5C,0x9F,0x53,0x54,0x55,0x56,0x57,0x58,0x59,0x5A,0xF4,0xF5,0xF6,0xF7,0xF8,0xF9,    //	'\','.','S','T','U','V','W','X','Y','Z','.','.','.','.','.','.',
		0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0xFA,0xFB,0xFC,0xFD,0xFE,0xFF};   //	'0','1','2','3','4','5','6','7','8','9','.','.','.','.','.','.'};
		*/

		0x00, 0x01, 0x02, 0x03, 0x9C, 0x09, 0x86, 0x7F, 0x97, 0x8D, 0x8E, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,    //	'.','.','.','.','ú','.','Ü','','ó','ç','é','.','.','.','.','.',
		0x10, 0x11, 0x12, 0x13, 0x9D, 0x85, 0x08, 0x87, 0x18, 0x19, 0x92, 0x8F, 0x1C, 0x1D, 0x1E, 0x1F,    //	'.','.','.','.','ù','Ö','.','á','.','.','í','è','.','.','.','.',
		0x80, 0x81, 0x82, 0x83, 0x84, 0x0A, 0x17, 0x1B, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x05, 0x06, 0x07,    //	'Ä','Å','Ç','É','Ñ','.','.','.','à','â','ä','ã','å','.','.','.',
		0x90, 0x91, 0x16, 0x93, 0x94, 0x95, 0x96, 0x04, 0x98, 0x99, 0x9A, 0x9B, 0x14, 0x15, 0x9E, 0x1A,    //	'ê','ë','.','ì','î','ï','ñ','.','ò','ô','ö','õ','.','.','û','.',
		0x20, 0xA0, 0xD2, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0x5B, 0x2E, 0x3C, 0x28, 0x2B, 0x21,    //	'.',' ','°','¢','£','§','•','¶','ß','.','.','.','<','(','+','|',//pos 6 al reves, cambio D5 por 5B //pos 3 cambio A1 por D2 //CV se cambio 0xA5 por 0xD9
		0x26, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0x5D, 0x24, 0x2A, 0x29, 0x3B, 0x5E,    //	'&','©','™','´','¨','≠','Æ','Ø','∞','±','!','$','*',')',';','^',//5E cambio por E2 ultima pos //pos 6 al reves 21 por E3 //tempos 6 al reves E3 por 5D//CV se cambio 0xE2 por 0x5E
		0x2D, 0x2F, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0x7C, 0x2C, 0x25, 0x5F, 0x3E, 0x3F,    //	'-','/','≤','≥','¥','µ','∂','∑','∏','π','.',',','%','_','>','?',//pos 7 al reves cambio B9 por 23 // CV se cambio 0xE5 por 0x7C
		0xBA, 0xBB, 0xBC, 0xBD, 0xBE, 0xD3, 0xC0, 0xC1, 0xC2, 0x60, 0x3A, 0x23, 0x40, 0x27, 0x3D, 0x22,    //	'∫','ª','º','Ω','æ','ø','.','.','.','`',':','#','@',''','=','"',//pos 5 al reves cambio 23 por B9 // CV se cambio 0xB9 por 0x23
		0xC3, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9,    //	'.','a','b','c','d','e','f','g','h','i','.','.','.','.','.','.',
		0xCA, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0xCB, 0xCC, 0xCD, 0xCE, 0xCF, 0xD0,    //	'.','j','k','l','m','n','o','p','q','r','.','.','.','.','.','.',
		0xD1, 0xE3, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0xA1, 0xBF, 0xD4, 0xD5, 0xD6, 0xD7,    //	'.','~','s','t','u','v','w','x','y','z','.','.','.','[','.','.',//2da posicion era 7E //pos 3 al reves 5B por D5 //Temp pos 2 cambio de 5D por // pos 6 al reves cambio D2 por A1
		0xD8, 0xD9, 0xDA, 0xDB, 0xDC, 0xDD, 0xDE, 0xDF, 0xE0, 0xE1, 0xE2, 0x21, 0xE4, 0x7E, 0xE6, 0xE7,    //	'.','.','.','.','.','.','.','.','.','.','.','.','.',']','.','.',//3ra pos al reves era 5D, E2 por 5e pos 6 al reves //pos 5 al reves E3 por 21 // CV se cambio 0x5E por 0xE2
		0x7B, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0xE8, 0xE9, 0xEA, 0xEB, 0xEC, 0xED,    //	'{','A','B','C','D','E','F','G','H','I','.','.','.','.','.','.',
		0x7D, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0xEE, 0xEF, 0xF0, 0xF1, 0xF2, 0xF3,    //	'}','J','K','L','M','N','O','P','Q','R','.','.','.','.','.','.',
		0x5C, 0x9F, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9,    //	'\','.','S','T','U','V','W','X','Y','Z','.','.','.','.','.','.',
		0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF };   //	'0','1','2','3','4','5','6','7','8','9','.','.','.','.','.','.'};
	try
	{
		for (i = 0; i < (TamReg*NroReg); i++)
		{
			aux = cadena[i];
			aux = (((aux) ^ (0xff00))&(aux));
			_snprintf(&c, 1, "%c", ebcdic[aux]);

			//sprintf(&c,"%c",ebcdic[aux]);
			cadena[i] = c;
		}

		return NO_ERROR_;
	}
	catch (...)
	{
		return ERR_EBCDIC_TO_ASCII;
	}
}

/*====================================================================
Busca el segmento NroReg en Origen y lo coloca en cadena
====================================================================*/
int segmentar(char* cadena1, char* origen, int NroReg, int TamReg)
{
	int i, j, enter;
	char ENTER;

	try
	{
		enter = 0x0d;
		_snprintf(&ENTER, 1, "%c", enter);
		//sprintf(&ENTER,"%c",enter);

		for (j = 0; j < NroReg; j++)
		{
			for (i = 0; i < TamReg; i++)
				cadena1[(j*(TamReg + 1)) + i] = origen[((j*(TamReg + 1)) + i) - j];

			cadena1[((j*(TamReg + 1)) + i)] = ENTER;
		}

		return NO_ERROR_;
	}
	catch (...)
	{
		return ERR_SEGMENTANDO;
	}
}


int initResources(char *token_name, char *passphrase, char *key_label, char *out_hSession, char *out_hKey)
{
	//variables locales
	CK_RV rv = CKR_OK;
	CK_ULONG i, j, nslots;
	CK_SLOT_ID_PTR pslots = 0;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE hObject;
	CK_ULONG ulObjectCount;
	CK_ULONG login_success = 0;
	CK_ULONG key_found = 0;
	char *local_token_name[32 + 1];
	char pObjectLabel[1024];
	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);

	open_log();

	fprintf(FLOG, "%s >>>INICIAR_RECURSOS()>>>\n", time_str);
	rv = C_GetSlotList(0, NULL_PTR, &nslots);
	if (rv != CKR_OK) {
		fprintf(FLOG, "%s Error C_GetSlotList 0x%08x\n", time_str, rv);
		rv = INICIA_ERROR;
		goto err;
	}

	pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
	rv = C_GetSlotList(1, pslots, &nslots);
	if (rv != CKR_OK) {
		fprintf(FLOG, "%s Error C_GetSlotList 0x%08x\n", time_str, rv);
		rv = INICIA_ERROR;
		goto err;
	}

	for (i = 0; !login_success && i < nslots; i++) {
		hSlot = pslots[i];

		rv = C_GetSlotInfo(hSlot, &sinfo);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_GetSlotInfo 0x%08x\n", time_str, rv);
			goto err;
		}

		rv = C_GetTokenInfo(hSlot, &tinfo);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_GetTokenInfo 0x%08x\n", time_str, rv);
			goto err;
		}

		/* Skip write protected slots */
		if (tinfo.flags & CKF_WRITE_PROTECTED) {
			continue;
		}

		/* If we have a PIN, skip slots that don't accept it */
		if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED)) {
			continue;
		}

		/* The token label must match */
		memset(local_token_name, 0x00, sizeof(local_token_name));
		memcpy(local_token_name, tinfo.label, 32);
		local_token_name[32] = '\0';

		fprintf(FLOG, "%s token_name='%s', local_token_name='%s'\n", time_str, token_name, local_token_name);
		if (strncmp((char*)token_name, (char*)local_token_name, strlen(token_name)) != 0) {
			continue;
		}

		//CKF_RW_SESSION: in carwizard session can be read only
		rv = C_OpenSession(hSlot,
			CKF_SERIAL_SESSION,
			0, 0,
			&hSession);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_OpenSession 0x%08x\n", time_str, rv);
			rv = SESION_ERROR;
			goto err;
		}

		rv = C_Login(hSession, CKU_USER,
			(CK_CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
		if (rv != CKR_OK && rv != CKR_USER_ALREADY_LOGGED_IN) {
			fprintf(FLOG, "%s Error C_Login 0x%08x\n", time_str, rv);
			rv = LOGIN_ERROR;
			goto err;
		}
		else {
			login_success = 1;
		}

		if (!login_success) {
			//continue to next slot
			continue;
		}

		//list token objects
		rv = C_FindObjectsInit(hSession, NULL_PTR, 0);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_FindObjectsInit 0x%08x\n", time_str, rv);
			rv = NOKEY_ERROR;
			goto err;
		}

		j = 0;
		while (1) {
			rv = C_FindObjects(hSession, &hObject, 1, &ulObjectCount);
			if (rv != CKR_OK || ulObjectCount == 0) break;

			//print object label attribute
			get_label_template[0].pValue = NULL_PTR;
			get_label_template[0].ulValueLen = 0;
			C_GetAttributeValue(hSession, hObject, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK || ulObjectCount == 0) break;

			memset(pObjectLabel, 0x00, sizeof(pObjectLabel));
			get_label_template[0].pValue = (CK_VOID_PTR)pObjectLabel;
			C_GetAttributeValue(hSession, hObject, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK || ulObjectCount == 0) break;

			pObjectLabel[get_label_template[0].ulValueLen] = '\0';

			if (strcmp(pObjectLabel, key_label) == 0) {
				key_found = 1;
				break;
			}
		}

		rv = C_FindObjectsFinal(hSession);
	}

	if (login_success == 0) {
		rv = LOGIN_ERROR;
		goto err;
	}

	if (key_found == 0) {
		rv = NOKEY_ERROR;
		goto err;
	}

	fprintf(FLOG, "Handle Session: '%lu'\n", hSession);
	fprintf(FLOG, "Handle Key: '%lu'\n", hObject);
	sprintf(out_hSession, "%lu", hSession);
	sprintf(out_hKey, "%lu", hObject);

err:
	fprintf(FLOG, "%s ret: %d\n", time_str, rv);
	fprintf(FLOG, "%s <<<INICIAR_RECURSOS()<<<\n", time_str);
	close_log();
	if (pslots) {
		free(pslots);
	}

	return rv;
}



int TDes(CK_BYTE* argDataOrigen,
	int argTamArchivo,
	int argTamRegistro,
	CK_BYTE KeyValue[32],
	int Encrypt,
	CK_BYTE* argRespCompleta,
	FILE *fpLog,
	int modoCifrado)
{
	//local vars
	int rc = NO_ERROR_;
	static CK_CHAR myEmptyAuthInfo[] = "";
	char log_msg[1024];
	int zmk_count = 0;

	CK_RV rv = CKR_OK;
	CK_BBOOL trueVal = TRUE;
	CK_BBOOL falseVal = FALSE;
	CK_OBJECT_HANDLE handle2DesKey = CK_INVALID_HANDLE;
	CK_BYTE IV[] = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	CK_MECHANISM mechanismTDES_CBC = { CKM_DES3_CBC, IV, sizeof(IV) };
	CK_MECHANISM mechanismTDES_ECB = { CKM_DES3_ECB };
	CK_MECHANISM mechanismAES_ECB = { CKM_AES_ECB };
	CK_MECHANISM_PTR finalMechanism = NULL_PTR;
	CK_MECHANISM mechanismZMK = { CKM_DES3_ECB, NULL_PTR, 0 };   // key wrapping mechansim by ZMK key, cambio para nShield
	CK_SLOT_INFO slotInfo;
	CK_TOKEN_INFO tokenInfo;
	CK_SLOT_ID slotID_List[MAX_SLOTS];
	CK_ULONG numSlots = MAX_SLOTS, SlotRight;
	CK_SESSION_HANDLE sessionHandle;
	CK_ULONG cipherDataLength;
	CK_KEY_TYPE keyTypeZMK3 = CKK_DES2; // ZMK is a double lenght 3DES (nShield)
	CK_OBJECT_HANDLE handleObject[MAX_OBJS];
	CK_OBJECT_HANDLE zmk_handle = 0;
	CK_OBJECT_HANDLE obj_handle = 0;
	CK_ULONG ulObjectCount = 0;
	// ZMK label is the literal: "ZMK_DECRYPTFILE"
	CK_CHAR ZMK3label[] = "Kek.persocard";
	CK_CHAR Objectlabel[1024];
	CK_OBJECT_CLASS keyClass = CKO_SECRET_KEY;
	CK_KEY_TYPE keyType2DES = CKK_DES2;
	//CK_BYTE DesKeyValue[16];
	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };
	CK_ULONG wrappedDESKeyLength = 16;

	// local templates
	CK_ATTRIBUTE attributeZMK3Template[] =
	{
		{ CKA_KEY_TYPE, &keyTypeZMK3, sizeof(keyTypeZMK3) },	// double-length ZMK
		{ CKA_LABEL, ZMK3label, sizeof(ZMK3label) },	// key label
		{ CKA_CLASS, &keyClass, sizeof(keyClass) },	// key class
		{ CKA_TOKEN, &trueVal, sizeof(trueVal) },	// fixed value; token object
	};

	CK_ATTRIBUTE attribute2Key[] =
	{
		{ CKA_CLASS, &keyClass, sizeof(keyClass) },	// object class
		{ CKA_TOKEN, &trueVal, sizeof(CK_BBOOL) },	// fixed value; token object
		{ CKA_PRIVATE, &falseVal, sizeof(CK_BBOOL) },	// fixed value; private object
		{ CKA_MODIFIABLE, &trueVal, sizeof(CK_BBOOL) },	// modifiable attribute
		{ CKA_LABEL, NULL_PTR, 0 },	// label null
		{ CKA_KEY_TYPE, &keyType2DES, sizeof(keyType2DES) },	// double-length DES key
		{ CKA_ENCRYPT, &trueVal, sizeof(CK_BBOOL) },	// encrypt function
		{ CKA_DECRYPT, &trueVal, sizeof(CK_BBOOL) },	// encrypt function
		{ CKA_UNWRAP, &trueVal, sizeof(CK_BBOOL) },	// unwrap function
		{ CKA_SIGN, &trueVal, sizeof(CK_BBOOL) },	// sign function
		{ CKA_VERIFY, &trueVal, sizeof(CK_BBOOL) },	// verify function
		{ CKA_DERIVE, &trueVal, sizeof(CK_BBOOL) },	// derive function
		{ CKA_SENSITIVE, &falseVal, sizeof(CK_BBOOL) },	// extractable in clear
		{ CKA_EXTRACTABLE, &trueVal, sizeof(CK_BBOOL) }	// extractable in clear
	};

	CK_C_INITIALIZE_ARGS args =
	{
		NULL_PTR,			// use default locking
		NULL_PTR,			// mechanisms for
		NULL_PTR,			// multithreaded access
		NULL_PTR,			//
		CKF_OS_LOCKING_OK,	// yes, multithreaded
		NULL_PTR
	};

	//---------------------------------------------------------------
	CK_BYTE* respAux = NULL_PTR;
	CK_BYTE* respDesencript = NULL_PTR;
	int totalRegistros = argTamArchivo / argTamRegistro;

	try
	{
		if ((respAux = (CK_BYTE*)malloc(argTamRegistro * sizeof(CK_BYTE))) == NULL)
		{
			Escribir("Memoria Insuficiente 1", fpLog);
			rc = ERR_MEMORIA_INSUFICIENTE;
			goto end_function;
		}

		if ((respDesencript = (CK_BYTE*)malloc(argTamRegistro * sizeof(CK_BYTE))) == NULL)
		{
			Escribir("Memoria insuficiente TDES", fpLog);
			rc = ERR_MEMORIA_INSUFICIENTE;
			goto end_function;
		}


		Escribir("*****************Parametros de entrada***********************\n", fpLog);
		fprintf(fpLog, "Variable  argDataOrigen: %s\n", argDataOrigen);
		fprintf(fpLog, "Variable  argTamArchivo): %d\n", argTamArchivo);
		fprintf(fpLog, "Variable  argTamRegistro: %d\n", argTamRegistro);
		fprintf(fpLog, "Variable  KeyValue: %s\n", KeyValue);
		fprintf(fpLog, "Variable  Encrypt: %d\n", Encrypt);
		fprintf(fpLog, "Variable  argRespCompleta: %s\n", argRespCompleta);
		fprintf(fpLog, "Variable  modoCifrado: %d\n", modoCifrado);

		//---------------------------------------------------------------------
		// preparamos la tarjeta
		//---------------------------------------------------------------------
		rv = C_Initialize(&args);
		if (rv != CKR_OK)
		{
			sprintf(log_msg, "Error iniciando tarjeta, ret: %08x", rv);
			Escribir(log_msg, fpLog);
			rc = ERR_INICIANDO_LIBPKCS11;
			goto end_function;
		}

		//---------------------------------------------------------------------
		// obtenemos lista de slots disponibles
		//---------------------------------------------------------------------
		rv = C_GetSlotList(FALSE, slotID_List, &numSlots);
		if (rv != CKR_OK){
			Escribir("Error obteniendo el slot", fpLog);
			rc = ERR_GETSLOT_LIST;
			goto end_function;
		}

		//---------------------------------------------------------------------
		// obtenemos slots de la tarjeta de cifrado
		//---------------------------------------------------------------------
		if (numSlots <= 0)
		{
			rc = ERR_NUMSLOT_INVALIDO;
			goto end_function;
		}

		for (unsigned short i = 0; i < numSlots; i++)
		{
			(void)memset(slotInfo.slotDescription, 0x00, sizeof(slotInfo.slotDescription));
			(void)memset(slotInfo.manufacturerID, 0x00, sizeof(slotInfo.manufacturerID));

			rv = C_GetSlotInfo(slotID_List[i], &slotInfo);
			if (rv != CKR_OK)
			{
				rc = ERR_GETSLOT_INFO;
				goto end_function;
			}

			sprintf(log_msg, "Obtener info slot: %d OK", i);
			Escribir(log_msg, fpLog);

			rv = C_GetTokenInfo(slotID_List[i], &tokenInfo);
			if (rv != CKR_OK)
			{
				rc = ERR_GETTOKEN_INFO;
				goto end_function;
			}

			sprintf(log_msg, "Obtener info token: %d OK", i);
			Escribir(log_msg, fpLog);

			// skip write protected slots
			if (tokenInfo.flags & CKF_WRITE_PROTECTED)
				continue;

			// skip slots that accept PIN
			if (tokenInfo.flags & CKF_USER_PIN_INITIALIZED)
				continue;

			//save the right slot and break
			SlotRight = slotID_List[i];
			sprintf(log_msg, "Slot seleccionado: %d", i);
			Escribir(log_msg, fpLog);
			break;
		}

		//---------------------------------------------------------------------
		// iniciamos sesion
		//---------------------------------------------------------------------
		rv = C_OpenSession(SlotRight,
			(CKF_SERIAL_SESSION | CKF_RW_SESSION),
			NULL_PTR,
			NULL_PTR,
			&sessionHandle);

		if (rv != CKR_OK)
		{
			rc = ERR_INICIANDO_SESION;
			goto end_function;
		}

		Escribir("Inicio de sesion OK", fpLog);

		//---------------------------------------------------------------------
		// iniciamos busqueda de Zone Master Key
		//---------------------------------------------------------------------
		rv = C_FindObjectsInit(sessionHandle, NULL_PTR, 0);
		if (rv != CKR_OK)
		{
			rc = ERR_INICIANDO_FIND_ZMK;
			goto end_function;
		}

		Escribir("Inicio busqueda de ZMK OK", fpLog);

		//---------------------------------------------------------------------
		// obtiene el manejador del objeto (ZMK)
		//---------------------------------------------------------------------
		while (1)
		{
			rv = C_FindObjects(sessionHandle, &obj_handle, 1, &ulObjectCount);
			if (rv != CKR_OK)
			{
				Escribir("C_FindObjects() Error...", fpLog);
				rc = ERR_BUSCANDO_ZMK;
				goto end_function;
			}

			if (ulObjectCount <= 0)
				break;

			Escribir("Iterando sobre objeto...", fpLog);

			get_label_template[0].pValue = NULL_PTR;
			get_label_template[0].ulValueLen = 0;
			C_GetAttributeValue(sessionHandle, obj_handle, get_label_template, NUM(get_label_template));
			if (rv != CKR_OK)
			{
				Escribir("C_GetAttributeValue(0) Error...", fpLog);
				rc = ERR_BUSCANDO_ZMK;
				goto end_function;
			}

			memset(Objectlabel, 0x00, sizeof(Objectlabel));
			get_label_template[0].pValue = Objectlabel;
			C_GetAttributeValue(sessionHandle, obj_handle, get_label_template, NUM(get_label_template));
			if (rv != CKR_OK)
			{
				Escribir("C_GetAttributeValue(V) Error...", fpLog);
				rc = ERR_BUSCANDO_ZMK;
				goto end_function;
			}

			Objectlabel[get_label_template[0].ulValueLen] = '\0';
			Escribir("Llave\n", fpLog);
			Escribir((char*)Objectlabel, fpLog);


			if (strcmp((char*)ZMK3label, (char*)Objectlabel) == 0)
			{
				Escribir("Manejador ZMK encontrado OK", fpLog);
				Escribir((char*)Objectlabel, fpLog);
				zmk_handle = obj_handle;
				zmk_count++;
				break;
			}
		}

		//---------------------------------------------------------------------
		// termina la busqueda del objeto
		//---------------------------------------------------------------------
		rv = C_FindObjectsFinal(sessionHandle);
		if (rv != CKR_OK)
		{
			Escribir("Error encontarndo objeto final", fpLog);
			rc = ERR_FINALIZANDO_FIND_ZMK;
			goto end_function;
		}
		Escribir("Finalizo busqueda ZMK OK", fpLog);

		//---------------------------------------------------------------------
		// verificamos existencia de algun ZMK
		// debe existir una y sola una ZMK
		//---------------------------------------------------------------------
		if (zmk_count != 1)
		{
			rc = ERR_CANTIDAD_ZMK_INVALIDAS;
			goto end_function;
		}

		Escribir("ZMK encontrada y unica OK", fpLog);

		//---------------------------------------------------------------------
		// desencriptamos CKA_VALUE asociada al manejador handle2DesKey
		// que fue obtenido en paso anterior
		//---------------------------------------------------------------------
		rv = C_UnwrapKey(sessionHandle,
			&mechanismZMK,
			zmk_handle, //handleObject[0] sutituido por unwrap_handle
			(unsigned char*)KeyValue,
			wrappedDESKeyLength,
			attribute2Key,
			NUM(attribute2Key),
			&handle2DesKey);

		if (rv != CKR_OK)
		{
			sprintf(log_msg, "Error desencriptando llave, ret: 0x%08x", rv);
			Escribir(log_msg, fpLog);
			rc = ERR_DESENCRIPTANDO_LLAVE;
			goto end_function;
		}

		//---------------------------------------------------------------------
		// seleccionamos el mecanismo
		//---------------------------------------------------------------------
		if (modoCifrado == 0)
		{
			finalMechanism = &mechanismTDES_ECB;
		}
		else
		{
			finalMechanism = &mechanismTDES_CBC;
		}

		//---------------------------------------------------------------------
		// encriptamos - desencriptamos
		//---------------------------------------------------------------------
		if (Encrypt == 1) // encriptamos
		{
			rv = C_EncryptInit(
				sessionHandle,
				finalMechanism,
				handle2DesKey);

			if (rv != CKR_OK)
			{
				sprintf(log_msg, "Error al inicio del encriptado, ret: 0x%08x", rv);
				Escribir(log_msg, fpLog);
				rc = ERR_INICIANDO_ENCRIPCION;
				goto end_function;
			}

			rv = C_Encrypt(sessionHandle,
				argDataOrigen,
				argTamArchivo,
				argRespCompleta,
				&cipherDataLength);

			if (rv != CKR_OK)
			{
				sprintf(log_msg, "Error encriptando, ret: 0x%08x", rv);
				Escribir(log_msg, fpLog);
				rc = ERR_ENCRIPTANDO_DATA;
				goto end_function;
			}
		}
		else // desencriptamos
		{
			int j = 0;
			int pos = 0;
			int desde = 0;
			int hasta = 0;
			int i = 0;
			char buffer[1];

			for (i = 0; i < totalRegistros; i++)
			{
				desde = argTamRegistro*i;
				hasta = desde + argTamRegistro;

				(void)memset((CK_BYTE*)respAux, '\0', sizeof(CK_BYTE)*argTamRegistro);
				(void)memset((CK_BYTE*)respDesencript, '\0', sizeof(CK_BYTE)*argTamRegistro);

				for (j = desde, pos = 0; j < hasta; j++, pos++)
					respAux[pos] = argDataOrigen[j];

				rv = C_DecryptInit(
					sessionHandle,
					finalMechanism,
					handle2DesKey);

				if (rv != CKR_OK)
				{
					Escribir("Error al inicio del desencriptado", fpLog);
					rc = ERR_INICIANDO_DEENCRIPCION;
					goto end_function;
				}

				rv = C_Decrypt(
					sessionHandle,
					(unsigned char*)respAux,
					argTamRegistro,
					respDesencript,
					&cipherDataLength);

				if (rv != CKR_OK)
				{
					_itoa(i, buffer, 10);
					fwrite("Error desencriptando el registro: ", sizeof(char), 34, fpLog);
					Escribir(buffer, fpLog);
					rc = ERR_DESENCRIPTANDO_DATA;
					goto end_function;
				}

				if (i == 0)
					strncpy((char*)argRespCompleta, (char*)respDesencript, argTamRegistro);
				else
					strncat((char*)argRespCompleta, (char*)respDesencript, argTamRegistro);
			}
		}

		rv = C_DestroyObject(sessionHandle, handle2DesKey);

		if (rv != CKR_OK)
		{
			Escribir("Error destruyendo el objeto", fpLog);
			rc = ERR_DESTRUYENDO_OBJ;
			goto end_function;
		}
	}
	catch (...)
	{
		return ERR_DESENCRIPTANDO_DATA;
	}

end_function:
	//free memory
	if (respAux != NULL_PTR)
		free(respAux);
	if (respDesencript != NULL_PTR)
		free(respDesencript);

	//clean up
	if (handle2DesKey != CK_INVALID_HANDLE)
		C_DestroyObject(sessionHandle, handle2DesKey);
	C_Logout(sessionHandle);
	C_CloseSession(sessionHandle);
	C_Finalize(NULL_PTR);

	return rc;
}




int TDesCopy(CK_BYTE* argDataOrigen,
	int argTamArchivo,
	int argTamRegistro,
	CK_BYTE KeyValue[32],
	int Encrypt,
	CK_BYTE* argRespCompleta,
	FILE *fpLog,
	int modoCifrado,
	char *tokenname,
	char *passphrase,
	char *zmklabel)
{
	//local vars
	int rc = NO_ERROR_;
	static CK_CHAR myEmptyAuthInfo[] = "";
	char log_msg[1024];
	int zmk_count = 0;

	CK_RV rv = CKR_OK;
	CK_BBOOL trueVal = TRUE;
	CK_BBOOL falseVal = FALSE;
	CK_OBJECT_HANDLE handle2DesKey = CK_INVALID_HANDLE;
	CK_BYTE IV[] = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	CK_MECHANISM mechanismTDES_CBC = { CKM_DES3_CBC, IV, sizeof(IV) };
	CK_MECHANISM mechanismTDES_ECB = { CKM_DES3_ECB };
	CK_MECHANISM_PTR finalMechanism = NULL_PTR;
	CK_MECHANISM mechanismZMK = { CKM_DES3_ECB, NULL_PTR, 0 };   // key wrapping mechansim by ZMK key, cambio para nShield
	CK_SLOT_INFO slotInfo;
	CK_TOKEN_INFO tokenInfo;
	CK_SLOT_ID slotID_List[MAX_SLOTS];
	CK_ULONG numSlots = MAX_SLOTS, SlotRight;
	CK_SESSION_HANDLE sessionHandle;
	CK_ULONG cipherDataLength;
	CK_KEY_TYPE keyTypeZMK3 = CKK_DES2; // ZMK is a double lenght 3DES (nShield)
	CK_OBJECT_HANDLE handleObject[MAX_OBJS];
	CK_OBJECT_HANDLE zmk_handle = 0;
	CK_OBJECT_HANDLE obj_handle = 0;
	CK_ULONG ulObjectCount = 0;
	// ZMK label is the literal: "ZMK_DECRYPTFILE"
	CK_CHAR ZMK3label[] = "1nt3csusOCS";
	CK_CHAR Objectlabel[1024];
	CK_OBJECT_CLASS keyClass = CKO_SECRET_KEY;
	CK_KEY_TYPE keyType2DES = CKK_DES2;
	//CK_BYTE DesKeyValue[16];
	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };
	CK_ULONG wrappedDESKeyLength = 16;
	CK_ULONG i, nslots, token_found = 0;
	CK_SLOT_ID_PTR pslots = NULL;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	//token name structures
	char *local_token_name[32 + 1];

	//KCV data structures
	CK_MECHANISM mechanism_tdes_ECB = { CKM_DES3_ECB };
	CK_BYTE pData[8] = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	CK_ULONG ulDataLen = 8;
	CK_BYTE pEncryptedData[8];
	CK_ULONG ulEncryptedDataLen = 8;

	// local templates
	CK_ATTRIBUTE attributeZMK3Template[] =
	{
		{ CKA_KEY_TYPE, &keyTypeZMK3, sizeof(keyTypeZMK3) },	// double-length ZMK
		{ CKA_LABEL, ZMK3label, sizeof(ZMK3label) },	// key label
		{ CKA_CLASS, &keyClass, sizeof(keyClass) },	// key class
		{ CKA_TOKEN, &trueVal, sizeof(trueVal) },	// fixed value; token object
	};

	CK_ATTRIBUTE attribute2Key[] =
	{
		{ CKA_CLASS, &keyClass, sizeof(keyClass) },	// object class
		{ CKA_TOKEN, &trueVal, sizeof(CK_BBOOL) },	// fixed value; token object
		{ CKA_PRIVATE, &falseVal, sizeof(CK_BBOOL) },	// fixed value; private object
		{ CKA_MODIFIABLE, &trueVal, sizeof(CK_BBOOL) },	// modifiable attribute
		{ CKA_LABEL, NULL_PTR, 0 },	// label null
		{ CKA_KEY_TYPE, &keyType2DES, sizeof(keyType2DES) },	// double-length DES key
		{ CKA_ENCRYPT, &trueVal, sizeof(CK_BBOOL) },	// encrypt function
		{ CKA_DECRYPT, &trueVal, sizeof(CK_BBOOL) },	// encrypt function
		{ CKA_UNWRAP, &trueVal, sizeof(CK_BBOOL) },	// unwrap function
		{ CKA_SIGN, &trueVal, sizeof(CK_BBOOL) },	// sign function
		{ CKA_VERIFY, &trueVal, sizeof(CK_BBOOL) },	// verify function
		{ CKA_DERIVE, &trueVal, sizeof(CK_BBOOL) },	// derive function
		{ CKA_SENSITIVE, &falseVal, sizeof(CK_BBOOL) },	// extractable in clear
		{ CKA_EXTRACTABLE, &trueVal, sizeof(CK_BBOOL) }	// extractable in clear
	};

	CK_C_INITIALIZE_ARGS args =
	{
		NULL_PTR,			// use default locking
		NULL_PTR,			// mechanisms for
		NULL_PTR,			// multithreaded access
		NULL_PTR,			//
		CKF_OS_LOCKING_OK,	// yes, multithreaded
		NULL_PTR
	};

	//---------------------------------------------------------------
	CK_BYTE* respAux = NULL_PTR;
	CK_BYTE* respDesencript = NULL_PTR;
	int totalRegistros = argTamArchivo / argTamRegistro;

	try
	{
		if ((respAux = (CK_BYTE*)malloc(argTamRegistro * sizeof(CK_BYTE))) == NULL)
		{
			Escribir("Memoria Insuficiente 1", fpLog);
			rc = ERR_MEMORIA_INSUFICIENTE;
			goto end_function;
		}

		if ((respDesencript = (CK_BYTE*)malloc(argTamRegistro * sizeof(CK_BYTE))) == NULL)
		{
			Escribir("Memoria insuficiente TDES", fpLog);
			rc = ERR_MEMORIA_INSUFICIENTE;
			goto end_function;
		}


		Escribir("*****************Parametros de entrada***********************\n", fpLog);
		fprintf(fpLog, "Variable  tokenname: %s\n", tokenname);
		fprintf(fpLog, "Variable  passphrase: %s\n", passphrase);
		fprintf(fpLog, "Variable  zmklabel: %s\n", zmklabel);



		rv = C_Initialize(NULL_PTR);
		if (rv != CKR_OK) goto end_function;

		rv = C_GetSlotList(0, NULL_PTR, &nslots);
		if (rv != CKR_OK) goto end_function;

		pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
		rv = C_GetSlotList(1, pslots, &nslots);
		if (rv != CKR_OK) goto end_function;

		for (i = 0; i < nslots; i++) {
			hSlot = pslots[i];

			rv = C_GetSlotInfo(hSlot, &sinfo);
			if (rv != CKR_OK) goto end_function;

			rv = C_GetTokenInfo(hSlot, &tinfo);
			if (rv != CKR_OK) goto end_function;

			/* Skip write protected slots */
			if (tinfo.flags & CKF_WRITE_PROTECTED)
				continue;

			/* If we have a PIN, skip slots that don't accept it */
			if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED))
				continue;

			/* The token label must match */
			memset(local_token_name, 0x00, sizeof(local_token_name));
			memcpy(local_token_name, tinfo.label, 32);
			local_token_name[32] = '\0';

			rtrim((char *)local_token_name);
			if (strcmp((char*)tokenname, (char*)local_token_name) != 0) {
				continue;
			}
			token_found = 1;

			rv = C_OpenSession(hSlot,
				CKF_RW_SESSION | CKF_SERIAL_SESSION,
				NULL, NULL,
				&sessionHandle);
			if (rv != CKR_OK) goto end_function;

			private_val = 0;
			if (passphrase) {
				rv = C_Login(sessionHandle, CKU_USER,
					(CK_UTF8CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
				if (rv != CKR_OK) {
					fprintf(stderr, "Failed to login\n");
					goto end_function;
				}
				private_val = 1;
			}

			//init PKCS11 find object method
			rv = C_FindObjectsInit(sessionHandle, NULL_PTR, 0);
			if (rv != CKR_OK) goto end_function;

			//get ZMK handle
			while (1)
			{
				rv = C_FindObjects(sessionHandle, &obj_handle, 1, &ulObjectCount);
				if (rv != CKR_OK) goto end_function;

				if (ulObjectCount <= 0)
					break;

				get_label_template[0].pValue = NULL_PTR;
				get_label_template[0].ulValueLen = 0;
				C_GetAttributeValue(sessionHandle, obj_handle, get_label_template, NUM(get_label_template));

				if (rv != CKR_OK) goto end_function;

				memset(Objectlabel, 0x00, sizeof(Objectlabel));
				get_label_template[0].pValue = Objectlabel;
				C_GetAttributeValue(sessionHandle, obj_handle, get_label_template, NUM(get_label_template));

				if (rv != CKR_OK) goto end_function;

				Objectlabel[get_label_template[0].ulValueLen] = '\0';

				fprintf(fpLog, "Llave: %s\n", Objectlabel);

				if (strcmp(zmklabel, (char*)Objectlabel) == 0)
				{
					Escribir("*****************Llave encontrada***********************\n", fpLog);
					fprintf(fpLog, "Llave encontrada: %s\n", Objectlabel);
					zmk_count++;
					break;
				}
			}
		}


		//---------------------------------------------------------------------
		// termina la busqueda del objeto
		//---------------------------------------------------------------------
		rv = C_FindObjectsFinal(sessionHandle);
		if (rv != CKR_OK)
		{
			Escribir("Error encontarndo objeto final", fpLog);
			rc = ERR_FINALIZANDO_FIND_ZMK;
			goto end_function;
		}
		Escribir("Finalizo busqueda ZMK OK", fpLog);

		//---------------------------------------------------------------------
		// verificamos existencia de algun ZMK
		// debe existir una y sola una ZMK
		//---------------------------------------------------------------------
		if (zmk_count != 1)
		{
			rc = ERR_CANTIDAD_ZMK_INVALIDAS;
			goto end_function;
		}

		Escribir("ZMK encontrada y unica OK", fpLog);

		//---------------------------------------------------------------------
		// desencriptamos CKA_VALUE asociada al manejador handle2DesKey
		// que fue obtenido en paso anterior
		//---------------------------------------------------------------------

		

		//rv = C_UnwrapKey(sessionHandle,
		//	&mechanismZMK,
		//	zmk_handle, //handleObject[0] sutituido por unwrap_handle
		//	(unsigned char*)KeyValue,
		//	wrappedDESKeyLength,
		//	attribute2Key,
		//	NUM(attribute2Key),
		//	&handle2DesKey);


		//if (rv != CKR_OK)
		//{
		//	//fprintf(fpLog, "Error: %s\n", rv);
		//	fprintf(fpLog, "Error: %d\n", rv);
		//	sprintf(log_msg, "Error desencriptando llave, ret: 0x%08x", rv);
		//	Escribir(log_msg, fpLog);
		//	rc = ERR_DESENCRIPTANDO_LLAVE;
		//	goto end_function;
		//}
		char out_hSession, out_hKey;
		int respuestaResources = 0;
		respuestaResources = initResources(tokenname, passphrase, zmklabel, &out_hSession, &out_hKey);
		/*fprintf(fpLog, "Variable  tokenname: %s\n", tokenname);
		fprintf(fpLog, "Variable  passphrase: %s\n", passphrase);
		fprintf(fpLog, "Variable  zmklabel: %s\n", zmklabel);
		fprintf(fpLog, "Variable  KeyValue: %s\n", KeyValue);
	 	fprintf(fpLog, "Variable  out_hSession: %s\n", &out_hSession);
		fprintf(fpLog, "Variable  out_hKey: %s\n", &out_hKey);*/


		//---------------------------------------------------------------------
		// seleccionamos el mecanismo
		//---------------------------------------------------------------------
		if (modoCifrado == 0)
		{
			finalMechanism = &mechanismTDES_ECB;
		}
		else
		{
			finalMechanism = &mechanismTDES_CBC;
		}

		//---------------------------------------------------------------------
		// encriptamos - desencriptamos
		//---------------------------------------------------------------------
		if (Encrypt == 1) // encriptamos
		{

		
			sprintf(&out_hSession, "%lu", sessionHandle);
			sprintf(&out_hKey, "%lu", obj_handle);
			
			rv = C_EncryptInit(
			sessionHandle,
			finalMechanism,
			obj_handle);

			if (rv != CKR_OK)
			{
				sprintf(log_msg, "Error al inicio del encriptado, ret: 0x%08x", rv);
				Escribir(log_msg, fpLog);
				rc = ERR_INICIANDO_ENCRIPCION;
				goto end_function;
			}

			rv = C_Encrypt(sessionHandle,
				argDataOrigen,
				argTamArchivo,
				argRespCompleta,
				&cipherDataLength);


			if (rv != CKR_OK)
			{
				sprintf(log_msg, "Error encriptando, ret: 0x%08x", rv);
				Escribir(log_msg, fpLog);
				rc = ERR_ENCRIPTANDO_DATA;
				goto end_function;
			}
		}
		else // desencriptamos
		{
			int j = 0;
			int pos = 0;
			int desde = 0;
			int hasta = 0;
			int i = 0;
			char buffer[1];

			for (i = 0; i < totalRegistros; i++)
			{
				desde = argTamRegistro*i;
				hasta = desde + argTamRegistro;

				(void)memset((CK_BYTE*)respAux, '\0', sizeof(CK_BYTE)*argTamRegistro);
				(void)memset((CK_BYTE*)respDesencript, '\0', sizeof(CK_BYTE)*argTamRegistro);

				for (j = desde, pos = 0; j < hasta; j++, pos++)
					respAux[pos] = argDataOrigen[j];

				rv = C_DecryptInit(
					sessionHandle,
					finalMechanism,
					handle2DesKey);

				if (rv != CKR_OK)
				{
					Escribir("Error al inicio del desencriptado", fpLog);
					rc = ERR_INICIANDO_DEENCRIPCION;
					goto end_function;
				}

				rv = C_Decrypt(
					sessionHandle,
					(unsigned char*)respAux,
					argTamRegistro,
					respDesencript,
					&cipherDataLength);

				if (rv != CKR_OK)
				{
					_itoa(i, buffer, 10);
					fwrite("Error desencriptando el registro: ", sizeof(char), 34, fpLog);
					Escribir(buffer, fpLog);
					rc = ERR_DESENCRIPTANDO_DATA;
					goto end_function;
				}

				if (i == 0)
					strncpy((char*)argRespCompleta, (char*)respDesencript, argTamRegistro);
				else
					strncat((char*)argRespCompleta, (char*)respDesencript, argTamRegistro);
			}
		}

		rv = C_DestroyObject(sessionHandle, handle2DesKey);

		if (rv != CKR_OK)
		{
			Escribir("Error destruyendo el objeto", fpLog);
			rc = ERR_DESTRUYENDO_OBJ;
			goto end_function;
		}
	}
	catch (...)
	{
		return ERR_DESENCRIPTANDO_DATA;
	}

end_function:
	//free memory
	if (respAux != NULL_PTR)
		free(respAux);
	if (respDesencript != NULL_PTR)
		free(respDesencript);

	//clean up
	if (handle2DesKey != CK_INVALID_HANDLE)
		C_DestroyObject(sessionHandle, handle2DesKey);
	C_Logout(sessionHandle);
	C_CloseSession(sessionHandle);
	C_Finalize(NULL_PTR);

	return rc;
}




DLLDIR int STACK_CALL LOAD_ZMK(char *C1, char *C2, char *C3, char *tokenname, char *passphrase, char *zmklabel)
{
	CK_ULONG i, nslots, ulObjectCount = 0, zmk_count = 0, token_found = 0;
	CK_SLOT_ID_PTR pslots = NULL;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	CK_RV rv = CKR_OK;
	CK_OBJECT_HANDLE hKey;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE obj_handle = 0;

	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };
	CK_CHAR Objectlabel[1024];

	CK_BYTE local_c1[COMPONENT_SIZE + 1];
	CK_BYTE local_c2[COMPONENT_SIZE + 1];
	CK_BYTE local_c3[COMPONENT_SIZE + 1];
	CK_BYTE IV[IV_LEN + 1];

	//token name structures
	char *local_token_name[32 + 1];

	//validating null values
	if (C1 == NULL) return -1;
	if (C2 == NULL) return -1;
	if (C3 == NULL) return -1;

	//parsing components
	parse_component(C1, (char*)local_c1);
	parse_component(C2, (char*)local_c2);
	parse_component(C3, (char*)local_c3);

	rv = C_Initialize(NULL_PTR);
	if (rv != CKR_OK) goto err;

	rv = C_GetSlotList(0, NULL_PTR, &nslots);
	if (rv != CKR_OK) goto err;

	pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
	rv = C_GetSlotList(1, pslots, &nslots);
	if (rv != CKR_OK) goto err;

	for (i = 0; i < nslots; i++) {
		hSlot = pslots[i];

		rv = C_GetSlotInfo(hSlot, &sinfo);
		if (rv != CKR_OK) goto err;

		rv = C_GetTokenInfo(hSlot, &tinfo);
		if (rv != CKR_OK) goto err;

		/* Skip write protected slots */
		if (tinfo.flags & CKF_WRITE_PROTECTED)
			continue;

		/* If we have a PIN, skip slots that don't accept it */
		if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED))
			continue;

		/* The token label must match */
		memset(local_token_name, 0x00, sizeof(local_token_name));
		memcpy(local_token_name, tinfo.label, 32);
		local_token_name[32] = '\0';

		rtrim((char *)local_token_name);
		if (strcmp((char*)tokenname, (char*)local_token_name) != 0) {
			continue;
		}
		token_found = 1;

		rv = C_OpenSession(hSlot,
			CKF_RW_SESSION | CKF_SERIAL_SESSION,
			NULL, NULL,
			&hSession);
		if (rv != CKR_OK) goto err;

		private_val = 0;
		if (passphrase) {
			rv = C_Login(hSession, CKU_USER,
				(CK_UTF8CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
			if (rv != CKR_OK) {
				fprintf(stderr, "Failed to login\n");
				goto err;
			}
			private_val = 1;
		}

		//init PKCS11 find object method
		rv = C_FindObjectsInit(hSession, NULL_PTR, 0);
		if (rv != CKR_OK) goto err;

		//count ZMKs
		while (1)
		{
			rv = C_FindObjects(hSession, &obj_handle, 1, &ulObjectCount);
			if (rv != CKR_OK) goto err;

			if (ulObjectCount <= 0)
				break;

			get_label_template[0].pValue = NULL_PTR;
			get_label_template[0].ulValueLen = 0;
			C_GetAttributeValue(hSession, obj_handle, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK) goto err;

			memset(Objectlabel, 0x00, sizeof(Objectlabel));
			get_label_template[0].pValue = Objectlabel;
			C_GetAttributeValue(hSession, obj_handle, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK) goto err;

			Objectlabel[get_label_template[0].ulValueLen] = '\0';

			if (strcmp(zmklabel, (char*)Objectlabel) == 0)
			{
				zmk_count++;
				continue;
			}
		}

		//end PKCS11 find object method
		rv = C_FindObjectsFinal(hSession);
		if (rv != CKR_OK) goto err;

		//if zmk already exist, this function throws and error
		if (zmk_count > 0) {
			rv = 0x8FFFFFFF;
			goto err;
		}

		//assert(zmk_des_template[5].type == CKA_LABEL);
		zmk_des_template[5].pValue = (void *)zmklabel;
		zmk_des_template[5].ulValueLen = (CK_ULONG)strlen((const char *)zmklabel);

		//assert(zmk_des_template[15].type == CKA_TKC1);
		zmk_des_template[15].pValue = (void *)local_c1;
		zmk_des_template[15].ulValueLen = (CK_ULONG)COMPONENT_SIZE;

		//assert(zmk_des_template[16].type == CKA_TKC2);
		zmk_des_template[16].pValue = (void *)local_c2;
		zmk_des_template[16].ulValueLen = (CK_ULONG)COMPONENT_SIZE;

		//assert(zmk_des_template[17].type == CKA_TKC3);
		zmk_des_template[17].pValue = (void *)local_c3;
		zmk_des_template[17].ulValueLen = (CK_ULONG)COMPONENT_SIZE;

		//se genera el IV random
		rv = C_GenerateRandom(hSession, IV, IV_LEN);

		zmk_mechanism.pParameter = IV;
		zmk_mechanism.ulParameterLen = IV_LEN;

		rv = C_GenerateKey(hSession, &zmk_mechanism,
			zmk_des_template, NUM(zmk_des_template),
			&hKey);

		if (rv != CKR_OK) {
			fprintf(stderr, "Failed to generate ZMK key\n");
			goto err;
		}

		if (passphrase) {
			rv = C_Logout(hSession);
			if (rv != CKR_OK) goto err;
		}

		rv = C_CloseSession(hSession);
		if (rv != CKR_OK) goto err;
	} /* for all slots */

	if (token_found == 0)
		rv = 0x8FFFFFFE;

	free(pslots);

err:
	if (rv == CKR_OK)
		fprintf(stdout, "OK\n");
	else
		fprintf(stderr, "failed rv = %08lX\n", rv);

	C_Finalize(NULL_PTR);

	return rv;
}

//DLLDIR int STACK_CALL KCV_ZMK_TEST(char *tokenname, char *passphrase, string *llave[3])
DLLDIR int STACK_CALL KCV_ZMK_TEST(char *tokenname, char *passphrase)
{
	FILE *fpLog = NULL;
	CK_ULONG i, nslots, ulObjectCount = 0, zmk_count = 0, token_found = 0;
	CK_SLOT_ID_PTR pslots = NULL;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	CK_RV rv = CKR_OK;
	CK_OBJECT_HANDLE hKey;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE obj_handle = 0;

	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };
	CK_CHAR Objectlabel[1024];

	//KCV data structures
	CK_MECHANISM mechanism_tdes_ECB = { CKM_DES3_ECB };
	CK_BYTE pData[8] = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	CK_ULONG ulDataLen = 8;
	CK_BYTE pEncryptedData[8];
	CK_ULONG ulEncryptedDataLen = 8;

	//token name structures
	char *local_token_name[32 + 1];
	errno_t err3;



	err3 = fopen_s(&fpLog, "C:\\Windows\\Temp\\Decript.log", "w+b");
	if (err3 != 0)
	{
		return ERR_CREANDO_ARCHIVO_LOG;
	}

	fprintf(fpLog, "Variable tokenname: %s\n", tokenname);
	fprintf(fpLog, "Variable passphras: %s\n", passphrase);

	//string *apuntador = NULL; //Declaramos un puntero
	////Es recomendable inicializar un puntero en null, para detectar errores f·cilmente

	//string letra; //Declaramos una variable primitiva

	//apuntador = &letra; //Asignamos al apuntador la direcciÛn de memoria de la variable primitiva

	//*apuntador = 'prue';
	//llave[0] = apuntador;
	//*apuntador = 'part';
	//llave[1] = apuntador;
	//*apuntador = 'asda';
	//llave[2] = apuntador;





	rv = C_Initialize(NULL_PTR);
	if (rv != CKR_OK) goto err;

	rv = C_GetSlotList(0, NULL_PTR, &nslots);
	if (rv != CKR_OK) goto err;

	pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
	rv = C_GetSlotList(1, pslots, &nslots);
	if (rv != CKR_OK) goto err;

	for (i = 0; i < nslots; i++) {
		hSlot = pslots[i];

		rv = C_GetSlotInfo(hSlot, &sinfo);
		if (rv != CKR_OK) goto err;

		rv = C_GetTokenInfo(hSlot, &tinfo);
		if (rv != CKR_OK) goto err;

		/* Skip write protected slots */
		if (tinfo.flags & CKF_WRITE_PROTECTED)
			continue;

		/* If we have a PIN, skip slots that don't accept it */
		if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED))
			continue;

		/* The token label must match */
		memset(local_token_name, 0x00, sizeof(local_token_name));
		memcpy(local_token_name, tinfo.label, 32);
		local_token_name[32] = '\0';

		rtrim((char *)local_token_name);
		if (strcmp((char*)tokenname, (char*)local_token_name) != 0) {
			continue;
		}
		token_found = 1;

		rv = C_OpenSession(hSlot,
			CKF_RW_SESSION | CKF_SERIAL_SESSION,
			NULL, NULL,
			&hSession);
		if (rv != CKR_OK) goto err;

		private_val = 0;
		if (passphrase) {
			rv = C_Login(hSession, CKU_USER,
				(CK_UTF8CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
			if (rv != CKR_OK) {
				fprintf(stderr, "Failed to login\n");
				goto err;
			}
			private_val = 1;
		}

		//init PKCS11 find object method
		rv = C_FindObjectsInit(hSession, NULL_PTR, 0);
		if (rv != CKR_OK) goto err;

		//get ZMK handle
		while (1)
		{
			rv = C_FindObjects(hSession, &obj_handle, 1, &ulObjectCount);
			if (rv != CKR_OK) goto err;

			if (ulObjectCount <= 0)
				break;

			get_label_template[0].pValue = NULL_PTR;
			get_label_template[0].ulValueLen = 0;
			C_GetAttributeValue(hSession, obj_handle, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK) goto err;

			memset(Objectlabel, 0x00, sizeof(Objectlabel));
			get_label_template[0].pValue = Objectlabel;
			C_GetAttributeValue(hSession, obj_handle, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK) goto err;

			Objectlabel[get_label_template[0].ulValueLen] = '\0';

			fprintf(fpLog, "LLave: %s\n", (char*)Objectlabel);
		}

		//end PKCS11 find object method
		rv = C_FindObjectsFinal(hSession);
		if (rv != CKR_OK) goto err;



		if (passphrase) {
			rv = C_Logout(hSession);
			if (rv != CKR_OK) goto err;
		}

		rv = C_CloseSession(hSession);
		if (rv != CKR_OK) goto err;

	} /* for all slots */

	if (token_found == 0)
		rv = 0x8FFFFFFE;

	free(pslots);

err:
	if (rv == CKR_OK) {
		fprintf(stdout, "OK\n");
	}
	else {
		fprintf(stderr, "failed rv = %08lX\n", rv);
	}

	CerrarArchivoLog(fpLog);

	C_Finalize(NULL_PTR);

	return rv;
}

DLLDIR int STACK_CALL KCV_ZMK(char *tokenname, char *passphrase, char *zmklabel, char *KCV)
{
	FILE *fpLog = NULL;
	CK_ULONG i, nslots, ulObjectCount = 0, zmk_count = 0, token_found = 0;
	CK_SLOT_ID_PTR pslots = NULL;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	CK_RV rv = CKR_OK;
	CK_OBJECT_HANDLE hKey;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE obj_handle = 0;

	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };
	CK_CHAR Objectlabel[1024];

	//KCV data structures
	CK_MECHANISM mechanism_tdes_ECB = { CKM_DES3_ECB };
	CK_BYTE pData[8] = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	CK_ULONG ulDataLen = 8;
	CK_BYTE pEncryptedData[8];
	CK_ULONG ulEncryptedDataLen = 8;

	//token name structures
	char *local_token_name[32 + 1];
	errno_t err3;

	err3 = fopen_s(&fpLog, "C:\\Windows\\Temp\\Decript.log", "w+b");
	if (err3 != 0)
	{
		return ERR_CREANDO_ARCHIVO_LOG;
	}

	fprintf(fpLog, "Variable tokenname: %s\n", tokenname);
	fprintf(fpLog, "Variable passphras: %s\n", passphrase);
	fprintf(fpLog, "Variable zmklabel: %s\n", zmklabel);

	CerrarArchivoLog(fpLog);



	//validating null values
	if (KCV == NULL) return -1;

	rv = C_Initialize(NULL_PTR);
	if (rv != CKR_OK) goto err;

	rv = C_GetSlotList(0, NULL_PTR, &nslots);
	if (rv != CKR_OK) goto err;

	pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
	rv = C_GetSlotList(1, pslots, &nslots);
	if (rv != CKR_OK) goto err;

	for (i = 0; i < nslots; i++) {
		hSlot = pslots[i];

		rv = C_GetSlotInfo(hSlot, &sinfo);
		if (rv != CKR_OK) goto err;

		rv = C_GetTokenInfo(hSlot, &tinfo);
		if (rv != CKR_OK) goto err;

		/* Skip write protected slots */
		if (tinfo.flags & CKF_WRITE_PROTECTED)
			continue;

		/* If we have a PIN, skip slots that don't accept it */
		if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED))
			continue;

		/* The token label must match */
		memset(local_token_name, 0x00, sizeof(local_token_name));
		memcpy(local_token_name, tinfo.label, 32);
		local_token_name[32] = '\0';

		rtrim((char *)local_token_name);
		if (strcmp((char*)tokenname, (char*)local_token_name) != 0) {
			continue;
		}
		token_found = 1;

		rv = C_OpenSession(hSlot,
			CKF_RW_SESSION | CKF_SERIAL_SESSION,
			NULL, NULL,
			&hSession);
		if (rv != CKR_OK) goto err;

		private_val = 0;
		if (passphrase) {
			rv = C_Login(hSession, CKU_USER,
				(CK_UTF8CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
			if (rv != CKR_OK) {
				fprintf(stderr, "Failed to login\n");
				goto err;
			}
			private_val = 1;
		}

		//init PKCS11 find object method
		rv = C_FindObjectsInit(hSession, NULL_PTR, 0);
		if (rv != CKR_OK) goto err;

		//get ZMK handle
		while (1)
		{
			rv = C_FindObjects(hSession, &obj_handle, 1, &ulObjectCount);
			if (rv != CKR_OK) goto err;

			if (ulObjectCount <= 0)
				break;

			get_label_template[0].pValue = NULL_PTR;
			get_label_template[0].ulValueLen = 0;
			C_GetAttributeValue(hSession, obj_handle, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK) goto err;

			memset(Objectlabel, 0x00, sizeof(Objectlabel));
			get_label_template[0].pValue = Objectlabel;
			C_GetAttributeValue(hSession, obj_handle, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK) goto err;

			Objectlabel[get_label_template[0].ulValueLen] = '\0';

			if (strcmp(zmklabel, (char*)Objectlabel) == 0)
			{
				zmk_count++;
				break;
			}
		}

		//end PKCS11 find object method
		rv = C_FindObjectsFinal(hSession);
		if (rv != CKR_OK) goto err;

		if (zmk_count <= 0) {
			rv = CKR_GENERAL_ERROR;
			goto err;
		}

		//init encrypt
		rv = C_EncryptInit(hSession,
			&mechanism_tdes_ECB,
			obj_handle);

		if (rv != CKR_OK) goto err;

		//generating KCV
		rv = C_Encrypt(hSession,
			pData,
			ulDataLen,
			pEncryptedData,
			&ulEncryptedDataLen);

		if (rv != CKR_OK) goto err;

		hex2str(KCV, (char*)pEncryptedData, ulEncryptedDataLen);

		if (passphrase) {
			rv = C_Logout(hSession);
			if (rv != CKR_OK) goto err;
		}

		rv = C_CloseSession(hSession);
		if (rv != CKR_OK) goto err;

	} /* for all slots */

	if (token_found == 0)
		rv = 0x8FFFFFFE;

	free(pslots);

err:
	if (rv == CKR_OK) {
		fprintf(stdout, "OK\n");
	}
	else {
		fprintf(stderr, "failed rv = %08lX\n", rv);
	}

	C_Finalize(NULL_PTR);

	return rv;
}

DLLDIR int STACK_CALL GENERATE_KEY(char *C1, char *C2, char *C3, char *tokenname, char *passphrase)
{
	CK_ULONG i, nslots, ulObjectCount = 0, zmk_count = 0, token_found = 0;
	CK_SLOT_ID_PTR pslots = NULL;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	CK_RV rv = CKR_OK;
	CK_OBJECT_HANDLE hKey;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE obj_handle = 0;

	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };
	CK_CHAR Objectlabel[1024];

	CK_BYTE local_c1[COMPONENT_SIZE + 1];
	CK_BYTE local_c2[COMPONENT_SIZE + 1];
	CK_BYTE local_c3[COMPONENT_SIZE + 1];
	CK_BYTE IV[IV_LEN + 1];

	//token name structures
	char *local_token_name[32 + 1];

	rv = C_Initialize(NULL_PTR);
	if (rv != CKR_OK) goto err;

	rv = C_GetSlotList(0, NULL_PTR, &nslots);
	if (rv != CKR_OK) goto err;

	pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
	rv = C_GetSlotList(1, pslots, &nslots);
	if (rv != CKR_OK) goto err;

	for (i = 0; i < nslots; i++) {
		hSlot = pslots[i];

		rv = C_GetSlotInfo(hSlot, &sinfo);
		if (rv != CKR_OK) goto err;

		rv = C_GetTokenInfo(hSlot, &tinfo);
		if (rv != CKR_OK) goto err;

		/* Skip write protected slots */
		if (tinfo.flags & CKF_WRITE_PROTECTED)
			continue;

		/* If we have a PIN, skip slots that don't accept it */
		if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED))
			continue;

		/* The token label must match */
		memset(local_token_name, 0x00, sizeof(local_token_name));
		memcpy(local_token_name, tinfo.label, 32);
		local_token_name[32] = '\0';

		rtrim((char *)local_token_name);
		if (strcmp((char*)tokenname, (char*)local_token_name) != 0) {
			continue;
		}
		token_found = 1;

		rv = C_OpenSession(hSlot,
			CKF_RW_SESSION | CKF_SERIAL_SESSION,
			NULL, NULL,
			&hSession);
		if (rv != CKR_OK) goto err;

		private_val = 0;
		if (passphrase) {
			rv = C_Login(hSession, CKU_USER,
				(CK_UTF8CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
			if (rv != CKR_OK) {
				fprintf(stderr, "Failed to login\n");
				goto err;
			}
			private_val = 1;
		}

		//Generate 3 random compoents
		rv = C_GenerateRandom(hSession, local_c1, COMPONENT_SIZE);
		if (rv != CKR_OK) {
			fprintf(stderr, "Failed to generate component1\n");
			goto err;
		}

		rv = C_GenerateRandom(hSession, local_c2, COMPONENT_SIZE);
		if (rv != CKR_OK) {
			fprintf(stderr, "Failed to generate component2\n");
			goto err;
		}

		rv = C_GenerateRandom(hSession, local_c3, COMPONENT_SIZE);
		if (rv != CKR_OK) {
			fprintf(stderr, "Failed to generate component3\n");
			goto err;
		}

		hex2str(C1, (char*)local_c1, COMPONENT_SIZE);
		hex2str(C2, (char*)local_c2, COMPONENT_SIZE);
		hex2str(C3, (char*)local_c3, COMPONENT_SIZE);

		if (passphrase) {
			rv = C_Logout(hSession);
			if (rv != CKR_OK) goto err;
		}

		rv = C_CloseSession(hSession);
		if (rv != CKR_OK) goto err;
	} /* for all slots */

	if (token_found == 0)
		rv = 0x8FFFFFFE;

	free(pslots);

err:
	if (rv == CKR_OK)
		fprintf(stdout, "OK\n");
	else
		fprintf(stderr, "failed rv = %08lX\n", rv);

	C_Finalize(NULL_PTR);

	return rv;
}

/*====================================================================
Esta funcion lee el archivo origen de la ruta especificada por *fuente,
lo procesa (cifra/decifra) mediante el algoritmo 3DES utilizando las
llaves contenidas en llaves(llave_A, y graba el resultado
en el archivo especificado en la ruta especificada por *destino. La funciÛn
devuelve cero (0) si la operaciÛn se realizÛ completa, y uno (1) en caso
contrario.

encrip=0-> descifrar;
encrip=1-> cifrar;
ebcdic=1->los datos fueron cifrados en abcdic y hace falta traducir
ebcdic=0->los datos fueron cifrados en ascii y no hace falta traducir
modoCifrado=0 -> Usa modo ECB
modoCifrado=1 -> Usa modo CBC, con IV = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}
====================================================================*/
DLLDIR int STACK_CALL Decrypt(char* argArchivoFuente,
	char* argArchivoDestino,
	char* tamArchivoFuenteArg,
	char* tamRegistroArg,
	char* llavesArg,
	char* encriptarArg,
	char* fuenteEbcdicArg,
	char* modoCifradoArg)
{
	FILE *fpLog = NULL;
	FILE *fpOrigen = NULL;
	FILE *fpDestino = NULL;

	errno_t err1;
	errno_t err2;
	errno_t err3;

	CK_BYTE* grupoRegOrigen = NULL;
	CK_BYTE* grupoRegDestino = NULL;
	CK_BYTE* grupoRegDestinoEnter = NULL;

	long argTamArchivoFuente = atol(tamArchivoFuenteArg);
	CK_ULONG argTamRegistro = atol(tamRegistroArg);
	int argEncriptar = atoi(encriptarArg);
	int argFuenteEbcdic = atoi(fuenteEbcdicArg);
	int modoCifrado = atoi(modoCifradoArg);

	char argLlaves[32];
	strcpy(argLlaves, llavesArg);

	int respuestaInt = 0;
	int totBloquesLectura = 0;
	int regPorBloqueLectura = 100000;  //Capacidad maxima 100.000 registros por archivo

	int tamBloqueLectura = regPorBloqueLectura*argTamRegistro;
	int tamBloqueLecturaResiduo = 0;


	// ----------------------------------------------------------------
	// preparamos archivos origen, destino, log
	// ----------------------------------------------------------------	
	try
	{
		err1 = fopen_s(&fpOrigen, argArchivoFuente, "rb");
		if (err1 != 0)
		{
			return ERR_ABRIENDO_ARCHIVO;
		}

		err2 = fopen_s(&fpDestino, argArchivoDestino, "w+b");
		if (err2 != 0)
		{
			return ERR_CREANDO_ARCHIVO_DES;
		}

		err3 = fopen_s(&fpLog, "C:\\Windows\\Temp\\Decript.log", "w+b");
		if (err3 != 0)
		{
			return ERR_CREANDO_ARCHIVO_LOG;
		}

		Escribir("*****************Parametros de entrada***********************\n", fpLog);
		fprintf(fpLog, "Variable  argArchivoFuente: %s\n", argArchivoFuente);
		fprintf(fpLog, "Variable  argArchivoDestino: %s\n", argArchivoDestino);
		fprintf(fpLog, "Variable  tamArchivoFuenteArg: %s\n", tamArchivoFuenteArg);
		fprintf(fpLog, "Variable  tamRegistroArg: %s\n", tamRegistroArg);
		fprintf(fpLog, "Variable  llavesArg LLAVE: %s\n", llavesArg);
		fprintf(fpLog, "Variable  encriptarArg: %s\n", encriptarArg);
		fprintf(fpLog, "Variable  fuenteEbcdicArgr: %s\n", fuenteEbcdicArg);
		fprintf(fpLog, "Variable  modoCifradoArg: %s\n", modoCifradoArg);

		// get start time
		struct _timeb startTime;
		char *timeline;

		_ftime(&startTime);
		timeline = ctime(&(startTime.time));



		Escribir("*********************************************************", fpLog);
		fwrite("Fecha del reporte: ", sizeof(char), 19, fpLog);
		Escribir(timeline, fpLog);
		Escribir("Archivo procesado: ", fpLog);
		Escribir(argArchivoFuente, fpLog);
		Escribir("*********************************************************", fpLog);



	}
	catch (...)
	{
		Escribir("Error preparando archivos (origen,destino,log)", fpLog);
		CerrarArchivo(fpOrigen, fpDestino, fpLog);
		return ERR_ABRIENDO_ARCHIVO;
	}

	// ----------------------------------------------------------------
	// determinamos total bloques de lecturas (continuos y residuales)
	// ----------------------------------------------------------------
	try
	{
		if (argTamArchivoFuente <= tamBloqueLectura)
		{
			totBloquesLectura = 0;
			tamBloqueLecturaResiduo = argTamArchivoFuente;
		}
		else
		{
			totBloquesLectura = argTamArchivoFuente / tamBloqueLectura;
			tamBloqueLecturaResiduo = argTamArchivoFuente - (totBloquesLectura*tamBloqueLectura);
		}

		fprintf(fpLog, "argTamRegistro: %d", argTamRegistro);

	}
	catch (...)
	{
		Escribir("Error determinando tamano bloque de lectura", fpLog);
		CerrarArchivo(fpOrigen, fpDestino, fpLog);
		return ERR_DETERMINA_BLOKLECTURA;
	}
	Escribir("Determinando bloques de lectura", fpLog);

	// ----------------------------------------------------------------
	// reservamos espacio de memoria
	// ----------------------------------------------------------------	
	try
	{
		if ((grupoRegOrigen = (CK_BYTE*)malloc(tamBloqueLectura)) == NULL)
		{
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_MEMORIA_INSUFICIENTE;
		}

		if ((grupoRegDestino = (CK_BYTE*)malloc(tamBloqueLectura)) == NULL)
		{
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_MEMORIA_INSUFICIENTE;
		}

		if ((grupoRegDestinoEnter = (CK_BYTE*)malloc(tamBloqueLectura + regPorBloqueLectura)) == NULL)
		{
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_MEMORIA_INSUFICIENTE;
		}
	}
	catch (...)
	{
		Escribir("Error reservando espacio de memoria", fpLog);
		CerrarArchivo(fpOrigen, fpDestino, fpLog);
		return ERR_RESERVANDO_MEMORIA;
	}

	Escribir("Memoria reservada", fpLog);

	// ----------------------------------------------------------------
	// desencriptamos grupo de registros continuos
	// ----------------------------------------------------------------
	for (int i = 0; i < totBloquesLectura; i++)
	{
		// ---------------------------------------------
		// limpiamos contenido de variables de memoria
		// ---------------------------------------------

		//strset((char*)grupoRegOrigen,'\0');
		//strset((char*)grupoRegDestino,'\0');
		//strset((char*)grupoRegDestinoEnter,'\0');

		memset((char*)grupoRegOrigen, '0', tamBloqueLectura);
		memset((char*)grupoRegDestino, '0', tamBloqueLectura);
		memset((char*)grupoRegDestinoEnter, '0', tamBloqueLectura + regPorBloqueLectura);

		// ---------------------------------------------
		// leemos grupo de registros en grupoRegOrigen
		// ---------------------------------------------
		fread(grupoRegOrigen, sizeof(char), tamBloqueLectura, fpOrigen);
		Escribir("fread completado", fpLog);

		// ---------------------------------------------
		// invocamos servicio de desencriptado
		// ---------------------------------------------
		respuestaInt = TDes(
			grupoRegOrigen,
			tamBloqueLectura,
			argTamRegistro,
			(CK_BYTE_PTR)argLlaves,
			argEncriptar,
			grupoRegDestino,
			fpLog,
			modoCifrado);

		if (respuestaInt != 0)
		{
			Escribir("Error desencriptando archivo (parte continua)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Respuesta descrifrado=0", fpLog);
		}


		// ---------------------------------------------
		// paso de EBCDIC to ASCII, para poder grabar en el archivo
		// ---------------------------------------------
		if (argFuenteEbcdic == 1)
		{
			respuestaInt = translate((char *)grupoRegDestino, regPorBloqueLectura, argTamRegistro);
			respuestaInt = 0;
			if (respuestaInt != 0)
			{
				Escribir("Error pasando datos de EBCDIC to ASCCI (parte continua)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return respuestaInt;

			}
			Escribir("EBCDIC exitoso", fpLog);
		}

		// ---------------------------------------------
		// colocamos enter a la data desencriptada
		// ---------------------------------------------
		respuestaInt = segmentar((char *)grupoRegDestinoEnter, (char *)grupoRegDestino, regPorBloqueLectura, argTamRegistro);
		respuestaInt = 0;
		if (respuestaInt != 0)
		{
			Escribir("Error segmentando data desencriptada (parte continua)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Segmentado exitoso", fpLog);
		}

		try
		{
			if (fwrite(grupoRegDestinoEnter, sizeof(unsigned char), tamBloqueLectura + regPorBloqueLectura, fpDestino) != (tamBloqueLectura + regPorBloqueLectura))
			{
				Escribir("Error escribiendo data desencriptada (parte continua)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return ERR_ESCRIBIDATA_DESENCRIP;
			}
		}
		catch (...)
		{
			Escribir("Error escribiendo data desencriptada (parte continua)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_ESCRIBIDATA_DESENCRIP;
		}
	}

	//--------------------------------------------------------------------------------------------------
	//--------------------------------------------------------------------------------------------------

	if (tamBloqueLecturaResiduo != 0)
	{
		// ---------------------------------------------
		// limpiamos contenido de variables de memoria
		// ---------------------------------------------
		memset((char*)grupoRegOrigen, '\0', tamBloqueLectura);
		memset((char*)grupoRegDestino, '\0', tamBloqueLectura);
		memset((char*)grupoRegDestinoEnter, '\0', tamBloqueLectura + regPorBloqueLectura);

		// ---------------------------------------------
		// leemos grupo de registros en grupoRegOrigen
		// ---------------------------------------------
		fread(grupoRegOrigen, sizeof(CK_BYTE), tamBloqueLecturaResiduo, fpOrigen);
		Escribir("fread ejecutado", fpLog);

		// ---------------------------------------------
		// invocamos servicio de desencriptado
		// ---------------------------------------------
		respuestaInt = TDes(
			grupoRegOrigen,
			tamBloqueLecturaResiduo,
			argTamRegistro,
			(CK_BYTE_PTR)argLlaves,
			argEncriptar,
			grupoRegDestino,
			fpLog,
			modoCifrado);

		if (respuestaInt != 0)
		{
			Escribir("Error desencriptando archivo (parte residual)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Respuesta descrifrado=0", fpLog);
		}

		// ---------------------------------------------
		// paso de EBCDIC to ASCII, para poder grabar en el archivo
		// ---------------------------------------------
		if (argFuenteEbcdic == 1)
		{
			respuestaInt = translate((char *)grupoRegDestino, regPorBloqueLectura, argTamRegistro);
			respuestaInt = 0;
			if (respuestaInt != 0)
			{
				Escribir("Error pasando datos de EBCDIC to ASCCI (parte residual)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return respuestaInt;
			}
			Escribir("EBCDIC Exitoso", fpLog);
		}

		// ---------------------------------------------
		// colocamos enter a la data desencriptada
		// ---------------------------------------------
		int nroRegistroResiduos = tamBloqueLecturaResiduo / argTamRegistro;
		respuestaInt = segmentar((char *)grupoRegDestinoEnter, (char *)grupoRegDestino, nroRegistroResiduos, argTamRegistro);
		respuestaInt = 0;
		if (respuestaInt != 0)
		{
			Escribir("Error segmentando data desencriptada (parte residual)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Segmentado exitoso", fpLog);
		}

		try
		{
			if (fwrite(grupoRegDestinoEnter, sizeof(unsigned char), tamBloqueLecturaResiduo + nroRegistroResiduos, fpDestino) != (tamBloqueLecturaResiduo + nroRegistroResiduos))
			{
				Escribir("Error escribiendo data desencriptada (parse residual)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return ERR_ESCRIBIDATA_DESENCRIP;
			}
		}
		catch (...)
		{
			Escribir("Error escribiendo data desencriptada (parse residual)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_ESCRIBIDATA_DESENCRIP;
		}
	}

	Escribir("Archivos y memoria comienzo para cerrar", fpLog);
	CerrarArchivo(fpOrigen, fpDestino, fpLog);
	LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
	return NO_ERROR;
}

/*====================================================================
Esta funcion lee el archivo origen de la ruta especificada por *fuente,
lo procesa (cifra/decifra) mediante el algoritmo 3DES utilizando las
llaves contenidas en llaves(llave_A, y graba el resultado
en el archivo especificado en la ruta especificada por *destino. La funciÛn
devuelve cero (0) si la operaciÛn se realizÛ completa, y uno (1) en caso
contrario.

encrip=0-> descifrar;
encrip=1-> cifrar;
ebcdic=1->los datos fueron cifrados en abcdic y hace falta traducir
ebcdic=0->los datos fueron cifrados en ascii y no hace falta traducir
modoCifrado=0 -> Usa modo ECB
modoCifrado=1 -> Usa modo CBC, con IV = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}
====================================================================*/
DLLDIR int STACK_CALL DecryptTest(char* argArchivoFuente,
	char* argArchivoDestino,
	char* tamArchivoFuenteArg,
	char* tamRegistroArg,
	char* llavesArg,
	char* encriptarArg,
	char* fuenteEbcdicArg,
	char* modoCifradoArg,
	char* tokenname,
	char* passphrase,
	char* zmklabel
	)
{
	FILE *fpLog = NULL;
	FILE *fpOrigen = NULL;
	FILE *fpDestino = NULL;

	errno_t err1;
	errno_t err2;
	errno_t err3;

	CK_BYTE* grupoRegOrigen = NULL;
	CK_BYTE* grupoRegDestino = NULL;
	CK_BYTE* grupoRegDestinoEnter = NULL;

	long argTamArchivoFuente = atol(tamArchivoFuenteArg);
	CK_ULONG argTamRegistro = atol(tamRegistroArg);
	int argEncriptar = atoi(encriptarArg);
	int argFuenteEbcdic = atoi(fuenteEbcdicArg);
	int modoCifrado = atoi(modoCifradoArg);

	char argLlaves[32];
	strcpy(argLlaves, llavesArg);

	int respuestaInt = 0;
	int totBloquesLectura = 0;
	int regPorBloqueLectura = 100000;  //Capacidad maxima 100.000 registros por archivo

	int tamBloqueLectura = regPorBloqueLectura*argTamRegistro;
	//fprintf(fpLog, "Variable  tamBloqueLectura: %d\n", &tamBloqueLectura);
	int tamBloqueLecturaResiduo = 0;


	// ----------------------------------------------------------------
	// preparamos archivos origen, destino, log
	// ----------------------------------------------------------------	
	try
	{
		err1 = fopen_s(&fpOrigen, argArchivoFuente, "rb");
		if (err1 != 0)
		{
			return ERR_ABRIENDO_ARCHIVO;
		}

		err2 = fopen_s(&fpDestino, argArchivoDestino, "w+b");
		if (err2 != 0)
		{
			return ERR_CREANDO_ARCHIVO_DES;
		}

		err3 = fopen_s(&fpLog, "C:\\Windows\\Temp\\Decript.log", "w+b");
		if (err3 != 0)
		{
			return ERR_CREANDO_ARCHIVO_LOG;
		}

		Escribir("*****************Parametros de entrada***********************\n", fpLog);
		fprintf(fpLog, "Variable  argArchivoFuente: %s\n", argArchivoFuente);
		fprintf(fpLog, "Variable  argArchivoDestino: %s\n", argArchivoDestino);
		fprintf(fpLog, "Variable  tamArchivoFuenteArg: %s\n", tamArchivoFuenteArg);
		fprintf(fpLog, "Variable  tamRegistroArg: %s\n", tamRegistroArg);
		fprintf(fpLog, "Variable  llavesArg LLAVE: %s\n", llavesArg);
		fprintf(fpLog, "Variable  encriptarArg: %s\n", encriptarArg);
		fprintf(fpLog, "Variable  fuenteEbcdicArgr: %s\n", fuenteEbcdicArg);
		fprintf(fpLog, "Variable  modoCifradoArg: %s\n", modoCifradoArg);
		fprintf(fpLog, "Variable  tokenname: %s\n", tokenname);
		fprintf(fpLog, "Variable  passphrase: %s\n", passphrase);
		fprintf(fpLog, "Variable  zmklabel: %s\n", zmklabel);


		// get start time
		struct _timeb startTime;
		char *timeline;

		_ftime(&startTime);
		timeline = ctime(&(startTime.time));



		Escribir("*********************************************************", fpLog);
		fwrite("Fecha del reporte: ", sizeof(char), 19, fpLog);
		Escribir(timeline, fpLog);
		Escribir("Archivo procesado: ", fpLog);
		Escribir(argArchivoFuente, fpLog);
		Escribir("*********************************************************", fpLog);



	}
	catch (...)
	{
		Escribir("Error preparando archivos (origen,destino,log)", fpLog);
		CerrarArchivo(fpOrigen, fpDestino, fpLog);
		return ERR_ABRIENDO_ARCHIVO;
	}

	// ----------------------------------------------------------------
	// determinamos total bloques de lecturas (continuos y residuales)
	// ----------------------------------------------------------------
	try
	{
		if (argTamArchivoFuente <= tamBloqueLectura)
		{
			totBloquesLectura = 0;
			tamBloqueLecturaResiduo = argTamArchivoFuente;
		}
		else
		{
			totBloquesLectura = argTamArchivoFuente / tamBloqueLectura;
			tamBloqueLecturaResiduo = argTamArchivoFuente - (totBloquesLectura*tamBloqueLectura);
		}

		fprintf(fpLog, "argTamRegistro: %d", argTamRegistro);

	}
	catch (...)
	{
		Escribir("Error determinando tamano bloque de lectura", fpLog);
		CerrarArchivo(fpOrigen, fpDestino, fpLog);
		return ERR_DETERMINA_BLOKLECTURA;
	}
	Escribir("Determinando bloques de lectura", fpLog);

	// ----------------------------------------------------------------
	// reservamos espacio de memoria
	// ----------------------------------------------------------------	
	try
	{
		if ((grupoRegOrigen = (CK_BYTE*)malloc(tamBloqueLectura)) == NULL)
		{
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_MEMORIA_INSUFICIENTE;
		}

		if ((grupoRegDestino = (CK_BYTE*)malloc(tamBloqueLectura)) == NULL)
		{
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_MEMORIA_INSUFICIENTE;
		}

		if ((grupoRegDestinoEnter = (CK_BYTE*)malloc(tamBloqueLectura + regPorBloqueLectura)) == NULL)
		{
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_MEMORIA_INSUFICIENTE;
		}
	}
	catch (...)
	{
		Escribir("Error reservando espacio de memoria", fpLog);
		CerrarArchivo(fpOrigen, fpDestino, fpLog);
		return ERR_RESERVANDO_MEMORIA;
	}

	Escribir("Memoria reservada", fpLog);

	// ----------------------------------------------------------------
	// desencriptamos grupo de registros continuos
	// ----------------------------------------------------------------
	for (int i = 0; i < totBloquesLectura; i++)
	{
		// ---------------------------------------------
		// limpiamos contenido de variables de memoria
		// ---------------------------------------------

		//strset((char*)grupoRegOrigen,'\0');
		//strset((char*)grupoRegDestino,'\0');
		//strset((char*)grupoRegDestinoEnter,'\0');

		memset((char*)grupoRegOrigen, '0', tamBloqueLectura);
		memset((char*)grupoRegDestino, '0', tamBloqueLectura);
		memset((char*)grupoRegDestinoEnter, '0', tamBloqueLectura + regPorBloqueLectura);

		// ---------------------------------------------
		// leemos grupo de registros en grupoRegOrigen
		// ---------------------------------------------
		fread(grupoRegOrigen, sizeof(char), tamBloqueLectura, fpOrigen);
		Escribir("fread completado", fpLog);

		// ---------------------------------------------
		// invocamos servicio de desencriptado
		// ---------------------------------------------
		respuestaInt = TDesCopy(
			grupoRegOrigen,
			tamBloqueLectura,
			argTamRegistro,
			(CK_BYTE_PTR)argLlaves,
			argEncriptar,
			grupoRegDestino,
			fpLog,
			modoCifrado,
			tokenname,
			passphrase,
			zmklabel
			);


		if (respuestaInt != 0)
		{
			Escribir("Error desencriptando archivo (parte continua)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Respuesta descrifrado=0", fpLog);
		}


		// ---------------------------------------------
		// paso de EBCDIC to ASCII, para poder grabar en el archivo
		// ---------------------------------------------
		if (argFuenteEbcdic == 1)
		{
			respuestaInt = translate((char *)grupoRegDestino, regPorBloqueLectura, argTamRegistro);
			respuestaInt = 0;
			if (respuestaInt != 0)
			{
				Escribir("Error pasando datos de EBCDIC to ASCCI (parte continua)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return respuestaInt;

			}
			Escribir("EBCDIC exitoso", fpLog);
		}

		// ---------------------------------------------
		// colocamos enter a la data desencriptada
		// ---------------------------------------------
		respuestaInt = segmentar((char *)grupoRegDestinoEnter, (char *)grupoRegDestino, regPorBloqueLectura, argTamRegistro);
		respuestaInt = 0;
		if (respuestaInt != 0)
		{
			Escribir("Error segmentando data desencriptada (parte continua)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Segmentado exitoso", fpLog);
		}

		try
		{
			if (fwrite(grupoRegDestinoEnter, sizeof(unsigned char), tamBloqueLectura + regPorBloqueLectura, fpDestino) != (tamBloqueLectura + regPorBloqueLectura))
			{
				Escribir("Error escribiendo data desencriptada (parte continua)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return ERR_ESCRIBIDATA_DESENCRIP;
			}
		}
		catch (...)
		{
			Escribir("Error escribiendo data desencriptada (parte continua)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_ESCRIBIDATA_DESENCRIP;
		}
	}

	//--------------------------------------------------------------------------------------------------
	//--------------------------------------------------------------------------------------------------

	if (tamBloqueLecturaResiduo != 0)
	{
		// ---------------------------------------------
		// limpiamos contenido de variables de memoria
		// ---------------------------------------------
		memset((char*)grupoRegOrigen, '\0', tamBloqueLectura);
		memset((char*)grupoRegDestino, '\0', tamBloqueLectura);
		memset((char*)grupoRegDestinoEnter, '\0', tamBloqueLectura + regPorBloqueLectura);

		// ---------------------------------------------
		// leemos grupo de registros en grupoRegOrigen
		// ---------------------------------------------
		fread(grupoRegOrigen, sizeof(CK_BYTE), tamBloqueLecturaResiduo, fpOrigen);
		Escribir("fread ejecutado", fpLog);

		// ---------------------------------------------
		// invocamos servicio de desencriptado
		// ---------------------------------------------
		respuestaInt = TDesCopy(
			grupoRegOrigen,
			tamBloqueLecturaResiduo,
			argTamRegistro,
			(CK_BYTE_PTR)argLlaves,
			argEncriptar,
			grupoRegDestino,
			fpLog,
			modoCifrado,
			tokenname,
			passphrase,
			zmklabel);

		if (respuestaInt != 0)
		{
			Escribir("Error desencriptando archivo (parte residual)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Respuesta descrifrado=0", fpLog);
		}

		// ---------------------------------------------
		// paso de EBCDIC to ASCII, para poder grabar en el archivo
		// ---------------------------------------------
		if (argFuenteEbcdic == 1)
		{
			respuestaInt = translate((char *)grupoRegDestino, regPorBloqueLectura, argTamRegistro);
			respuestaInt = 0;
			if (respuestaInt != 0)
			{
				Escribir("Error pasando datos de EBCDIC to ASCCI (parte residual)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return respuestaInt;
			}
			Escribir("EBCDIC Exitoso", fpLog);
		}

		// ---------------------------------------------
		// colocamos enter a la data desencriptada
		// ---------------------------------------------
		int nroRegistroResiduos = tamBloqueLecturaResiduo / argTamRegistro;
		respuestaInt = segmentar((char *)grupoRegDestinoEnter, (char *)grupoRegDestino, nroRegistroResiduos, argTamRegistro);
		respuestaInt = 0;
		if (respuestaInt != 0)
		{
			Escribir("Error segmentando data desencriptada (parte residual)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return respuestaInt;
		}
		else
		{
			Escribir("Segmentado exitoso", fpLog);
		}

		try
		{
			if (fwrite(grupoRegDestinoEnter, sizeof(unsigned char), tamBloqueLecturaResiduo + nroRegistroResiduos, fpDestino) != (tamBloqueLecturaResiduo + nroRegistroResiduos))
			{
				Escribir("Error escribiendo data desencriptada (parse residual)", fpLog);
				CerrarArchivo(fpOrigen, fpDestino, fpLog);
				LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
				return ERR_ESCRIBIDATA_DESENCRIP;
			}
		}
		catch (...)
		{
			Escribir("Error escribiendo data desencriptada (parse residual)", fpLog);
			CerrarArchivo(fpOrigen, fpDestino, fpLog);
			LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
			return ERR_ESCRIBIDATA_DESENCRIP;
		}
	}

	Escribir("Archivos y memoria comienzo para cerrar", fpLog);
	CerrarArchivo(fpOrigen, fpDestino, fpLog);
	LiberarMemoria(grupoRegOrigen, grupoRegDestino, grupoRegDestinoEnter);
	return NO_ERROR;
}

//Cambios Dll nueva
DLLDIR int STACK_CALL INICIAR_PKCS11()
{
	//Variables locales
	CK_RV rv = CKR_OK;
	CK_C_INITIALIZE_ARGS init_args = {
		NULL_PTR,
		NULL_PTR,
		NULL_PTR,
		NULL_PTR,
		CKF_OS_LOCKING_OK,
		NULL_PTR };

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);
	errno_t err3;
	err3 = fopen_s(&FLOG, "C:\\Windows\\Temp\\Decript.log", "w+b");
	//open_log();

	fprintf(FLOG, "%s >>>INICIALIZA()>>>\n", time_str);

	if (!initialized) {
		//rv = functions_init();
		rv = CKR_OK;
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error functions_init %d\n", time_str, rv);
			rv = INICIA_ERROR;
			goto err;
		}
		initialized = true;
	}

	rv = C_Initialize((CK_VOID_PTR)&init_args);
	if (rv != CKR_OK) {
		fprintf(FLOG, "%s Error C_Initialize 0x%08x\n", time_str, rv);
		rv = INICIA_ERROR;
		goto err;
	}

err:
	fprintf(FLOG, "%s <<<INICIALIZA()<<<\n", time_str);
	//close_log();
	CerrarArchivoLog(FLOG);
	return rv;
}
DLLDIR int STACK_CALL INICIAR_RECURSOS(char *token_name, char *passphrase, char *key_label, char *out_hSession, char *out_hKey)
{
	//variables locales
	CK_RV rv = CKR_OK;
	CK_ULONG i, j, nslots;
	CK_SLOT_ID_PTR pslots = 0;
	CK_SLOT_ID hSlot;
	CK_SLOT_INFO sinfo;
	CK_TOKEN_INFO tinfo;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE hObject;
	CK_ULONG ulObjectCount;
	CK_ULONG login_success = 0;
	CK_ULONG key_found = 0;
	char *local_token_name[32 + 1];
	char pObjectLabel[1024];
	CK_ATTRIBUTE get_label_template[] = { { CKA_LABEL, NULL_PTR, 0 } };

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);

	open_log();

	fprintf(FLOG, "%s >>>INICIAR_RECURSOS()>>>\n", time_str);
	rv = C_GetSlotList(0, NULL_PTR, &nslots);
	if (rv != CKR_OK) {
		fprintf(FLOG, "%s Error C_GetSlotList 0x%08x\n", time_str, rv);
		rv = INICIA_ERROR;
		goto err;
	}

	pslots = (CK_SLOT_ID_PTR)malloc(sizeof(CK_SLOT_ID)* nslots);
	rv = C_GetSlotList(1, pslots, &nslots);
	if (rv != CKR_OK) {
		fprintf(FLOG, "%s Error C_GetSlotList 0x%08x\n", time_str, rv);
		rv = INICIA_ERROR;
		goto err;
	}

	for (i = 0; !login_success && i < nslots; i++) {
		hSlot = pslots[i];

		rv = C_GetSlotInfo(hSlot, &sinfo);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_GetSlotInfo 0x%08x\n", time_str, rv);
			goto err;
		}

		rv = C_GetTokenInfo(hSlot, &tinfo);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_GetTokenInfo 0x%08x\n", time_str, rv);
			goto err;
		}

		/* Skip write protected slots */
		if (tinfo.flags & CKF_WRITE_PROTECTED) {
			continue;
		}

		/* If we have a PIN, skip slots that don't accept it */
		if (passphrase && !(tinfo.flags & CKF_USER_PIN_INITIALIZED)) {
			continue;
		}

		/* The token label must match */
		memset(local_token_name, 0x00, sizeof(local_token_name));
		memcpy(local_token_name, tinfo.label, 32);
		local_token_name[32] = '\0';

		fprintf(FLOG, "%s token_name='%s', local_token_name='%s'\n", time_str, token_name, local_token_name);
		if (strncmp((char*)token_name, (char*)local_token_name, strlen(token_name)) != 0) {
			continue;
		}

		//CKF_RW_SESSION: in carwizard session can be read only
		rv = C_OpenSession(hSlot,
			CKF_SERIAL_SESSION,
			0, 0,
			&hSession);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_OpenSession 0x%08x\n", time_str, rv);
			rv = SESION_ERROR;
			goto err;
		}

		rv = C_Login(hSession, CKU_USER,
			(CK_CHAR_PTR)passphrase, (CK_ULONG)strlen((char *)passphrase));
		if (rv != CKR_OK && rv != CKR_USER_ALREADY_LOGGED_IN) {
			fprintf(FLOG, "%s Error C_Login 0x%08x\n", time_str, rv);
			rv = LOGIN_ERROR;
			goto err;
		}
		else {
			login_success = 1;
		}

		if (!login_success) {
			//continue to next slot
			continue;
		}

		//list token objects
		rv = C_FindObjectsInit(hSession, NULL_PTR, 0);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_FindObjectsInit 0x%08x\n", time_str, rv);
			rv = NOKEY_ERROR;
			goto err;
		}

		j = 0;
		while (1) {
			rv = C_FindObjects(hSession, &hObject, 1, &ulObjectCount);
			if (rv != CKR_OK || ulObjectCount == 0) break;

			//print object label attribute
			get_label_template[0].pValue = NULL_PTR;
			get_label_template[0].ulValueLen = 0;
			C_GetAttributeValue(hSession, hObject, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK || ulObjectCount == 0) break;

			memset(pObjectLabel, 0x00, sizeof(pObjectLabel));
			get_label_template[0].pValue = (CK_VOID_PTR)pObjectLabel;
			C_GetAttributeValue(hSession, hObject, get_label_template, NUM(get_label_template));

			if (rv != CKR_OK || ulObjectCount == 0) break;

			pObjectLabel[get_label_template[0].ulValueLen] = '\0';

			if (strcmp(pObjectLabel, key_label) == 0) {
				key_found = 1;
				break;
			}
		}

		rv = C_FindObjectsFinal(hSession);
	}

	if (login_success == 0) {
		rv = LOGIN_ERROR;
		goto err;
	}

	if (key_found == 0) {
		rv = NOKEY_ERROR;
		goto err;
	}

	fprintf(FLOG, "Handle Session: '%lu'\n", hSession);
	fprintf(FLOG, "Handle Key: '%lu'\n", hObject);
	sprintf(out_hSession, "%lu", hSession);
	sprintf(out_hKey, "%lu", hObject);

err:
	fprintf(FLOG, "%s ret: %d\n", time_str, rv);
	fprintf(FLOG, "%s <<<INICIAR_RECURSOS()<<<\n", time_str);
	close_log();
	if (pslots) {
		free(pslots);
	}

	return rv;
}
DLLDIR int STACK_CALL FINALIZAR_RECURSOS(char *in_hSession)
{
	//variables locales
	CK_RV rv = CKR_OK;
	CK_SESSION_HANDLE hSession;

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);
	errno_t err3;
	err3 = fopen_s(&FLOG, "C:\\Windows\\Temp\\Decript.log", "w+b");
	//open_log();

	fprintf(FLOG, "%s >>>FINALIZAR_RECURSOS()>>>\n", time_str);
	hSession = (CK_SESSION_HANDLE)atol(in_hSession);

	//rv = __DLL_C_Logout(hSession);
	//if(rv != CKR_OK) {
	//	fprintf(stderr, "Error __DLL_C_Logout 0x%08x\n", rv);
	//	rv = LOGOUT_ERROR;
	//	goto err;
	//}

	rv = C_CloseSession(hSession);
	if (rv != CKR_OK) {
		fprintf(FLOG, "%s Error C_CloseSession 0x%08x\n", time_str, rv);
		rv = SESION_CLS_ERROR;
		goto err;
	}

	fprintf(FLOG, "%s Close session: '%lu'\n", time_str, hSession);

err:
	fprintf(FLOG, "%s <<<FINALIZAR_RECURSOS()<<<\n", time_str);
	//close_log();
	CerrarArchivoLog(FLOG);
	return rv;
}
DLLDIR int STACK_CALL TDESCBC(char *in_hSession, char *in_hKey, char *data_in, char *data_out, char *mode, char *IV)
{
	// variables locales
	CK_RV rv = CKR_OK;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE hObject;
	CK_BYTE IV_BUFFER[8];
	char encrypt_data[1024];
	char clear_data[1024];
	CK_ULONG encrypt_data_len;
	CK_ULONG clear_data_len;
	CK_MECHANISM mechanismTDES_ECB;
	CK_MECHANISM mechanismTDES_CBC;

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);

	open_log();

	//fprintf(FLOG, "%s >>>TDES()>>>\n", time_str);
	if (!initialized) {
		rv = INICIA_ERROR;
		goto err;
	}

	str2hex((char*)IV_BUFFER, IV, strlen(IV));

	mechanismTDES_ECB = { CKM_DES3_ECB };
	mechanismTDES_CBC = { CKM_DES3_CBC, IV_BUFFER, sizeof(IV_BUFFER) };

	hSession = (CK_SESSION_HANDLE)atol(in_hSession);
	hObject = (CK_OBJECT_HANDLE)atol(in_hKey);

	memset(clear_data, 0x00, sizeof(clear_data));
	memset(encrypt_data, 0x00, sizeof(encrypt_data));

	if (strcmp(mode, "E") == 0) {
		str2hex(clear_data, data_in, strlen(data_in));
		rv = C_EncryptInit(hSession, &mechanismTDES_CBC, hObject);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error __DLL_C_EncryptInit 0x%08x\n", time_str, rv);
			rv = ENCRYPT_ERROR;
			goto err;
		}

		clear_data_len = strlen(data_in) / 2;
		encrypt_data_len = clear_data_len;

		rv = C_Encrypt(
			hSession,
			(CK_BYTE_PTR)clear_data,
			(CK_ULONG)clear_data_len,
			(CK_BYTE_PTR)encrypt_data,
			(CK_ULONG_PTR)&encrypt_data_len);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_Encrypt 0x%08x\n", time_str, rv);
			rv = ENCRYPT_ERROR;
			goto err;
		}
		hex2str(data_out, encrypt_data, encrypt_data_len);
	}
	else {
		str2hex(encrypt_data, data_in, strlen(data_in));

		rv = C_DecryptInit(hSession, &mechanismTDES_CBC, hObject);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_DecryptInit 0x%08x\n", time_str, rv);
			rv = DECRYPT_ERROR;
			goto err;
		}

		encrypt_data_len = strlen(data_in) / 2;
		clear_data_len = sizeof(clear_data);

		rv = C_Decrypt(
			hSession,
			(CK_BYTE_PTR)encrypt_data,
			(CK_ULONG)encrypt_data_len,
			(CK_BYTE_PTR)clear_data,
			(CK_ULONG_PTR)&clear_data_len);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_Decrypt 0x%08x\n", time_str, rv);
			rv = DECRYPT_ERROR;
			goto err;
		}

		hex2str(data_out, clear_data, clear_data_len);
	}

err:
	//fprintf(FLOG, "%s <<<TDES()<<<\n", time_str);
	close_log();

	return rv;
}
DLLDIR int STACK_CALL TDESECB(char *in_hSession, char *in_hKey, char *data_in, char *data_out, char *mode)
{
	// variables locales
	CK_RV rv = CKR_OK;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE hObject;
	CK_BYTE IV_BUFFER[8];
	char encrypt_data[1024];
	char clear_data[1024];
	CK_ULONG encrypt_data_len;
	CK_ULONG clear_data_len;
	CK_MECHANISM mechanismTDES_ECB;

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);

	open_log();

	//fprintf(FLOG, "%s >>>TDES()>>>\n", time_str);
	if (!initialized) {
		rv = INICIA_ERROR;
		goto err;
	}
	mechanismTDES_ECB = { CKM_DES3_ECB };

	hSession = (CK_SESSION_HANDLE)atol(in_hSession);
	hObject = (CK_OBJECT_HANDLE)atol(in_hKey);

	memset(clear_data, 0x00, sizeof(clear_data));
	memset(encrypt_data, 0x00, sizeof(encrypt_data));

	if (strcmp(mode, "E") == 0) {
		str2hex(clear_data, data_in, strlen(data_in));
		rv = C_EncryptInit(hSession, &mechanismTDES_ECB, hObject);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error __DLL_C_EncryptInit 0x%08x\n", time_str, rv);
			rv = ENCRYPT_ERROR;
			goto err;
		}

		clear_data_len = strlen(data_in) / 2;
		encrypt_data_len = clear_data_len;

		rv = C_Encrypt(
			hSession,
			(CK_BYTE_PTR)clear_data,
			(CK_ULONG)clear_data_len,
			(CK_BYTE_PTR)encrypt_data,
			(CK_ULONG_PTR)&encrypt_data_len);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_Encrypt 0x%08x\n", time_str, rv);
			rv = ENCRYPT_ERROR;
			goto err;
		}
		hex2str(data_out, encrypt_data, encrypt_data_len);
	}
	else {
		str2hex(encrypt_data, data_in, strlen(data_in));

		rv = C_DecryptInit(hSession, &mechanismTDES_ECB, hObject);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_DecryptInit 0x%08x\n", time_str, rv);
			rv = DECRYPT_ERROR;
			goto err;
		}

		encrypt_data_len = strlen(data_in) / 2;
		clear_data_len = sizeof(clear_data);

		rv = C_Decrypt(
			hSession,
			(CK_BYTE_PTR)encrypt_data,
			(CK_ULONG)encrypt_data_len,
			(CK_BYTE_PTR)clear_data,
			(CK_ULONG_PTR)&clear_data_len);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_Decrypt 0x%08x\n", time_str, rv);
			rv = DECRYPT_ERROR;
			goto err;
		}

		hex2str(data_out, clear_data, clear_data_len);
	}

err:
	//fprintf(FLOG, "%s <<<TDES()<<<\n", time_str);
	close_log();

	return rv;
}
DLLDIR int STACK_CALL AESECB(char *in_hSession, char *in_hKey, char *data_in, char *data_out, char *mode)
{
	// variables locales
	CK_RV rv = CKR_OK;
	CK_SESSION_HANDLE hSession;
	CK_OBJECT_HANDLE hObject;
	CK_BYTE IV_BUFFER[8];
	char encrypt_data[1024];
	char clear_data[1024];
	CK_ULONG encrypt_data_len;
	CK_ULONG clear_data_len;
	CK_MECHANISM mechanismAES_ECB;

	time_t t = time(NULL);
	struct tm * p = localtime(&t);
	char time_str[1024];
	strftime(time_str, sizeof(time_str), "[%Y-%m-%d %H:%M:%S] - ", p);

	open_log();

	//fprintf(FLOG, "%s >>>TDES()>>>\n", time_str);
	if (!initialized) {
		rv = INICIA_ERROR;
		goto err;
	}
	mechanismAES_ECB = { CKM_AES_ECB };

	hSession = (CK_SESSION_HANDLE)atol(in_hSession); 
	hObject = (CK_OBJECT_HANDLE)atol(in_hKey);

	memset(clear_data, 0x00, sizeof(clear_data));
	memset(encrypt_data, 0x00, sizeof(encrypt_data));

	if (strcmp(mode, "E") == 0) {
		str2hex64(clear_data, data_in, strlen(data_in));
		rv = C_EncryptInit(hSession, &mechanismAES_ECB, hObject);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error __DLL_C_EncryptInit 0x%08x\n", time_str, rv);
			rv = ENCRYPT_ERROR;
			goto err;
		}

		clear_data_len = strlen(data_in) / 2;
		encrypt_data_len = clear_data_len;

		rv = C_Encrypt(
			hSession,
			(CK_BYTE_PTR)clear_data,
			(CK_ULONG)clear_data_len,
			(CK_BYTE_PTR)encrypt_data,
			(CK_ULONG_PTR)&encrypt_data_len);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_Encrypt 0x%08x\n", time_str, rv);
			rv = ENCRYPT_ERROR;
			goto err;
		}
		hex2str64(data_out, encrypt_data, encrypt_data_len);
	}
	else {
		str2hex64(encrypt_data, data_in, strlen(data_in));

		rv = C_DecryptInit(hSession, &mechanismAES_ECB, hObject);
		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_DecryptInit 0x%08x\n", time_str, rv);
			rv = DECRYPT_ERROR;
			goto err;
		}

		encrypt_data_len = strlen(data_in) / 2;
		clear_data_len = sizeof(clear_data);

		rv = C_Decrypt(
			hSession,
			(CK_BYTE_PTR)encrypt_data,
			(CK_ULONG)encrypt_data_len,
			(CK_BYTE_PTR)clear_data,
			(CK_ULONG_PTR)&clear_data_len);

		if (rv != CKR_OK) {
			fprintf(FLOG, "%s Error C_Decrypt 0x%08x\n", time_str, rv);
			rv = DECRYPT_ERROR;
			goto err;
		}

		hex2str64(data_out, clear_data, clear_data_len);
	}

err:
	//fprintf(FLOG, "%s <<<TDES()<<<\n", time_str);
	close_log();

	return rv;
}

