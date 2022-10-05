#include <string>
using namespace std;

#ifdef __cplusplus
extern "C" {
#endif

#ifndef __NSHIELDKEYLOADER
#define __NSHIELDKEYLOADER 1

#include <windows.h>
#include "cryptoki.h"

#define STACK_CALL __cdecl

#define NUM(a) (sizeof(a) / sizeof((a)[0]))

#ifdef NSHIELDKEYLOADER_EXPORTS
#define DLLDIR  __declspec(dllexport)   // export DLL information
#else
#define DLLDIR  __declspec(dllimport)   // import DLL information
#endif 


#define	COMPONENT_SIZE 16
#define IV_LEN 8

	/**
	CODIGOS DE ERROR
	*/
# define NO_ERROR_					0
# define ERR_ABRIENDO_ARCHIVO		1
# define ERR_ESCRIBIENDO_ARCHIVO	2
# define ERR_CREANDO_ARCHIVO_DES	3
# define ERR_CREANDO_ARCHIVO_LOG	4
# define ERR_MEMORIA_INSUFICIENTE	5

# define ERR_INICIANDO_LIBPKCS11	10
# define ERR_GETSLOT_LIST			11
# define ERR_NUMSLOT_INVALIDO		12

# define ERR_INICIANDO_SESION		13
# define ERR_INICIANDO_LOGIN		14
# define ERR_INICIANDO_FIND_ZMK		15
# define ERR_BUSCANDO_ZMK			16
# define ERR_FINALIZANDO_FIND_ZMK	17

# define ERR_CANTIDAD_ZMK_INVALIDAS	18
# define ERR_CREANDO_MANEJADOR		19
# define ERR_DESENCRIPTANDO_LLAVE	20
# define ERR_INICIANDO_ENCRIPCION	21
# define ERR_ENCRIPTANDO_DATA		22
# define ERR_INICIANDO_DEENCRIPCION	23
# define ERR_DESENCRIPTANDO_DATA	24
# define ERR_DESTRUYENDO_OBJ		25
# define ERR_GETLLAVE_DESENCRIPTADA 26
# define ERR_DESENCRIPTANDO         27
# define ERR_ENCRIPTANDO_LLAVE      28

# define ERR_RESERVANDO_MEMORIA		30
# define ERR_LIBERANDO_MEMORIA		31
# define ERR_CERRANDO_ARCHIVO		32
# define ERR_DETERMINA_BLOKLECTURA  33
# define ERR_EBCDIC_TO_ASCII		34
# define ERR_SEGMENTANDO			35
# define ERR_ESCRIBIDATA_DESENCRIP	36
	// Nuevos errores (IDENPLA Colombia)

# define ERR_GETSLOT_INFO			37
# define ERR_GETTOKEN_INFO			38
# define ERR_GETCKA_VALUE			39
# define ERR_WRAPPING_KEY			40
# define ERR_INVALID_ZMKLEN			41
# define ERR_CARGANDO_ZMK			42
# define ERR_ALREADY_EXIST_ZMK		43
# define ERR_BAD_KEY_LENGHT			44
# define ERR_NO_AUTH_CARD			45

# define ERR_EXCEPTION			    100

	//Nuevos errores
	// ERROR CODES
#define INICIA_ERROR		1001
#define SESION_ERROR		1002
#define LOGIN_ERROR			1003
#define NOKEY_ERROR			1004
#define ENCRYPT_ERROR		1005
#define DECRYPT_ERROR		1006
#define LOGOUT_ERROR		1007
#define SESION_CLS_ERROR	1008

	// PIN ERROR CODES
#define PIN_SECUENCE		2001
#define PIN_SAME_DIGIT		2002
#define PIN_LEN				2003
#define PINS_NO_EQUALS		2004


#define MAX_OBJS	(128)
#define MAX_SLOTS	(128)

	extern CK_BBOOL true_val;
	extern CK_BBOOL false_val;

	extern CK_OBJECT_CLASS class_public;
	extern CK_OBJECT_CLASS class_private;
	extern CK_OBJECT_CLASS class_secret;
	extern CK_OBJECT_CLASS class_data;
	extern CK_OBJECT_CLASS class_cert;
	extern CK_OBJECT_CLASS class_domain;

	/**
	* LOAD_KEY: función que carga la llave ZMK (3DES de doble longitud) en un nshield
	*/
	DLLDIR int STACK_CALL LOAD_ZMK(char *C1, char *C2, char *C3, char *tokenname, char *passphrase, char *zmklabel);

	/**
	* KCV_ZMK: función que calcula el KCV de la llave ZMK, según el zmklabel pasado por parámetro
	*/
	DLLDIR int STACK_CALL KCV_ZMK(char *tokenname, char *passphrase, char *zmklabel, char *KCV);

	/**
	* GENERATE_KEY: función que genera una llave 3DES (3DES de doble longitud) en un nshield
	*/
	DLLDIR int STACK_CALL GENERATE_KEY(char *C1, char *C2, char *C3, char *tokenname, char *passphrase);

	//DLLDIR int STACK_CALL KCV_ZMK_TEST(char *tokenname, char *passphrase, string *llave[3]);


	DLLDIR int STACK_CALL KCV_ZMK_TEST(char *tokenname, char *passphrase);


	DLLDIR int STACK_CALL Decrypt(char* argArchivoFuente,
		char* argArchivoDestino,
		char* tamArchivoFuenteArg,
		char* tamRegistroArg,
		char* llavesArg,
		char* encriptarArg,
		char* fuenteEbcdicArg,
		char* modoCifradoArg);

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
		char* zmklabel);

	/**
	* INICIALIZA_PKCS11: carga las funciones PKCS11 directamente de la cryptoki.dll
	* llama a la función para inicilizar las estructura en memoria de PKCS11
	* Esta función solo debe ser llamada una vez por proceso
	*/
	DLLDIR int STACK_CALL INICIAR_PKCS11();
	/**
	* INICIAR_RECURSOS: Inicializa los recursos tales como sesión pkcs11, login, y busqueda de llave
	* token_name:		nombre del token
	* passphrase:		passphrase del token
	* bdk_label:		etiqueta de la bdk
	* panek_label:		etiqueta de la panek (llave para encriptado de PAN)
	* out_hSession:	manejador de la sesion abierta por esta funcion
	* out_hBdk:		manejador de la llave bdk
	* out_hPanek:		manejador de la llave panek
	*/
	DLLDIR int STACK_CALL INICIAR_RECURSOS(char *token_name, char *passphrase, char *key_label, char *out_hSession, char *out_hKey);
	/**
	* FINALIZAR_RECURSOS: Finaliza los recursos reservados por el hilo, tales como la sesión pkcs11, logout, llave abierta
	* in_hSession: el handler de la sesión abierta por el hilo
	*/
	DLLDIR int STACK_CALL FINALIZAR_RECURSOS(char *in_hSession);
	/**
	* TDES:		función que se encarga de encriptar/desencriptar datos haciendo llamadas a funciones usando el modo de cifrado CBC
	*			PKCS#11 del SafeNet. Siempre se emplea el modo CBC
	* in_hSession:		handler de la sesion abierta por el hilo
	* in_hKey:			handler de la llave encontrada por el hilo
	* data_in:			data de entrada a operar (hex-string)
	* data_out:			data de salida (hex-string)
	* modo:				"E": encriptar, "D": desencriptar
	* IV:				Verctor de inicialización (hex-string)
	*/
	DLLDIR int STACK_CALL TDESCBC(char *in_hSession, char *in_hKey, char *data_in, char *data_out, char *mode, char *IV);
	/**
	* TDES:		función que se encarga de encriptar/desencriptar datos haciendo llamadas a funciones usando el modo de cifrado ECB
	*			PKCS#11 del SafeNet. Siempre se emplea el modo CBC
	* in_hSession:		handler de la sesion abierta por el hilo
	* in_hKey:			handler de la llave encontrada por el hilo
	* data_in:			data de entrada a operar (hex-string)
	* data_out:			data de salida (hex-string)
	* modo:				"E": encriptar, "D": desencriptar
	*/
	DLLDIR int STACK_CALL TDESECB(char *in_hSession, char *in_hKey, char *data_in, char *data_out, char *mode);
	DLLDIR int STACK_CALL AESECB(char *in_hSession, char *in_hKey, char *data_in, char *data_out, char *mode);



#endif /*__NSHIELDKEYLOADER*/

#ifdef __cplusplus
}
#endif