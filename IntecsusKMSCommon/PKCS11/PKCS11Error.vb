Public Class PKCS11Error
    Public Shared Function getErrorDescription(res As Integer) As String
        Dim response As String
        Select Case res
            Case &H1
                ' CKR_CANCEL 0x00000001
                response = "Operación cancelada (0x00000001)"
            Case &H2
                ' CKR_HOST_MEMORY 0x00000002
                response = "Memoria insuficiente en el host (0x00000002)"
            Case &H3
                ' CKR_SLOT_ID_INVALID 0x00000003
                response = "Id de slot inválido (0x00000003)"
            Case &H5
                ' CKR_GENERAL_ERROR 0x00000005
                response = "Error general (0x00000005)"
            Case &H6
                ' CKR_FUNCTION_FAILED 0x00000006
                response = "Función falló (0x00000006)"
            Case &H7
                ' CKR_ARGUMENTS_BAD 0x00000007
                response = "Argumentos incorrectos (0x00000007)"
            Case &H8
                ' CKR_NO_EVENT 0x00000008
                response = "Sin evento (0x00000008)"
            Case &H9
                ' CKR_NEED_TO_CREATE_THREADS 0x00000009
                response = "Se necesitan crear hilos (0x00000009)"
            Case &HA
                ' CKR_CANT_LOCK 0x0000000A
                response = "Imposible bloquear (0x0000000A)"
            Case &H10
                ' CKR_ATTRIBUTE_READ_ONLY 0x00000010
                response = "Atributo de solo lectura (0x00000010)"
            Case &H11
                ' CKR_ATTRIBUTE_SENSITIVE 0x00000011
                response = "Atributo sensible (0x00000011)"
            Case &H12
                ' CKR_ATTRIBUTE_TYPE_INVALID 0x00000012
                response = "Tipo de atributo inválido (0x00000012)"
            Case &H13
                ' CKR_ATTRIBUTE_VALUE_INVALID 0x00000013
                response = "Valor de atributo inválido (0x00000013)"
            Case &H20
                ' CKR_DATA_INVALID 0x00000020
                response = "Dato invalido (0x00000020)"
            Case &H21
                ' CKR_DATA_LEN_RANGE 0x00000021
                response = "Rango longitud del dato inválido (0x00000021)"
            Case &H30
                ' CKR_DEVICE_ERROR 0x00000030
                response = "Error en dispositivo (0x00000030)"
            Case &H31
                ' CKR_DEVICE_MEMORY 0x00000031
                response = "Memoria de dispositivo insuficiente (0x00000031)"
            Case &H32
                ' CKR_DEVICE_REMOVED 0x00000032
                response = "Dispositivo removido (0x00000032)"
            Case &H40
                ' CKR_ENCRYPTED_DATA_INVALID 0x00000040
                response = "Información encriptada inválida (0x00000040)"
            Case &H41
                ' CKR_ENCRYPTED_DATA_LEN_RANGE 0x00000041
                response = "Rango de longitud de dato encriptado inválido (0x00000041)"
            Case &H50
                ' CKR_FUNCTION_CANCELED 0x00000050
                response = "Función cancelada (0x00000050)"
            Case &H51
                ' CKR_FUNCTION_NOT_PARALLEL 0x00000051
                response = "Función no es paralela (0x00000051)"
            Case &H54
                ' CKR_FUNCTION_NOT_SUPPORTED 0x00000054
                response = "Función no soportada (0x00000054)"
            Case &H60
                ' CKR_KEY_HANDLE_INVALID 0x00000060
                response = "Manejador de llave inválido (0x00000060)"
            Case &H62
                ' CKR_KEY_SIZE_RANGE 0x00000062
                response = "Rango de tamaño de llave inválido (0x00000062)"
            Case &H63
                ' CKR_KEY_TYPE_INCONSISTENT 0x00000063
                response = "Tipo de llave inconsistente (0x00000063)"
            Case &H64
                ' CKR_KEY_NOT_NEEDED 0x00000064
                response = "Llave no necesaria (0x00000064)"
            Case &H65
                ' CKR_KEY_CHANGED 0x00000065
                response = "Llave cambiada (0x00000065)"
            Case &H66
                ' CKR_KEY_NEEDED 0x00000066
                response = "Se necesita llave (0x00000066)"
            Case &H67
                ' CKR_KEY_INDIGESTIBLE 0x00000067
                response = "No se puede digerir llave (0x00000067)"
            Case &H68
                ' CKR_KEY_FUNCTION_NOT_PERMITTED 0x00000068
                response = "Función no permitida en llave (0x00000068)"
            Case &H69
                ' CKR_KEY_NOT_WRAPPABLE 0x00000069
                response = "No se permite envolver llave (0x00000069)"
            Case &H6A
                ' CKR_KEY_UNEXTRACTABLE 0x0000006A
                response = "No se permite extraer llave (0x0000006A)"
            Case &H70
                ' CKR_MECHANISM_INVALID 0x00000070
                response = "Mecanismo inválido (0x00000070)"
            Case &H71
                ' CKR_MECHANISM_PARAM_INVALID 0x00000071
                response = "Mecanismo paralelo inválido (0x00000071)"
            Case &H82
                ' CKR_OBJECT_HANDLE_INVALID 0x00000082
                response = "Manejador de objeto inválido (0x00000082)"
            Case &H90
                ' CKR_OPERATION_ACTIVE 0x00000090
                response = "Operación activa (0x00000090)"
            Case &H91
                ' CKR_OPERATION_NOT_INITIALIZED 0x00000091
                response = "Operación no inicializada (0x00000091)"
            Case &HA0
                ' CKR_PIN_INCORRECT 0x000000A0
                response = "PIN incorrecto (0x000000A0)"
            Case &HA1
                ' CKR_PIN_INVALID 0x000000A1
                response = "PIN inválido (0x000000A1)"
            Case &HA2
                ' CKR_PIN_LEN_RANGE 0x000000A2
                response = "Rango de longitud de PIN inválido (0x000000A2)"
            Case &HA3
                ' CKR_PIN_EXPIRED 0x000000A3
                response = "PIN expiró (0x000000A3)"
            Case &HA4
                ' CKR_PIN_LOCKED 0x000000A4
                response = "PIN bloqueado (0x000000A4)"
            Case &HB0
                ' CKR_SESSION_CLOSED 0x000000B0
                response = "Sesión cerrada (0x000000B0)"
            Case &HB1
                ' CKR_SESSION_COUNT 0x000000B1
                response = "Contador de sesión inválido (0x000000B1)"
            Case &HB3
                ' CKR_SESSION_HANDLE_INVALID 0x000000B3
                response = "Manejador de sesión inválido (0x000000B3)"
            Case &HB4
                ' CKR_SESSION_PARALLEL_NOT_SUPPORTED 0x000000B4
                response = "Sesión paralela no soportada (0x000000B4)"
            Case &HB5
                ' CKR_SESSION_READ_ONLY 0x000000B5
                response = "Sesión de solo lectura (0x000000B5)"
            Case &HB6
                ' CKR_SESSION_EXISTS 0x000000B6
                response = "Sesión ya existe (0x000000B6)"
            Case &HB7
                ' CKR_SESSION_READ_ONLY_EXISTS 0x000000B7
                response = "Sesión de solo lectura existe (0x000000B7)"
            Case &HB8
                ' CKR_SESSION_READ_WRITE_SO_EXISTS 0x000000B8
                response = "Sesión de lectura y escritura ya existe (0x000000B8)"
            Case &HC0
                ' CKR_SIGNATURE_INVALID 0x000000C0
                response = "Firma inválida (0x000000C0)"
            Case &HC1
                ' CKR_SIGNATURE_LEN_RANGE 0x000000C1
                response = "Rango de longitud de firma inválido (0x000000C1)"
            Case &HD0
                ' CKR_TEMPLATE_INCOMPLETE 0x000000D0
                response = "Plantilla incompleta (0x000000D0)"
            Case &HD1
                ' CKR_TEMPLATE_INCONSISTENT 0x000000D1
                response = "Plantilla inconsistente (0x000000D1)"
            Case &HE0
                ' CKR_TOKEN_NOT_PRESENT 0x000000E0
                response = "Token no presente (0x000000E0)"
            Case &HE1
                ' CKR_TOKEN_NOT_RECOGNIZED 0x000000E1
                response = "Token no reconocido (0x000000E1)"
            Case &HE2
                ' CKR_TOKEN_WRITE_PROTECTED 0x000000E2
                response = "Token protegido contra escritura (0x000000E2)"
            Case &HF0
                ' CKR_UNWRAPPING_KEY_HANDLE_INVALID 0x000000F0
                response = "Manejador de llave desenvuelta inválido (0x000000F0)"
            Case &HF1
                ' CKR_UNWRAPPING_KEY_SIZE_RANGE 0x000000F1
                response = "Rango de tamaño de llave desenvuelta inválido (0x000000F1)"
            Case &HF2
                ' CKR_UNWRAPPING_KEY_TYPE_INCONSISTENT 0x000000F2
                response = "Tipo de llave desenvuelta inconsistente (0x000000F2)"
            Case &H100
                ' CKR_USER_ALREADY_LOGGED_IN 0x00000100
                response = "Usuario ya logeado (0x00000100)"
            Case &H101
                ' CKR_USER_NOT_LOGGED_IN 0x00000101
                response = "Usuario no logeado (0x00000101)"
            Case &H102
                ' CKR_USER_PIN_NOT_INITIALIZED 0x00000102
                response = "PIN de usuario no inicializado (0x00000102)"
            Case &H103
                ' CKR_USER_TYPE_INVALID 0x00000103
                response = "Tipo de usuario inválido (0x00000103)"
            Case &H104
                ' CKR_USER_ANOTHER_ALREADY_LOGGED_IN 0x00000104
                response = "Usuario ya está logeado (0x00000104)"
            Case &H105
                ' CKR_USER_TOO_MANY_TYPES 0x00000105
                response = "Muchos tipos de usuario (0x00000105)"
            Case &H110
                ' CKR_WRAPPED_KEY_INVALID 0x00000110
                response = "Llave envuelta inválida (0x00000110)"
            Case &H112
                ' CKR_WRAPPED_KEY_LEN_RANGE 0x00000112
                response = "Rango de longitud de llave envuelta inválido (0x00000112)"
            Case &H113
                ' CKR_WRAPPING_KEY_HANDLE_INVALID 0x00000113
                response = "Manejador de llave envuelta inválido (0x00000113)"
            Case &H114
                ' CKR_WRAPPING_KEY_SIZE_RANGE 0x00000114
                response = "Rango de tamaño de llave envuelta inválido (0x00000114)"
            Case &H115
                ' CKR_WRAPPING_KEY_TYPE_INCONSISTENT 0x00000115
                response = "Tipo de llave envuelta inconsistente (0x00000115)"
            Case &H120
                ' CKR_RANDOM_SEED_NOT_SUPPORTED 0x00000120
                response = "Semilla aleatorea no soportada (0x00000120)"
            Case &H121
                ' CKR_RANDOM_NO_RNG 0x00000121
                response = "Error en generación de valor aleatoreo (0x00000121)"
            Case &H150
                ' CKR_BUFFER_TOO_SMALL 0x00000150
                response = "Buffer muy pequeño (0x00000150)"
            Case &H160
                ' CKR_SAVED_STATE_INVALID 0x00000160
                response = "Estado guardado inválido (0x00000160)"
            Case &H170
                ' CKR_INFORMATION_SENSITIVE 0x00000170
                response = "Información sensible (0x00000170)"
            Case &H180
                ' CKR_STATE_UNSAVEABLE 0x00000180
                response = "No se puede guardar estado (0x00000180)"
            Case &H190
                ' CKR_CRYPTOKI_NOT_INITIALIZED 0x00000190
                response = "Criptoku no inicializado (0x00000190)"
            Case &H191
                ' CKR_CRYPTOKI_ALREADY_INITIALIZED 0x00000191
                response = "Criptoki ya inicializado (0x00000191)"
            Case &H1A0
                ' CKR_MUTEX_BAD 0x000001A0
                response = "Mutex incorrecto (0x000001A0)"
            Case &H1A1
                ' CKR_MUTEX_NOT_LOCKED 0x000001A1
                response = "Mutex no bloqueado (0x000001A1)"
            Case &H80000000
                ' CKR_VENDOR_DEFINED 0x80000000
                response = "Error definido por fabricante (0x80000000)"
            Case &H8FFFFFFE
                ' CKR_TOKEN_NOT_MATCHES 0x8FFFFFFE
                response = "Tokens no coinciden (0x8FFFFFFE)"
            Case &H8FFFFFFF
                ' CKR_KEY_ALREADY_EXISTS 0x8FFFFFFF
                response = "Llave ya existe (0x8FFFFFFF)"
            Case Else
                response = String.Format("PKCS#11 Error: 0x{0:X8}", res)
        End Select
        Return response
    End Function
End Class
