Imports System.Security.Cryptography
Imports System.Configuration
'Imports Intecusus.OperarBD

Public Class TDES
    Implements IEncrypt

#Region "Variables Publicas"

    Dim output() As Byte
    Dim claseBase As DES = New DES()
    Dim des3Prov As New TripleDESCryptoServiceProvider()
    'Private operacionesBD As OperacionesBase = New OperacionesBase()
#End Region

#Region "Implementacion Intrerface"

    Public Function CifrarDato(cadena As String, key As String, iv As String) As String Implements IEncrypt.CifrarDato
        Try
            cadena = Me.ValidarDato(cadena, key)
            Dim m_Key16() As Byte = HexStringToBytes(key)
            Dim m_Vector() As Byte = HexStringToBytes(iv)
            Dim m_Cadena() As Byte = HexStringToBytes(cadena)
            Dim m_Resultado As String
            ReDim output(m_Cadena.Length - 1)

            Me.claseBase.encrypt_3des(m_Key16, m_Cadena, m_Cadena.Length, output, output.Length)
            m_Resultado = GetHexString(output)

            Try
                Dim netDes As ICryptoTransform = des3Prov.CreateEncryptor(m_Key16, m_Vector)
                netDes.TransformBlock(m_Cadena, 0, m_Cadena.Length, output, 0)
                m_Resultado = GetHexString(output)
            Catch ex As Exception
                'operacionesBD.GuardarLogXNombreConectionString(20, 0, 2, "TDES.vb - CifrarDato : " + cadena + " " + ex.Message, ConfigurationManager.AppSettings("BD"))
            End Try

            Return m_Resultado
        Catch ex As Exception
            Throw New Exception("TDES- CifrarDato " + ex.Message)
        End Try
    End Function

    Public Function DescifrarDato(cadenaCifrada As String, key As String, iv As String) As String Implements IEncrypt.DescifrarDato
        Try
            Me.ValidarDato(cadenaCifrada, key)
            Dim m_Key16() As Byte = HexStringToBytes(key)
            Dim m_Vector() As Byte = HexStringToBytes(iv)
            Dim m_Cadena() As Byte = HexStringToBytes(cadenaCifrada)

            Dim m_Resultado As String
            ReDim output(m_Cadena.Length - 1)
            Me.claseBase.decrypt_3des(m_Key16, m_Cadena, m_Cadena.Length, output, output.Length)
            m_Resultado = GetHexString(output)
            Try
                Dim netDes As ICryptoTransform = des3Prov.CreateDecryptor(m_Key16, m_Vector)
                netDes.TransformBlock(m_Cadena, 0, m_Cadena.Length, output, 0)
                m_Resultado = GetHexString(output)
            Catch ex As Exception
                ''Se debe guardar error del vector de inicio (Clave de seguridad muy debil)
                ' operacionesBD.GuardarLogXNombreConectionString(20, 0, 2, "TDES.vb - DescifrarDato : " + cadenaCifrada + " " + ex.Message, ConfigurationManager.AppSettings("BD"))
            End Try

            Return m_Resultado
        Catch ex As Exception
            Throw New Exception("TDES- DescifrarDato " + ex.Message)
        End Try
    End Function

    Public Function DigitoChequeo(tamano As Integer, key As String) As String Implements IEncrypt.DigitoChequeo
        Try
            'Proceso de cifrado
            Dim m_Resultado As String = Me.CifrarDato("0000000000000000000000000000000", key, "0000000000000000")

            'Proceso de descifrado
            Dim mResultadoDesc As String = Me.DescifrarDato(m_Resultado, key, "0000000000000000")

            Return m_Resultado.Substring(0, tamano)
        Catch ex As Exception
            Throw New Exception("TDES- DigitoChequeo " + ex.Message)
        End Try
    End Function
#End Region

#Region "Metodos privados"

    ''' <summary>
    ''' Metodo encargado de convertir de Hexa a bytes
    ''' </summary>
    ''' <param name="hexstring">Cadena en string</param>
    ''' <returns>Retorna los bytes de la llave</returns>
    ''' <remarks>AERM 15/09/2015</remarks>
    Public Function HexStringToBytes(ByVal hexstring As String) As Byte()
        Dim out((hexstring.Length / 2) - 1) As Byte
        For i = 0 To (hexstring.Length / 2) - 1
            out(i) = Convert.ToByte(hexstring.Substring(i * 2, 2), 16)
        Next
        Return out
    End Function

    ''' <summary>
    ''' Convierte de Hexa Bytes a String 
    ''' </summary>
    ''' <param name="bytes">Bytes a convertir</param>
    ''' <param name="len">Tamaño de los bytes</param>
    ''' <param name="spaces">Boolean indicando es con espacios</param>
    ''' <returns>Retorna hexadecimal en cadena strig</returns>
    ''' <remarks>AERM 15/09/2015</remarks>
    Private Function GetHexString(ByVal bytes() As Byte, Optional ByVal len As Integer = -1, Optional ByVal spaces As Boolean = False) As String
        If len = -1 Then len = bytes.Length
        Dim i As Integer
        Dim s As String = ""
        For i = 0 To len - 1
            s += bytes(i).ToString("x2")
            If spaces Then s += " "
        Next
        If spaces Then s = s.TrimEnd()
        Return s
    End Function


    ''' <summary>
    ''' Metodo encargado de validar las llaves
    ''' </summary>
    ''' <param name="cadenaCifrada">Cadena en hexadecimal</param>
    ''' <param name="key">Llave a validar</param>
    ''' <remarks>AERM 15/09/2015 </remarks>
    Private Function ValidarDato(cadenaCifrada As String, key As String) As String
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
    Private Function RellenarCadena(cadena As String, p2 As String) As String
        Try
            While (Me.ValidarTamanoDato(cadena, 8))
                cadena += "0"
            End While
        Catch ex As Exception
            Throw New Exception("Tamaño de la cadena no es multiplo de  8")
        End Try

        Return cadena
    End Function

    Private Function ValidarTamanoDato(cadena As String, valorValidar As Integer) As Boolean
        If (cadena.Length / 2) Mod valorValidar <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

End Class