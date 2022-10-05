Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Environment
Imports System.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Public Class FachadaKMS

    Private Shared LockThis As New Object

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function SetDllDirectory(lpPathName As String) As Boolean
    End Function


    <DllImport("kernel32", SetLastError:=True)>
    Private Shared Function LoadLibrary(lpFileName As String) As IntPtr
    End Function

    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="KCV_ZMK", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function KCV_ZMK(ByVal tokenname As String, ByVal passphrase As String, ByVal zmklabel As String, ByVal KCV As StringBuilder) As Integer
    End Function


    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="GENERATE_KEY", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function GENERATE_KEY(ByVal C1 As StringBuilder, ByVal C2 As StringBuilder, ByVal C3 As StringBuilder, ByVal tokenname As String, ByVal passphrase As String) As Integer
    End Function


    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="LOAD_ZMK", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function LOAD_ZMK(ByVal C1 As String, ByVal C2 As String, ByVal C3 As String, ByVal tokenname As String, ByVal passphrase As String, ByVal zmklabel As String) As Integer
    End Function

    <DllImport("NShieldCryptoki.dll", _
       BestFitMapping:=True, _
       PreserveSig:=True, _
       EntryPoint:="INICIAR_PKCS11", SetLastError:=False, _
       CharSet:=CharSet.Ansi, ExactSpelling:=False, _
       CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function INICIAR_PKCS11() As Integer
    End Function

    <DllImport("NShieldCryptoki.dll", _
        BestFitMapping:=True, _
        PreserveSig:=True, _
        EntryPoint:="INICIAR_RECURSOS", SetLastError:=False, _
        CharSet:=CharSet.Ansi, ExactSpelling:=False, _
        CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function INICIAR_RECURSOS(ByVal token_name As StringBuilder, ByVal passphrase As StringBuilder, ByVal key_label As StringBuilder, ByVal out_hSession As StringBuilder, ByVal out_hKey As StringBuilder) As Integer
    End Function

    <DllImport("NShieldCryptoki.dll", _
            BestFitMapping:=True, _
            PreserveSig:=True, _
            EntryPoint:="FINALIZAR_RECURSOS", SetLastError:=False, _
            CharSet:=CharSet.Ansi, ExactSpelling:=False, _
            CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function FINALIZAR_RECURSOS(ByVal in_hSession As StringBuilder) As Integer
    End Function


    <DllImport("NShieldCryptoki.dll", _
       BestFitMapping:=True, _
       PreserveSig:=True, _
       EntryPoint:="TDESECB", SetLastError:=False, _
       CharSet:=CharSet.Ansi, ExactSpelling:=False, _
       CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function TDESECB(ByVal in_hSession As StringBuilder, ByVal in_hKey As StringBuilder, ByVal data_in As StringBuilder, ByVal data_out As StringBuilder, ByVal mode As StringBuilder) As Integer
    End Function

    <DllImport("NShieldCryptoki.dll", _
       BestFitMapping:=True, _
       PreserveSig:=True, _
       EntryPoint:="TDES", SetLastError:=False, _
       CharSet:=CharSet.Ansi, ExactSpelling:=False, _
       CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function TDES(ByVal in_hSession As StringBuilder, ByVal in_hKey As StringBuilder, ByVal data_in As StringBuilder, ByVal data_out As StringBuilder, ByVal mode As StringBuilder, ByVal IV As StringBuilder) As Integer
    End Function


    Public Function LoadDll(rutaDll As String, NombreDll As String) As Boolean
        Try
            Dim dll2Path As String = rutaDll
            SetDllDirectory(dll2Path)
            Dim ptr2 As IntPtr = LoadLibrary(NombreDll)

            If ptr2 = IntPtr.Zero Then
                Console.WriteLine(Marshal.GetLastWin32Error())
                'Logger.Write("Error al cargar la dll:" + NombreDll + "Ruta dll : " + rutaDll + "Error generico:" + Marshal.GetLastWin32Error().ToString(), "ExceptionHandling")
                Return False
            Else
                'Logger.Write(NombreDll + " Cargada", "AppLog")

                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function kcvZmk(ByVal tokenname As String, ByVal passphrase As String, ByVal zmklabel As String, ByVal kcv As StringBuilder) As Integer
        Return KCV_ZMK(tokenname, passphrase, zmklabel, kcv)
    End Function



    Public Function loadZmk(ByVal C1 As String, ByVal C2 As String, ByVal C3 As String, ByVal tokenname As String, ByVal passphrase As String, ByVal key_name As String) As Integer
        Return LOAD_ZMK(C1, C2, C3, tokenname, passphrase, key_name)
    End Function


    Public Function generateKey(ByVal C1 As StringBuilder, ByVal C2 As StringBuilder, ByVal C3 As StringBuilder, ByVal tokenname As String, ByVal passphrase As String) As Integer
        Return GENERATE_KEY(C1, C2, C3, tokenname, passphrase)
    End Function

    Public Function IniciarPKCS11() As Integer
        Return INICIAR_PKCS11()
    End Function


    Public Function IniciarRecursos(ByVal token_name As StringBuilder, ByVal passphrase As StringBuilder, ByVal key_label As StringBuilder, ByVal out_hSession As StringBuilder, ByVal out_hKey As StringBuilder) As Integer
        Return INICIAR_RECURSOS(token_name, passphrase, key_label, out_hSession, out_hKey)
    End Function

    Private Shared Function FinalizalizarRecursos(ByVal in_hSession As StringBuilder) As Integer
        Return FINALIZAR_RECURSOS(in_hSession)
    End Function


    Public Function CifrarDato(ByVal hSession As StringBuilder, ByVal hKey As StringBuilder, ByVal cadena As String) As String
        SyncLock LockThis
            ' Variables locales
            cadena = Me.ValidarDato(cadena, "00000000000000000000000000000000")
            Dim mode As StringBuilder = New StringBuilder("E")
            Dim data_in As StringBuilder = New StringBuilder(cadena)
            Dim data_out As StringBuilder = New StringBuilder("", cadena.Length + 1)
            Dim rv As Integer = 0

            Dim CipherType As String = ConfigurationManager.AppSettings("CypherType")
            'Logger.Write("Tipo cifrado: " & CipherType, "Tracing")
            If Not String.IsNullOrEmpty(CipherType) Then

                Select Case CipherType
                    Case "ECB"
                        rv = TDESECB(hSession, hKey, data_in, data_out, mode)
                    Case "CBC"
                        Dim strVi As String = ConfigurationManager.AppSettings("VI")
                        If Not String.IsNullOrEmpty(strVi) Then
                            Dim IV As StringBuilder = New StringBuilder(strVi)
                            rv = TDES(hSession, hKey, data_in, data_out, mode, IV)
                        Else
                            Dim IV As StringBuilder = New StringBuilder("0000000000000000")
                            rv = TDES(hSession, hKey, data_in, data_out, mode, IV)
                        End If
                    Case Else
                        rv = TDESECB(hSession, hKey, data_in, data_out, mode)
                End Select
            Else
                rv = TDESECB(hSession, hKey, data_in, data_out, mode)
            End If

            If rv <> 0 Then
                Throw New Exception("HSMShield: funcion: CifrarDato, err: " & rv.ToString())
            End If
            Return data_out.ToString()
        End SyncLock
    End Function

    Public Function DescifrarDato(ByVal hSession As StringBuilder, ByVal hKey As StringBuilder, ByVal cadenaCifrada As String) As String
        SyncLock LockThis
            ' Variables locales
            If cadenaCifrada.Length / 2 Mod 8 <> 0 Then
                Throw New Exception("HSMShield - DescifrarDato: Tamaño de la cadena no es multiplo de  8")
            End If

            Dim mode As StringBuilder = New StringBuilder("D")
            Dim data_in As StringBuilder = New StringBuilder(cadenaCifrada)
            Dim data_out As StringBuilder = New StringBuilder("", cadenaCifrada.Length + 1)
            Dim rv As Integer = 0

            Dim CipherType As String = ConfigurationManager.AppSettings("CypherType")

            'Logger.Write("Tipo cifrado: " & CipherType, "Tracing")


            If Not String.IsNullOrEmpty(CipherType) Then

                Select Case CipherType
                    Case "ECB"
                        rv = TDESECB(hSession, hKey, data_in, data_out, mode)
                    Case "CBC"
                        Dim strVi As String = ConfigurationManager.AppSettings("VI")
                        If Not String.IsNullOrEmpty(strVi) Then
                            Dim IV As StringBuilder = New StringBuilder(strVi)
                            'Logger.Write("iv: " & IV.ToString(), "Tracing")
                            rv = TDES(hSession, hKey, data_in, data_out, mode, IV)
                        Else
                            Dim IV As StringBuilder = New StringBuilder("0000000000000000")
                            'Logger.Write("iv: " & IV.ToString(), "Tracing")
                            rv = TDES(hSession, hKey, data_in, data_out, mode, IV)
                        End If
                    Case Else
                        rv = TDESECB(hSession, hKey, data_in, data_out, mode)
                End Select
            Else
                rv = TDESECB(hSession, hKey, data_in, data_out, mode)
            End If

            ' Se llama a la función de la DLL
            'Dim IV As StringBuilder = New StringBuilder(ConfigurationManager.AppSettings("VI"))
            'rv = TDES(hSession, hKey, data_in, data_out, mode, IV)

            If rv <> 0 Then
                'Logger.Write("Excepcion descifrado error: " & rv.ToString(), "AppLog")
                Throw New Exception("HSMShield: funcion: DescifrarDato, err: " & rv.ToString())

            End If
            Return data_out.ToString()
        End SyncLock
    End Function


    Public Function DigitoChequeo(ByVal hSession As StringBuilder, ByVal hKey As StringBuilder, ByVal tamano As Integer) As String
        SyncLock LockThis
            ' Variables locales
            Dim IV As StringBuilder = New StringBuilder(ConfigurationManager.AppSettings("VI"))
            Dim mode As StringBuilder = New StringBuilder("E")
            Dim data_in As StringBuilder = New StringBuilder("00000000000000000000000000000000")
            Dim data_out As StringBuilder = New StringBuilder("", 32 + 1)
            Dim rv As Integer = 0

            ' Se llama a la función de la DLL
            rv = TDES(hSession, hKey, data_in, data_out, mode, IV)

            If rv <> 0 Then
                Throw New Exception("HSMShield: funcion: DigitoChequeo, err: " & rv.ToString())
            End If

            Return data_out.ToString().Substring(0, tamano)
        End SyncLock
    End Function

    ''' <summary>
    ''' Metodo encargado de validar las llaves
    ''' </summary>
    ''' <param name="cadenaCifrada">Cadena en hexadecimal</param>
    ''' <param name="key">Llave a validar</param>
    ''' <remarks>AERM 15/09/2015 </remarks>
    Private Function ValidarDato(ByVal cadenaCifrada As String, ByVal key As String) As String
        If (cadenaCifrada.Length / 2) Mod 8 <> 0 Then cadenaCifrada = Me.RellenarCadena(cadenaCifrada, key)
        If key.Length <> (16 * 2) Then Throw New Exception("El tamaño  del DES3 key no es de 16")

        Return cadenaCifrada
    End Function

    ''' <summary>
    ''' Metodo encargado de rellenar con 0 los valores faltantes del octeto
    ''' </summary>
    ''' <param name="cadena">Cadena a rellenar</param>
    ''' <param name="p2">llave </param>
    ''' <returns>Retorna la cadena con los valores resultantes</returns>
    ''' <remarks>AERM 15/09/2015</remarks>
    Private Function RellenarCadena(ByVal cadena As String, ByVal p2 As String) As String
        Try
            While (Me.ValidarTamanoDato(cadena, 8))
                cadena += "0"
            End While
        Catch ex As Exception
            Throw New Exception("Tamaño de la cadena no es multiplo de  8")
        End Try

        Return cadena
    End Function

    ''' <summary>
    ''' valida que el tamaño del dato sea múltiplo de 8
    ''' </summary>
    ''' <param name="cadena"></param>
    ''' <param name="valorValidar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidarTamanoDato(ByVal cadena As String, ByVal valorValidar As Integer) As Boolean
        If (cadena.Length / 2) Mod valorValidar <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Sub Finalizar(hSession As StringBuilder)
        SyncLock LockThis
            FINALIZAR_RECURSOS(hSession)
        End SyncLock
    End Sub


End Class
