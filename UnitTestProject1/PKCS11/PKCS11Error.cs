using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.PKCS11
{
    public class PKCS11Error
    {
        public static string getErrorDescription(Int64 res)
        {
            string response;
            switch (res)
            {
                case 0x1:
                    {
                        // CKR_CANCEL 0x00000001
                        response = "Operación cancelada (0x00000001)";
                        break;
                    }

                case 0x2:
                    {
                        // CKR_HOST_MEMORY 0x00000002
                        response = "Memoria insuficiente en el host (0x00000002)";
                        break;
                    }

                case 0x3:
                    {
                        // CKR_SLOT_ID_INVALID 0x00000003
                        response = "Id de slot inválido (0x00000003)";
                        break;
                    }

                case 0x5:
                    {
                        // CKR_GENERAL_ERROR 0x00000005
                        response = "Error general (0x00000005)";
                        break;
                    }

                case 0x6:
                    {
                        // CKR_FUNCTION_FAILED 0x00000006
                        response = "Función falló (0x00000006)";
                        break;
                    }

                case 0x7:
                    {
                        // CKR_ARGUMENTS_BAD 0x00000007
                        response = "Argumentos incorrectos (0x00000007)";
                        break;
                    }

                case 0x8:
                    {
                        // CKR_NO_EVENT 0x00000008
                        response = "Sin evento (0x00000008)";
                        break;
                    }

                case 0x9:
                    {
                        // CKR_NEED_TO_CREATE_THREADS 0x00000009
                        response = "Se necesitan crear hilos (0x00000009)";
                        break;
                    }

                case 0xA:
                    {
                        // CKR_CANT_LOCK 0x0000000A
                        response = "Imposible bloquear (0x0000000A)";
                        break;
                    }

                case 0x10:
                    {
                        // CKR_ATTRIBUTE_READ_ONLY 0x00000010
                        response = "Atributo de solo lectura (0x00000010)";
                        break;
                    }

                case 0x11:
                    {
                        // CKR_ATTRIBUTE_SENSITIVE 0x00000011
                        response = "Atributo sensible (0x00000011)";
                        break;
                    }

                case 0x12:
                    {
                        // CKR_ATTRIBUTE_TYPE_INVALID 0x00000012
                        response = "Tipo de atributo inválido (0x00000012)";
                        break;
                    }

                case 0x13:
                    {
                        // CKR_ATTRIBUTE_VALUE_INVALID 0x00000013
                        response = "Valor de atributo inválido (0x00000013)";
                        break;
                    }

                case 0x20:
                    {
                        // CKR_DATA_INVALID 0x00000020
                        response = "Dato invalido (0x00000020)";
                        break;
                    }

                case 0x21:
                    {
                        // CKR_DATA_LEN_RANGE 0x00000021
                        response = "Rango longitud del dato inválido (0x00000021)";
                        break;
                    }

                case 0x30:
                    {
                        // CKR_DEVICE_ERROR 0x00000030
                        response = "Error en dispositivo (0x00000030)";
                        break;
                    }

                case 0x31:
                    {
                        // CKR_DEVICE_MEMORY 0x00000031
                        response = "Memoria de dispositivo insuficiente (0x00000031)";
                        break;
                    }

                case 0x32:
                    {
                        // CKR_DEVICE_REMOVED 0x00000032
                        response = "Dispositivo removido (0x00000032)";
                        break;
                    }

                case 0x40:
                    {
                        // CKR_ENCRYPTED_DATA_INVALID 0x00000040
                        response = "Información encriptada inválida (0x00000040)";
                        break;
                    }

                case 0x41:
                    {
                        // CKR_ENCRYPTED_DATA_LEN_RANGE 0x00000041
                        response = "Rango de longitud de dato encriptado inválido (0x00000041)";
                        break;
                    }

                case 0x50:
                    {
                        // CKR_FUNCTION_CANCELED 0x00000050
                        response = "Función cancelada (0x00000050)";
                        break;
                    }

                case 0x51:
                    {
                        // CKR_FUNCTION_NOT_PARALLEL 0x00000051
                        response = "Función no es paralela (0x00000051)";
                        break;
                    }

                case 0x54:
                    {
                        // CKR_FUNCTION_NOT_SUPPORTED 0x00000054
                        response = "Función no soportada (0x00000054)";
                        break;
                    }

                case 0x60:
                    {
                        // CKR_KEY_HANDLE_INVALID 0x00000060
                        response = "La llave no existe (0x00000060)";
                        break;
                    }

                case 0x62:
                    {
                        // CKR_KEY_SIZE_RANGE 0x00000062
                        response = "Rango de tamaño de llave inválido (0x00000062)";
                        break;
                    }

                case 0x63:
                    {
                        // CKR_KEY_TYPE_INCONSISTENT 0x00000063
                        response = "Tipo de llave inconsistente (0x00000063)";
                        break;
                    }

                case 0x64:
                    {
                        // CKR_KEY_NOT_NEEDED 0x00000064
                        response = "Llave no necesaria (0x00000064)";
                        break;
                    }

                case 0x65:
                    {
                        // CKR_KEY_CHANGED 0x00000065
                        response = "Llave cambiada (0x00000065)";
                        break;
                    }

                case 0x66:
                    {
                        // CKR_KEY_NEEDED 0x00000066
                        response = "Se necesita llave (0x00000066)";
                        break;
                    }

                case 0x67:
                    {
                        // CKR_KEY_INDIGESTIBLE 0x00000067
                        response = "No se puede digerir llave (0x00000067)";
                        break;
                    }

                case 0x68:
                    {
                        // CKR_KEY_FUNCTION_NOT_PERMITTED 0x00000068
                        response = "Función no permitida en llave (0x00000068)";
                        break;
                    }

                case 0x69:
                    {
                        // CKR_KEY_NOT_WRAPPABLE 0x00000069
                        response = "No se permite envolver llave (0x00000069)";
                        break;
                    }

                case 0x6A:
                    {
                        // CKR_KEY_UNEXTRACTABLE 0x0000006A
                        response = "No se permite extraer llave (0x0000006A)";
                        break;
                    }

                case 0x70:
                    {
                        // CKR_MECHANISM_INVALID 0x00000070
                        response = "Mecanismo inválido (0x00000070)";
                        break;
                    }

                case 0x71:
                    {
                        // CKR_MECHANISM_PARAM_INVALID 0x00000071
                        response = "Mecanismo paralelo inválido (0x00000071)";
                        break;
                    }

                case 0x82:
                    {
                        // CKR_OBJECT_HANDLE_INVALID 0x00000082
                        response = "Manejador de objeto inválido (0x00000082)";
                        break;
                    }

                case 0x90:
                    {
                        // CKR_OPERATION_ACTIVE 0x00000090
                        response = "Operación activa (0x00000090)";
                        break;
                    }

                case 0x91:
                    {
                        // CKR_OPERATION_NOT_INITIALIZED 0x00000091
                        response = "Operación no inicializada (0x00000091)";
                        break;
                    }

                case 0xA0:
                    {
                        // CKR_PIN_INCORRECT 0x000000A0
                        response = "PIN incorrecto (0x000000A0)";
                        break;
                    }

                case 0xA1:
                    {
                        // CKR_PIN_INVALID 0x000000A1
                        response = "PIN inválido (0x000000A1)";
                        break;
                    }

                case 0xA2:
                    {
                        // CKR_PIN_LEN_RANGE 0x000000A2
                        response = "Rango de longitud de PIN inválido (0x000000A2)";
                        break;
                    }

                case 0xA3:
                    {
                        // CKR_PIN_EXPIRED 0x000000A3
                        response = "PIN expiró (0x000000A3)";
                        break;
                    }

                case 0xA4:
                    {
                        // CKR_PIN_LOCKED 0x000000A4
                        response = "PIN bloqueado (0x000000A4)";
                        break;
                    }

                case 0xB0:
                    {
                        // CKR_SESSION_CLOSED 0x000000B0
                        response = "Sesión cerrada (0x000000B0)";
                        break;
                    }

                case 0xB1:
                    {
                        // CKR_SESSION_COUNT 0x000000B1
                        response = "Contador de sesión inválido (0x000000B1)";
                        break;
                    }

                case 0xB3:
                    {
                        // CKR_SESSION_HANDLE_INVALID 0x000000B3
                        response = "Manejador de sesión inválido (0x000000B3)";
                        break;
                    }

                case 0xB4:
                    {
                        // CKR_SESSION_PARALLEL_NOT_SUPPORTED 0x000000B4
                        response = "Sesión paralela no soportada (0x000000B4)";
                        break;
                    }

                case 0xB5:
                    {
                        // CKR_SESSION_READ_ONLY 0x000000B5
                        response = "Sesión de solo lectura (0x000000B5)";
                        break;
                    }

                case 0xB6:
                    {
                        // CKR_SESSION_EXISTS 0x000000B6
                        response = "Sesión ya existe (0x000000B6)";
                        break;
                    }

                case 0xB7:
                    {
                        // CKR_SESSION_READ_ONLY_EXISTS 0x000000B7
                        response = "Sesión de solo lectura existe (0x000000B7)";
                        break;
                    }

                case 0xB8:
                    {
                        // CKR_SESSION_READ_WRITE_SO_EXISTS 0x000000B8
                        response = "Sesión de lectura y escritura ya existe (0x000000B8)";
                        break;
                    }

                case 0xC0:
                    {
                        // CKR_SIGNATURE_INVALID 0x000000C0
                        response = "Firma inválida (0x000000C0)";
                        break;
                    }

                case 0xC1:
                    {
                        // CKR_SIGNATURE_LEN_RANGE 0x000000C1
                        response = "Rango de longitud de firma inválido (0x000000C1)";
                        break;
                    }

                case 0xD0:
                    {
                        // CKR_TEMPLATE_INCOMPLETE 0x000000D0
                        response = "Plantilla incompleta (0x000000D0)";
                        break;
                    }

                case 0xD1:
                    {
                        // CKR_TEMPLATE_INCONSISTENT 0x000000D1
                        response = "Plantilla inconsistente (0x000000D1)";
                        break;
                    }

                case 0xE0:
                    {
                        // CKR_TOKEN_NOT_PRESENT 0x000000E0
                        response = "Token no presente (0x000000E0)";
                        break;
                    }

                case 0xE1:
                    {
                        // CKR_TOKEN_NOT_RECOGNIZED 0x000000E1
                        response = "Token no reconocido (0x000000E1)";
                        break;
                    }

                case 0xE2:
                    {
                        // CKR_TOKEN_WRITE_PROTECTED 0x000000E2
                        response = "Token protegido contra escritura (0x000000E2)";
                        break;
                    }

                case 0xF0:
                    {
                        // CKR_UNWRAPPING_KEY_HANDLE_INVALID 0x000000F0
                        response = "Manejador de llave desenvuelta inválido (0x000000F0)";
                        break;
                    }

                case 0xF1:
                    {
                        // CKR_UNWRAPPING_KEY_SIZE_RANGE 0x000000F1
                        response = "Rango de tamaño de llave desenvuelta inválido (0x000000F1)";
                        break;
                    }

                case 0xF2:
                    {
                        // CKR_UNWRAPPING_KEY_TYPE_INCONSISTENT 0x000000F2
                        response = "Tipo de llave desenvuelta inconsistente (0x000000F2)";
                        break;
                    }

                case 0x100:
                    {
                        // CKR_USER_ALREADY_LOGGED_IN 0x00000100
                        response = "Usuario ya logeado (0x00000100)";
                        break;
                    }

                case 0x101:
                    {
                        // CKR_USER_NOT_LOGGED_IN 0x00000101
                        response = "Usuario no logeado (0x00000101)";
                        break;
                    }

                case 0x102:
                    {
                        // CKR_USER_PIN_NOT_INITIALIZED 0x00000102
                        response = "PIN de usuario no inicializado (0x00000102)";
                        break;
                    }

                case 0x103:
                    {
                        // CKR_USER_TYPE_INVALID 0x00000103
                        response = "Tipo de usuario inválido (0x00000103)";
                        break;
                    }

                case 0x104:
                    {
                        // CKR_USER_ANOTHER_ALREADY_LOGGED_IN 0x00000104
                        response = "Usuario ya está logeado (0x00000104)";
                        break;
                    }

                case 0x105:
                    {
                        // CKR_USER_TOO_MANY_TYPES 0x00000105
                        response = "Muchos tipos de usuario (0x00000105)";
                        break;
                    }

                case 0x110:
                    {
                        // CKR_WRAPPED_KEY_INVALID 0x00000110
                        response = "Llave envuelta inválida (0x00000110)";
                        break;
                    }

                case 0x112:
                    {
                        // CKR_WRAPPED_KEY_LEN_RANGE 0x00000112
                        response = "Rango de longitud de llave envuelta inválido (0x00000112)";
                        break;
                    }

                case 0x113:
                    {
                        // CKR_WRAPPING_KEY_HANDLE_INVALID 0x00000113
                        response = "Manejador de llave envuelta inválido (0x00000113)";
                        break;
                    }

                case 0x114:
                    {
                        // CKR_WRAPPING_KEY_SIZE_RANGE 0x00000114
                        response = "Rango de tamaño de llave envuelta inválido (0x00000114)";
                        break;
                    }

                case 0x115:
                    {
                        // CKR_WRAPPING_KEY_TYPE_INCONSISTENT 0x00000115
                        response = "Tipo de llave envuelta inconsistente (0x00000115)";
                        break;
                    }

                case 0x120:
                    {
                        // CKR_RANDOM_SEED_NOT_SUPPORTED 0x00000120
                        response = "Semilla aleatorea no soportada (0x00000120)";
                        break;
                    }

                case 0x121:
                    {
                        // CKR_RANDOM_NO_RNG 0x00000121
                        response = "Error en generación de valor aleatoreo (0x00000121)";
                        break;
                    }

                case 0x150:
                    {
                        // CKR_BUFFER_TOO_SMALL 0x00000150
                        response = "Buffer muy pequeño (0x00000150)";
                        break;
                    }

                case 0x160:
                    {
                        // CKR_SAVED_STATE_INVALID 0x00000160
                        response = "Estado guardado inválido (0x00000160)";
                        break;
                    }

                case 0x170:
                    {
                        // CKR_INFORMATION_SENSITIVE 0x00000170
                        response = "Información sensible (0x00000170)";
                        break;
                    }

                case 0x180:
                    {
                        // CKR_STATE_UNSAVEABLE 0x00000180
                        response = "No se puede guardar estado (0x00000180)";
                        break;
                    }

                case 0x190:
                    {
                        // CKR_CRYPTOKI_NOT_INITIALIZED 0x00000190
                        response = "Criptoku no inicializado (0x00000190)";
                        break;
                    }

                case 0x191:
                    {
                        // CKR_CRYPTOKI_ALREADY_INITIALIZED 0x00000191
                        response = "Criptoki ya inicializado (0x00000191)";
                        break;
                    }

                case 0x1A0:
                    {
                        // CKR_MUTEX_BAD 0x000001A0
                        response = "Mutex incorrecto (0x000001A0)";
                        break;
                    }

                case 0x1A1:
                    {
                        // CKR_MUTEX_NOT_LOCKED 0x000001A1
                        response = "Mutex no bloqueado (0x000001A1)";
                        break;
                    }

                case 0x80000000:
                    {
                        // CKR_VENDOR_DEFINED 0x80000000
                        response = "Error definido por fabricante (0x80000000)";
                        break;
                    }

                case 0x8FFFFFFE:
                    {
                        // CKR_TOKEN_NOT_MATCHES 0x8FFFFFFE
                        response = "Tokens no coinciden (0x8FFFFFFE)";
                        break;
                    }

                case 0x8FFFFFFF:
                    {
                        // CKR_KEY_ALREADY_EXISTS 0x8FFFFFFF
                        response = "Llave ya existe (0x8FFFFFFF)";
                        break;
                    }

                case -1879048193:
                    {
                        // CKR_KEY_ALREADY_EXISTS 0x8FFFFFFF
                        response = "Llave ya existe (0x8FFFFFFF)";
                        break;
                    }

                default:
                    {
                        response = string.Format("PKCS#11 Error: 0x{0:X8}", res);
                        break;
                    }
            }
            return response;
        }
    }


}