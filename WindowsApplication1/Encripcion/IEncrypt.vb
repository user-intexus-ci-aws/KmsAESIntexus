''' <summary>
''' Interface encargada de generar la estrucutra para implementación de los diferentes metodos de cifrado de datos
''' </summary>
''' <remarks>AERM 09/09/2015</remarks>
Public Interface IEncrypt

    ''' <summary>
    ''' Metodo encargado de des-inscripción  la cadena 
    ''' </summary>
    ''' <param name="cadenaCifrada">Cadena cifrada</param>
    ''' <returns>Retorna strig con la cadena sin cifrar</returns>
    ''' <remarks>AERM 09/09/2015</remarks>
    Function DescifrarDato(cadenaCifrada As String, key As String, iv As String) As String

    ''' <summary>
    ''' Metodo encargado de cifrar una cadena
    ''' </summary>
    ''' <param name="cadena">Cadena que se debe cifrar</param>
    ''' <returns>Retorna el cifrado de la cadena</returns>
    ''' <remarks>AERM 09/09/2015</remarks>
    Function CifrarDato(cadena As String, key As String, iv As String) As String


    ''' <summary>
    ''' Metodo encargado de retornar los dgitos de chequeo para validar contraseñas
    ''' </summary>
    ''' <param name="tamano">Tamaño de los digitos de chequeo requeridos</param>
    ''' <returns>AERM 15/09/2015</returns>
    Function DigitoChequeo(ByVal tamano As Integer, key As String) As String

End Interface