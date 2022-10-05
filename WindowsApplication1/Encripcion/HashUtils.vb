Imports System.Text
Imports System.Security.Cryptography

Public Class HashUtils
    Public Function Bytes2HexString(ByVal bytes_Input As Byte()) As String
        Dim strTemp As New StringBuilder(bytes_Input.Length * 2)
        Dim hexTemp As String
        For Each b As Byte In bytes_Input
            hexTemp = Conversion.Hex(b)
            If hexTemp.Length < 2 Then
                strTemp.Append("0")
            End If
            strTemp.Append(hexTemp)
        Next
        Return strTemp.ToString()
    End Function

    Public Function GetSha512(data As String)
        Dim data_b() As Byte
        Dim result_b() As Byte
        data_b = System.Text.Encoding.ASCII.GetBytes(data)

        Dim shaM As New SHA512Managed()
        result_b = shaM.ComputeHash(data_b)

        Return Bytes2HexString(result_b)
    End Function

    Public Function XorHexString(A As String, B As String) As String
        Dim b1, b2, b3 As Byte
        Dim result As String = ""
        Dim hexTemp As String = ""

        For I As Integer = 0 To A.Length - 1 Step 2
            b1 = Convert.ToByte(Convert.ToInt32(A.Substring(I, 2), 16))
            b2 = Convert.ToByte(Convert.ToInt32(B.Substring(I, 2), 16))
            b3 = b1 Xor b2
            hexTemp = Conversion.Hex(b3)
            If hexTemp.Length <> 2 Then
                hexTemp = "0" & hexTemp
            End If
            result = result & hexTemp
        Next
        Return result
    End Function
End Class
