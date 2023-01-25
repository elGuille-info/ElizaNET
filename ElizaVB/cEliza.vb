'------------------------------------------------------------------------------
' cEliza                                                      (22/ene/23 10.08)
' Versi�n para .net 7.0
'
' �Guillermo Som (Guille), 1998-2002, 2023
'------------------------------------------------------------------------------

#Region " Notas de la versi�n de Visual Basic 5/6 "

' Basado en la versi�n para Visual Basic 5.0 y 6.0 con sp5 de 2002
'
' cEliza.cls                                                        (30/May/98)
'
' Clase para mantener colecciones del tipo cRegla
' y procesar la entrada del usuario y devolver una respuesta,
' es decir el "coco" de ELIZA...
'
' �Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
'
' Revisiones, normalmente en las comprobaciones de las palabras.
' 00.05.00  Las terminaciones rte,rme s�lo en los verbos conocidos
' 00.06.00  Se mantiene un array con las palabras de las frases
' 00.06.03  He quitado la opci�n del idioma.
' 00.07.04  Se cambia melo -> telo en los verbos conocidos
' 00.07.06  Si se especifica @ despu�s del nivel,
'           la primera respuesta ser� aleatoria
' 00.07.11  A�ado conjugaciones y corrijo las que estaban mal
' 00.08.00  Cambio la forma de analizar la frase.
'           Las sustituciones s�lo se har�n al usar *RESTO*
'           aunque esto implica el cambio de las palabras claves
' 00.09.00  Las palabras especificadas en {* se separar�n por ;
'           de esta forma se puede usar m�s de una palabra.
'           He cambiado la forma de usar SiguientePalabra.
'-----------
' 00.09.xx  En el entorno del VB me da un GPF al terminar de forma
'           normal, esto no ocurre con el programa compilado y
'           parece ser que s�lo ocurre en el 200 MMX (el de mi casa)
'           Las modificaciones hechas en las declaraciones de los
'           objetos en las clases ha sido para ver si se evitaba...
'-----------
' 00.09.07  Se permite comprobar una clave si el usuario escribe *consulta*
' 00.10.00  Eliza puede recordar algo de lo que dijo el usuario
'           Inicialmente se guardar� cuando el user diga:
'           "mi xxx" o "mis xxx" y se usar� cuando no entienda
' 00.11.00  Se puede usar {*iif para tener en cuenta la respuesta
'           del usuario, usando lo indicado en cada una de las
'           partes, seg�n se cumpla o no.
'           {*iff(*true*|*false*;PARTE-VERDADERA)(PARTE-FALSA)}
' 00.12.00  Empiezo a usar la base de datos del usuario.
'           Para almacenar datos se har� de esta forma:
'           Pregunta{*base:=clave_BaseDatos}
' 00.12.10  Cambio la forma de guardar las subclaves cuando se usan
'           {* xxx; yyy} de forma que el *equal:= haga referencia
'           directa a la clave superior.
'           ===LO QUITO===
' 00.13.00  A�ado un form para consulta de claves y respuestas
'
' 00.14.00  Con la opci�n de "decir" (o hablar) lo que escribe...
' 00.15.00  A�ado opci�n de 'hablar' con el Direct Speech Synthesis Control (DirectSS)
' 00.16.00  Quito el DirectSS
'           Cambio la forma de entrada/salida por "simples" TextBoxes
' 00.17.00  Se permiten varios ficheros de palabras, el nombre ser� ElizaVB_xxx.txt
'           El primero que se leer� ser� Eliza_SPA.txt, de esta forma, se pueden
'           aplicar nuevas reglas que sustituyan a las definidas en Eliza_SPA.txt
'
' POR HACER: (16/Sep/02)
'           Si una palabra va acentuada, se puede indicar de esta forma:
'           "que'" para indicar que se usar�n las dos formas: qu� y que
'           Permitir en las palabras claves el uso de *oa* para crear las
'           dos formas: masculina y femenina.
'------------------------------------------------------------------------------
#End Region

Option Strict On
Option Infer On

Option Explicit On
Option Compare Text

Public Class cEliza

    Private m_AppPath As String

    Public Sub New(appPath As String)
        'm_Idioma = Spanish
        m_Sexo = eSexo.Masculino
        'm_Iniciado = False
        Iniciado = False
        m_Releer = False
        m_AppPath = appPath
    End Sub

    ' �Esto se debe usar con signos de admiraci�n, etc.?
    'Public SiNo As String() = {"si", "s�", "ok", "vale", "yep", "yes", "no", "non", "nope", "nop", "nil", "nain"}
    Private m_rnd As New Random()

    Public Enum eTiposDeClaves
        eClaves '= 0
        eExtras '= 1
        eExtras2 '= 2
        eVerbos '= 3
        eRS '= 4
        eSimp '= 5
        eRec '= 6
        eBU '= 7
    End Enum

    ' Para la base de datos del usuario                                 (14/Jun/98)
    Private BaseUser As New cRespuestas
    Private sUsarBaseDatos As String

    ' Para interactuar con el usuario, se usar� un array cuando se le
    ' haga una pregunta a la que deber� responder con dos tipos de
    ' respuesta, normalmente una positiva y otra negativa
    Const cAfirmativa As Integer = 1
    Const cNegativa As Integer = 0
    ' de cero a uno
    Private sRespuestas(cAfirmativa) As String
    ' Esta variable servir� de 'flag' y contendr� la condici�n que
    ' usar� Eliza para evaluar una "respuesta" que contenga "{*iif("
    Private sUsarPregunta As String

    'Private m_Iniciado As Boolean
    Private m_Releer As Boolean

    ' Para la revisi�n 00.06.00
    ' Estos arrays tendr�n cada palabra de la frase original
    Private FraseOrig() As String
    ' y la convertida
    'Private FraseConv() As String
    ' el n�mero de la palabra actual
    Private PalabraOrig As Integer
    'Private PalabraConv As Integer
    ' aunque el n�mero de palabras se puede conseguir con UBound(array)
    ' es conveniente mantener una variable, entre otras cosas para saber
    ' cu�l es la que se a�ade
    Private PalabrasOrig As Integer
    'Private PalabrasConv As Integer

    Public Nombre As String

    ' Para indicar si es del sexo femenino o masculino
    ' (por defecto es masculino)
    Public Enum eSexo
        Masculino   '= 0
        Femenino    '= 1
        Ninguno     '= 2
    End Enum
    Private m_Sexo As eSexo

    'Public Enum eIdioma
    '    Spanish '= 0
    '    English '= 1
    '    Ninguno '= 2
    'End Enum
    'Private m_Idioma As eIdioma
    'Public Property Get Idioma() As eIdioma
    '    Idioma = m_Idioma
    'End Property
    'Public Property Let Idioma(ByVal NewIdioma As eIdioma)
    '    's�lo se deben admitir los valores indicados
    '    'de no ser as� se asignar� Spanish
    '    Select Case NewIdioma
    '    Case Spanish, English
    '        'nada
    '    Case Else
    '        NewIdioma = Spanish
    '    End Select
    '    m_Idioma = NewIdioma
    '
    'End Property

    ' Evento para indicar que el usuario se ha despedido
    Public Event Terminado()

    ' Colecci�n para recordar lo que ha dicho el usuario                (12/Jun/98)
    Private ReadOnly m_colRec As New cRespuestas
    ' colecci�n para los verbos
    Private ReadOnly m_Verbos As New cRespuestas
    ' Colecci�n para las palabras de las reglas de simplificaci�n
    ' Estas ser�n las que se usar�n en SustituirEnEntrada
    Private ReadOnly m_colRS As New cRespuestas
    ' Estas ser�n las que se usar�n en SimplificarEntrada
    Private ReadOnly m_colSimp As New cRespuestas

    Private ReadOnly sSeparadores As String = " .,;:�?�!()[]/-" & ChrW(9) & ChrW(34) & vbCr & vbLf

    ''' <summary>
    ''' Las colecci�n con las reglas
    ''' </summary>
    Public ReadOnly Property ColReglas As Dictionary(Of String, cRegla)
        Get
            Return m_colReglas
        End Get
    End Property
    Public ReadOnly Property ColVerbos As cRespuestas
        Get
            Return m_Verbos
        End Get
    End Property
    Public ReadOnly Property ColRec As cRespuestas
        Get
            Return m_colRec
        End Get
    End Property
    Public ReadOnly Property ColRS As cRespuestas
        Get
            Return m_colRS
        End Get
    End Property
    Public ReadOnly Property ColSimp As cRespuestas
        Get
            Return m_colSimp
        End Get
    End Property
    Public ReadOnly Property ColBaseUser As cRespuestas
        Get
            Return BaseUser
        End Get
    End Property

    ' Colecci�n de Reglas
    Private ReadOnly m_colReglas As New Dictionary(Of String, cRegla)

    ''' <summary>
    ''' Acceder a una regla en concreto. Si no existe, se a�ade.
    ''' </summary>
    ''' <param name="newRegla">La regla que queremos</param>
    Public Function Reglas(newRegla As String) As cRegla
        Dim tRegla As cRegla

        If m_colReglas.ContainsKey(newRegla) Then
            tRegla = m_colReglas.Item(newRegla)
        Else
            ' Si no existe a�adirlo
            tRegla = New cRegla(newRegla)
            m_colReglas.Add(newRegla, tRegla)
        End If
        Return tRegla
    End Function

    ''' <summary>
    ''' Devuelve los n �ltimos caracteres de la cadena indicada.
    ''' </summary>
    ''' <param name="texto"></param>
    ''' <param name="n"></param>
    ''' <remarks>Si n es mayor que la longitud o es menor de uno, se devuelve la cadena vac�a.</remarks>
    Public Shared Function RightN(texto As String, n As Integer) As String
        If String.IsNullOrEmpty(texto) Then Return texto
        Dim len = texto.Length
        If n > len OrElse n < 1 Then Return "" 'texto

        Return texto.Substring(len - n, n)
    End Function
    ''' <summary>
    ''' Devuelve los n primeros caracteres de la cadena indicada.
    ''' </summary>
    ''' <param name="texto"></param>
    ''' <param name="n"></param>
    ''' <remarks>Si n es mayor que la longitud o es menor de uno, se devuelve la cadena vac�a.</remarks>
    Public Shared Function LeftN(texto As String, n As Integer) As String
        If String.IsNullOrEmpty(texto) Then Return texto
        Dim len = texto.Length
        If n > len OrElse n < 1 Then Return "" 'texto

        Return texto.Substring(0, n)
    End Function

    ''' <summary>
    ''' Devuelve a partir del car�cter n de la cadena indicada.
    ''' </summary>
    ''' <param name="texto"></param>
    ''' <param name="n"></param>
    ''' <remarks>Si n es mayor que la longitud o es menor de uno, se devuelve la cadena vac�a.</remarks>
    Public Shared Function MidN(texto As String, n As Integer) As String
        If String.IsNullOrEmpty(texto) Then Return texto
        Dim len = texto.Length
        If n > len OrElse n < 1 Then Return "" 'texto

        Return texto.Substring(n)
    End Function

    ''' <summary>
    ''' Devuelve t caracteres a partir del car�cter n de la cadena indicada.
    ''' </summary>
    ''' <param name="texto"></param>
    ''' <param name="n"></param>
    ''' <param name="t"></param>
    ''' <remarks>Si n es mayor que la longitud o es menor de uno, se devuelve la cadena vac�a.</remarks>
    Public Shared Function MidN(texto As String, n As Integer, t As Integer) As String
        If String.IsNullOrEmpty(texto) Then Return texto
        Dim len = texto.Length
        If n > len OrElse n < 1 OrElse t > len OrElse t < 1 Then Return "" 'texto
        If n + t > len Then Return ""

        Return texto.Substring(n, t)
    End Function

    ' Esto es lo que en realidad hay que revisar porque no lo analiza... (24/ene/23 13.37)
    Public Function ProcesarEntrada(sEntrada As String) As String
        ' Se procesar� la entrada del usuario y devolver� la cadena con la respuesta
        ' A este procedimiento se llamar� desde el formulario,
        ' despu�s de que el usuario escriba.

        Dim sEntradaSimp As String
        Dim sClaves As String = ""
        Dim sCopiaEntrada As String
        Static ModoConsulta As Boolean

        Dim i As Integer
        Dim sPalabra As String = ""

        ' Convertirla en min�scula
        ' (no es necesario ya que se usa Option Compare Text)
        ' Eso funcionaba bien en VB, pero en .NET es un l�o,    (24/ene/23 11.02)
        ' Tengo que usar StringComparison.OrdinalIgnoreCase para hacer las comprobaciones.
        'sEntrada = LCase(sEntrada)

        ' Si se escribe *consulta* usar lo que viene a continuaci�n
        ' para buscar en las claves y subclaves

        ' Quitar los dobles espacios que haya
        sEntrada = QuitarEspaciosExtras(sEntrada)

        If String.IsNullOrEmpty(sEntrada) OrElse sEntrada = ">" Then
            Return "Por favor escribe algo, gracias."
        End If

        If sEntrada.StartsWith("*consulta*", StringComparison.OrdinalIgnoreCase) Then
            ModoConsulta = Not ModoConsulta
            If ModoConsulta Then
                sEntradaSimp = "Escribe la clave o sub-clave a comprobar, para terminar de consultar, escribe nuevamente *consulta*"
            Else
                sEntradaSimp = "Salimos del modo consulta, ya puedes continuar normalmente."
            End If
            Return sEntradaSimp
        End If
        If ModoConsulta Then
            ' buscar la clave y mostrar si existe o no
            ' si existe, mostrar la siguiente respuesta
            sClaves = BuscarEsaClave(sEntrada)
            If String.IsNullOrEmpty(sClaves) = False Then
                Return "Respuesta: " & sClaves
            End If
            Return "No existe '" & sEntrada & "' en las claves y sub-claves"
        End If

        ' Quitar los signos de separaci�n del principio
        'Do While sSeparadores.IndexOf(sEntrada.Substring(0, 1), StringComparison.OrdinalIgnoreCase) > -1
        Do While sSeparadores.IndexOf(sEntrada.Substring(0, 1)) > -1
            sEntrada = sEntrada.Substring(1)
            ' Por si se han escito solo separadores             (24/ene/23 17.42)
            If String.IsNullOrEmpty(sEntrada) Then
                Return "Por favor escribe algo (aparte de signos o espacios), gracias."
            End If
        Loop
        ' Quitar tambi�n los del final                                  (16/Sep/02)
        'Do While sSeparadores.IndexOf(RightN(sEntrada, 1), StringComparison.OrdinalIgnoreCase) > -1
        Do While sSeparadores.IndexOf(RightN(sEntrada, 1)) > -1
            'sEntrada = Left$(sEntrada, Len(sEntrada) - 1)
            sEntrada = sEntrada.Substring(0, sEntrada.Length - 1)
        Loop

        ' Si hay que usar una respuesta, procesar aqu� para el caso de
        ' que esa respuesta sea s� o no...
        If String.IsNullOrEmpty(sUsarPregunta) = False Then
            ' Se puede usar *afirmativo*, *true*
            If sUsarPregunta.IndexOf("*afirmativo*", StringComparison.OrdinalIgnoreCase) > -1 OrElse
                sUsarPregunta.IndexOf("*true*", StringComparison.OrdinalIgnoreCase) > -1 Then
                ' Comprobar si el contenido de sEntrada es una respuesta
                ' afirmativa
                If EsAfirmativo(sEntrada) Then
                    sUsarPregunta = sRespuestas(cAfirmativa)
                ElseIf EsNegativo(sEntrada) Then
                    sUsarPregunta = sRespuestas(cNegativa)
                Else
                    sUsarPregunta = sEntrada
                End If
            Else
                ' procesar la comparaci�n
                ' para probar se asume que es negativa
                If EsNegativo(sEntrada) Then
                    sUsarPregunta = sRespuestas(cNegativa)
                ElseIf EsAfirmativo(sEntrada) Then
                    sUsarPregunta = sRespuestas(cAfirmativa)
                Else
                    sUsarPregunta = sEntrada
                End If
            End If
            ' Comprobar si sUsarPregunta contiene *equal:=
            ' de ser as�, buscar la respuesta adecuada,
            ' en otro caso esa es la respuesta
            i = sUsarPregunta.IndexOf("*equal:=", StringComparison.OrdinalIgnoreCase)
            Do While i > -1 'i = 1
                'sPalabra = LTrim$(Mid$(sUsarPregunta, i + 8))
                sPalabra = sUsarPregunta.Substring(i + 8).TrimStart()
                Dim tRegla As cRegla

                If m_colReglas.ContainsKey(sPalabra) Then
                    tRegla = m_colReglas(sPalabra)
                Else
                    tRegla = Nothing
                End If
                If tRegla Is Nothing Then
                    sUsarPregunta = BuscarReglas(sPalabra, tRegla)
                Else
                    ' No existe como clave principal,
                    ' hay que buscarlo en las sub-claves
                    For Each tRegla In m_colReglas.Values
                        sUsarPregunta = BuscarReglas(sPalabra, tRegla)
                        If String.IsNullOrEmpty(sUsarPregunta) = False Then
                            Exit For
                        End If
                    Next
                End If
                i = sUsarPregunta.IndexOf("*equal:=", StringComparison.OrdinalIgnoreCase)
                ' repetir mientras en sRespuesta hay un *equal:=
            Loop
            i = sUsarPregunta.IndexOf("{*iif", StringComparison.OrdinalIgnoreCase)
            ' Si el usuario no ha contestado con lo esperado
            If sUsarPregunta <> sEntrada Then
                Dim res = ComprobarEspeciales(sUsarPregunta, sEntrada, sPalabra)
                If i = -1 Then
                    sUsarPregunta = ""
                End If
                Return res
            End If
        End If

        'If Len(sUsarBaseDatos) Then
        If String.IsNullOrEmpty(sUsarBaseDatos) = False Then
            ' antes de almacenar los datos, se deber�a chequear
            ' para que el usuario no nos de 'datos err�neos'
            ' Por ejemplo si se le pregunta el signo del zodiaco
            ' que no nos diga cualquier cosa...
            sCopiaEntrada = sUsarBaseDatos
            sUsarBaseDatos = ValidarDatosParaBase(sEntrada)
            ' Si el valor es diferente es que se ha modificado,
            ' para indicarle algo al usuario
            If sUsarBaseDatos <> sEntrada Then
                Return sUsarBaseDatos
            End If
            sUsarBaseDatos = ""
            ' Aqu� deber�a salir, para que no diga una chorrada
            ' cuando el usuario contesta

            'If CInt(Rnd() * 5) > 2 Then
            If m_rnd.Next(6) > 2 Then
                sCopiaEntrada = "Gracias por indicarme tu " & sCopiaEntrada
            Else
                sCopiaEntrada = "Gracias por decirme que tu " & sCopiaEntrada & " es " & sEntrada
            End If
            Return sCopiaEntrada & "."
        End If

        ' quitar los "s�," y "no," del principio
        If sEntrada.StartsWith("s�,", StringComparison.OrdinalIgnoreCase) Then
            sEntrada = MidN(sEntrada, 4).TrimStart()
        End If
        If sEntrada.StartsWith("no,", StringComparison.OrdinalIgnoreCase) Then
            sEntrada = MidN(sEntrada, 4).TrimStart()
        End If

        'sCopiaEntrada = sEntrada

        ' Convertir la entrada en un array de palabras       (07/Jun/98)
        ' En este procedimiento es d�nde se asignar�n las palabras que
        ' debe recordar de lo que ha dicho el usuario        (12/Jun/98)
        Entrada2Array(sEntrada)

        ' Sustituir las palabras de simplificaci�n,
        ' ahora ya no se usan todas, ni los verbos, eso se har� s�lo
        ' cuando haya que usar parte de la entrada del usuario
        ' en la respuesta, es decir donde est� *RESTO*
        sEntradaSimp = SustituirEnEntrada(sEntrada)

        'Buscar las claves incluidas en la entrada y devolverlas
        'ordenadas seg�n el Nivel
        'sClaves = BuscarClaves(sEntradaSimp)
        '---Ahora devolver� el Nivel m�s alto usado
        '   sClaves se pasar� como par�metro
        BuscarClaves(sEntradaSimp, sClaves)
        Return CrearRespuesta(sClaves, sEntradaSimp)

    End Function

    Public Sub Inicializar()
        ' Pone a cero todos los valores de la �ltima respuesta usada

        If m_Releer Then
            m_Releer = False
            'm_Iniciado = False
            Iniciado = False
        End If

        'DoEvents
        'If Not m_Iniciado Then
        If Not Iniciado Then
            'm_Iniciado = True
            Iniciado = True
            ' Leer el fichero de palabras y respuestas
            LeerReglasEliza()
        Else
            ' Inicializar el valor del �ltimo item usado
            For Each tRegla In m_colReglas.Values
                ' poner a cero las respuestas normales
                tRegla.Respuestas.UltimoItem = 0
                ' poner a cero las respuestas de la secci�n Extras
                For Each tRespuestas In tRegla.Extras.Valores '.Values
                    tRespuestas.UltimoItem = 0
                Next
            Next
        End If
        ' Se considera siempre que es un usuario nuevo, leer la base de datos del usuario
        DatosUsuario()
    End Sub

    Private Function SustituirEnEntrada(sEntrada As String) As String
        ' Cambia las palabras de la entrada por las de la colecci�n
        ' de palabras simplificadas
        Dim sPalabra As String
        Dim sPalabra1 As String
        Dim sPalabra1Ant As String
        Dim sSeparador As String = ""
        Dim sSeparador1 As String = ""
        Dim nuevaEntrada As New System.Text.StringBuilder()

        'nuevaEntrada = ""
        Do
            ' Buscar dos palabras seguidas
            ' En sPalabra estar� la primera palabra antes de un separador,
            ' sEntrada tendr� el resto y sSeparador el separador.
            sPalabra = SiguientePalabra(sEntrada, sSeparador)
            sPalabra1 = SiguientePalabra(sEntrada, sSeparador1)
            sPalabra1Ant = sPalabra1

            PalabraOrig += 1

            If String.IsNullOrEmpty(sPalabra) = False AndAlso String.IsNullOrEmpty(sPalabra1) = False Then
                ' Si existen las dos palabras juntas
                If m_colRS.ExisteItem(sPalabra & sSeparador & sPalabra1) Then
                    sPalabra = m_colRS.Item(sPalabra & sSeparador & sPalabra1).Contenido
                    'nuevaEntrada = nuevaEntrada & sPalabra & sSeparador1
                    nuevaEntrada.Append(sPalabra)
                    nuevaEntrada.Append(sSeparador1)
                Else
                    ' sino, tomar la primera y seguir el proceso
                    If m_colRS.ExisteItem(sPalabra) Then
                        sPalabra = m_colRS.Item(sPalabra).Contenido
                    End If
                    'nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
                    nuevaEntrada.Append(sPalabra)
                    nuevaEntrada.Append(sSeparador)
                    ' dejar la entrada como estaba
                    ' invertir la palabra, en caso de que antes se
                    ' haya encontrado en los verbos
                    PalabraOrig -= 1
                    sPalabra1 = sPalabra1Ant
                    ' Creo que no hay que invertir las palabras (24/ene/23 15.57)
                    sEntrada = sPalabra1 & sSeparador1 & sEntrada
                    ' Estaba usando nuevaEntrada y deb�a ser sEntrada
                    'Dim sb = nuevaEntrada.ToString()
                    'nuevaEntrada.Clear()
                    'nuevaEntrada.Append(sPalabra1)
                    'nuevaEntrada.Append(sSeparador1)
                    'nuevaEntrada.Append(sb)
                    'nuevaEntrada.Append(sPalabra1)
                    'nuevaEntrada.Append(sSeparador1)
                End If
            Else
                ' S�lo deber�a cumplirse esta cl�usula
                If String.IsNullOrEmpty(sPalabra) = False Then
                    If m_colRS.ExisteItem(sPalabra) Then
                        sPalabra = m_colRS.Item(sPalabra).Contenido
                    End If
                    'nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
                    nuevaEntrada.Append(sPalabra)
                    nuevaEntrada.Append(sSeparador)
                End If
                If String.IsNullOrEmpty(sPalabra1) = False Then
                    If m_colRS.ExisteItem(sPalabra1) Then
                        sPalabra1 = m_colRS.Item(sPalabra1).Contenido
                    End If
                    'nuevaEntrada = nuevaEntrada & sPalabra1 & sSeparador1
                    nuevaEntrada.Append(sPalabra1)
                    nuevaEntrada.Append(sSeparador1)
                End If
            End If
        Loop While String.IsNullOrEmpty(sEntrada) = False
        Return nuevaEntrada.ToString()
    End Function

    Private Function SimplificarEntrada(sEntrada As String) As String
        ' Cambia las palabras de la entrada por las de la colecci�n
        ' de palabras simplificadas
        Dim sPalabra As String
        Dim sPalabra1 As String
        Dim sPalabra1Ant As String
        Dim sSeparador As String = ""
        Dim sSeparador1 As String = ""
        Dim nuevaEntrada As String

        nuevaEntrada = ""
        Do
            ' Buscar dos palabras seguidas
            sPalabra = SiguientePalabra(sEntrada, sSeparador)
            sPalabra1 = SiguientePalabra(sEntrada, sSeparador1)
            sPalabra1Ant = sPalabra1

            '==========================================================
            ' Creo que NO se deber�a dar por hallada una palabra
            ' cuando se ha "conjugado", ya que puede que est� entre
            ' las palabras claves y de esta forma no la encontrar�a
            '==========================================================

            PalabraOrig += 1
            sPalabra = ComprobarVerbos(sPalabra)
            If String.IsNullOrEmpty(sPalabra1) = False Then
                PalabraOrig += 1
                sPalabra1 = ComprobarVerbos(sPalabra1)
            End If

            ' De esta forma no funciona:
            'If Len(sPalabra) And Len(sPalabra1) Then
            If String.IsNullOrEmpty(sPalabra) = False AndAlso String.IsNullOrEmpty(sPalabra1) = False Then
                'Si existen las dos palabras juntas
                If m_colSimp.ExisteItem(sPalabra & sSeparador & sPalabra1) Then
                    sPalabra = m_colSimp.Item(sPalabra & sSeparador & sPalabra1).Contenido
                    nuevaEntrada = nuevaEntrada & sPalabra & sSeparador1
                Else
                    ' sino, tomar la primera y seguir el proceso
                    If m_colSimp.ExisteItem(sPalabra) Then
                        sPalabra = m_colSimp.Item(sPalabra).Contenido
                    End If
                    nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
                    ' dejar la entrada como estaba
                    ' invertir la palabra, en caso de que antes se
                    ' haya encontrado en los verbos
                    PalabraOrig -= 1
                    sPalabra1 = sPalabra1Ant
                    ' Creo que no hay que invertir las palabras (24/ene/23 15.57)
                    sEntrada = sPalabra1 & sSeparador1 & sEntrada
                    'sEntrada = sEntrada & sSeparador1 & sPalabra1
                End If
            Else
                ' S�lo deber�a cumplirse esta cl�usula
                If String.IsNullOrEmpty(sPalabra) = False Then
                    If m_colSimp.ExisteItem(sPalabra) Then
                        sPalabra = m_colSimp.Item(sPalabra).Contenido
                    End If
                    nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
                End If
                If String.IsNullOrEmpty(sPalabra1) = False Then
                    If m_colSimp.ExisteItem(sPalabra1) Then
                        sPalabra1 = m_colSimp.Item(sPalabra1).Contenido
                    End If
                    nuevaEntrada = nuevaEntrada & sPalabra1 & sSeparador1
                End If
            End If
        Loop While String.IsNullOrEmpty(sEntrada) = False
        Return nuevaEntrada
    End Function

    Private Function BuscarClaves(sEntrada As String, ByRef sClaves As String) As Integer
        ' Devuelve las claves halladas
        ' y asigna la variable Claves, devolviendo el n�mero de claves halladas
        Dim sPalabra As String
        Dim i As Integer

        sClaves = ""
        For Each tRegla In m_colReglas.Values
            i = sEntrada.IndexOf(tRegla.Contenido, StringComparison.OrdinalIgnoreCase)
            If i > -1 Then
                ' comprobar si es una palabra completa
                If sSeparadores.IndexOf(MidN(sEntrada, i + tRegla.Contenido.Length, 1)) > -1 Then
                    ' Si el car�cter anterior es un separador o el principio de la palabra
                    If i > 0 Then
                        Dim i1 = i
                        'i = sSeparadores.IndexOf(MidN(sEntrada, i1 - 1, 1))
                        i = sSeparadores.IndexOf(MidN(sEntrada, i1, 1))
                    End If
                    If i > -1 Then
                        For Each tRespuestas In tRegla.Extras.Valores '.Values
                            i = sEntrada.IndexOf(tRespuestas.Contenido, StringComparison.OrdinalIgnoreCase)
                            If i > -1 Then
                                ' A�adir al principio las "subclaves"
                                ' aunque no es necesario, ya que se le da un nivel mayor
                                If sSeparadores.IndexOf(MidN(sEntrada, i + tRespuestas.Contenido.Length, 1), StringComparison.OrdinalIgnoreCase) > -1 Then
                                    sPalabra = tRespuestas.Contenido
                                    sClaves = "{" & tRegla.Nivel + 1 & "}" & sPalabra & "," & sClaves
                                End If
                            End If
                        Next
                        sPalabra = tRegla.Contenido
                        sClaves = sClaves & "{" & tRegla.Nivel & "}" & sPalabra & ","
                    End If
                End If
            End If
        Next
        ' Ordenar las claves
        '---Ahora devuelve la de Nivel mayor al principio
        Return OrdenarClaves(sClaves)
    End Function

    Private Function OrdenarClaves(ByRef sClaves As String) As Integer
        ' Ordena las claves seg�n el Nivel
        ' El nivel estar� indicado entre llaves y cada palabra separada por una coma.
        '
        ' Nota: Esta forma de indicar el nivel no es como se hace en el fichero de palabras,
        '       ya que en el fichero se indica despu�s de la palabra que est� entre corchetes.
        '       Es la forma que se hace al llamar a esta funci�n.

        Dim numPalabras As Integer
        Dim i As Integer
        Dim tContenidos As cRespuestas
        Dim sNivel As String
        Dim sPalabra As String
        Dim aPalabras() As String
        Dim sTmp As New System.Text.StringBuilder()
        Dim LaMayor As Integer

        tContenidos = New cRespuestas

        'sTmp = sClaves
        sTmp.Append(sClaves)
        Do
            i = sClaves.IndexOf("}")
            If i > -1 Then
                sNivel = LeftN(sClaves, i)
                sClaves = MidN(sClaves, i + 1)
                i = sClaves.IndexOf(",")
                If i > -1 Then
                    sPalabra = LeftN(sClaves, i - 1)
                    sClaves = MidN(sClaves, i + 1)
                    tContenidos.Item(sPalabra).Contenido = sNivel
                End If
            End If
        Loop While String.IsNullOrEmpty(sClaves) = False
        numPalabras = tContenidos.Count
        If numPalabras > 0 Then
            ReDim aPalabras(0 To numPalabras)
            For i = 0 To numPalabras - 1
                ' se asignar� el nivel, (antes se a�ad�a la palabra)
                aPalabras(i) = tContenidos.Item(i).Contenido
            Next
            ' Clasificar de mayor a menor                               (01/Jun/98)
            Dim tClasificar As New Clasificar(deMayorAMenor:=True)
            Array.Sort(aPalabras, tClasificar)

            'LaMayor = Val(Mid$(tContenidos(aOrden(1)).Contenido, 2))
            LaMayor = CInt(tContenidos.Item(0).Contenido.Substring(1))
            'sTmp = ""
            sTmp.Clear()
            For i = 0 To numPalabras - 1
                'sTmp = sTmp & tContenidos.Item(i).ID & ","
                sTmp.Append(tContenidos.Item(i).ID)
                sTmp.Append(","c)
            Next
        End If
        sClaves = sTmp.ToString()
        ' Se devuelve el Nivel mayor de las claves
        Return LaMayor
    End Function

    Private Function CrearRespuesta(sClaves As String, sEntrada As String) As String
        ' Devuelve la respuesta,
        ' las claves estar�n separadas por comas

        Dim sRespuesta As String = ""
        Dim sPalabra As String = ""
        Dim i As Integer

        ' tomar la primera palabra y buscar la respuesta adecuada
        i = sClaves.IndexOf(",")
        If i > -1 Then
            sPalabra = LeftN(sClaves, i - 1).Trim()
        End If
        ' Si no hay una palabra clave
        If String.IsNullOrEmpty(sPalabra) Then
            sPalabra = "respuestas-aleatorias"
            ' en principio s�lo "recordar�" si no ha entendido

            ' Crear una respuesta con algo que dijo antes...
            ' si es que se tiene constancia de ello...
            sRespuesta = CrearRespuestaRecordando(sPalabra)
        End If
        ' Si no se ha encontrado una respuesta "aleatoria"
        If String.IsNullOrEmpty(sRespuesta) Then
            ' Aqu� se puede usar la nueva funci�n BuscarEsaClave        (11/Jun/98)
            sRespuesta = BuscarEsaClave(sPalabra)
        End If

        sRespuesta = ComprobarEspeciales(sRespuesta, sEntrada, sPalabra)

        ' Si la respuesta es la despedida, disparar un evento indicando que se termina
        ' con adios no llega aqu�, a ver si llega con quit que es lo que se indica en [*rs*]
        ' s�, llega con quit                                    (24/ene/23 22.05)
        '
        'If sRespuesta.StartsWith("adios", StringComparison.OrdinalIgnoreCase) Then
        'If sRespuesta.StartsWith("adios", StringComparison.OrdinalIgnoreCase) Then
        If sRespuesta.StartsWith("quit", StringComparison.OrdinalIgnoreCase) Then
            RaiseEvent Terminado()
        End If
        Return sRespuesta
    End Function

    Private Function QuitarCaracterEx(sValor As String, sCaracter As String, Optional sPoner As String = "") As String
        '--------------------------------------------------------------------------
        ' Cambiar/Quitar caracteres                                     (17/Sep/97)
        ' Si se especifica sPoner, se cambiar� por ese car�cter
        '
        ' Esta versi�n permite cambiar los caracteres    (17/Sep/97)
        ' y sustituirlos por el/los indicados
        ' a diferencia de QuitarCaracter, no se buscan uno a uno,
        ' sino todos juntos
        '--------------------------------------------------------------------------
        Dim i As Integer
        Dim sCh As String = ""
        Dim bPoner As Boolean
        Dim iLen As Integer

        If String.IsNullOrEmpty(sCaracter) Then
            Return sValor
        End If

        bPoner = False
        'If Not IsMissing(sPoner) Then
        If String.IsNullOrEmpty(sPoner) = False Then
            sCh = sPoner
            bPoner = True
        End If

        ' Esto no estaba...                                     (24/ene/23 21.46)
        iLen = sCaracter.Length
        If iLen = 0 Then
            Return sValor
        End If

        ' Si el caracter a quitar/cambiar es Chr$(0), usar otro m�todo
        If AscW(sCaracter) = 0 Then
            ' Quitar todos los chr$(0) del final
            Do While RightN(sValor, 1) = ChrW(0)
                sValor = LeftN(sValor, sValor.Length - 1)
                If sValor.Length = 0 Then Exit Do
            Loop
            iLen = 0 '1 usando Instr
            Do
                'i = InStr(iLen, sValor, sCaracter)
                i = sValor.IndexOf(sCaracter, iLen)
                If i > -1 Then
                    If bPoner Then
                        'sValor = LeftN(sValor, i - 1) & sCh & MidN(sValor, i + 1)
                        sValor = LeftN(sValor, i) & sCh & MidN(sValor, i + 1)
                    Else
                        'sValor = LeftN(sValor, i - 1) & MidN(sValor, i + 1)
                        sValor = LeftN(sValor, i) & MidN(sValor, i + 1)
                    End If
                    iLen = i
                Else
                    ' ya no hay m�s, salir del bucle
                    Exit Do
                End If
            Loop
        Else
            i = 0 ' 1
            'Do While i <= sValor.Length
            Do While i < sValor.Length
                If MidN(sValor, i, iLen) = sCaracter Then
                    If bPoner Then
                        'sValor = LeftN(sValor, i - 1) & sCh & MidN(sValor, i + iLen)
                        sValor = LeftN(sValor, i) & sCh & MidN(sValor, i + iLen)
                        i -= 1
                        ' Si lo que hay que poner est� incluido en
                        ' lo que se busca, incrementar el puntero
                        '                                   (11/Jun/98)
                        If sCh.IndexOf(sCaracter) > -1 Then
                            i += 1
                        End If
                    Else
                        'sValor = LeftN(sValor, i - 1) & MidN(sValor, i + iLen)
                        sValor = LeftN(sValor, i) & MidN(sValor, i + iLen)
                    End If
                End If
                i += 1
            Loop
        End If

        Return sValor
    End Function

    Private Function SiguientePalabra(
            ByRef sFrase As String,
            ByRef sSeparador As String,
            Optional queSeparador As String = "") As String
        ' Busca la siguiente palabra de la frase de entrada
        ' En la frase se devolver� el resto sin la palabra hallada

        ' Si se especifica un caracter (o varios) en queSeparador
        ' se usar�n esos para comprobar cual es la siguiente palabra,
        ' sino, se usar� el contenido de sSeparadores
        Dim i As Integer
        Dim sPalabra As New System.Text.StringBuilder()
        Dim sLosSeparadores As String

        ' Nueva forma de comprobar una nueva palabra                    (10/Jun/98)
        If String.IsNullOrEmpty(queSeparador) = False Then
            sLosSeparadores = queSeparador
        Else
            sLosSeparadores = sSeparadores
        End If

        'sPalabra = ""
        sFrase = sFrase.Trim() & " "
        For i = 0 To sFrase.Length - 1
            If sLosSeparadores.IndexOf(sFrase(i)) > -1 Then
                sSeparador = sFrase(i)
                sFrase = sFrase.Substring(i + 1)
                Exit For
            Else
                sPalabra.Append(sFrase(i))
            End If
            'If sLosSeparadores.IndexOf(MidN(sFrase, i, 1)) > -1 Then
            '    ' Devolver el resto de la frase y el separador
            '    sSeparador = MidN(sFrase, i, 1)
            '    sFrase = MidN(sFrase, i + 1)
            '    Exit For
            'Else
            '    'sPalabra &= MidN(sFrase, i, 1)
            '    sPalabra.Append(MidN(sFrase, i, 1))
            'End If
        Next
        Return sPalabra.ToString()
    End Function

    ''' <summary>
    ''' Lee las reglas de los ficheros del directorio "palabras" donde est� el ejecutable.
    ''' </summary>
    ''' <remarks>Lee los ficheros y crea las reglas y sub-reglas.</remarks>
    Private Sub LeerReglasEliza()
        ' Leer el/los ficheros de palabras clave y respuestas,
        ' as� como las reglas de simplificaci�n

        Dim sFic As String
        Dim sDir As String
        Dim otrosEliza As String()

        ' El directorio de datos
        sDir = System.IO.Path.Combine(AppPath(), "palabras")

        ' El primer fichero en leer ser� Eliza_SPA.txt
        ' El resto se leer�n a continuaci�n, permitiendo de esta forma
        ' sustituir algunas reglas y palabras existentes
        sFic = System.IO.Path.Combine(sDir, "Eliza_SPA.txt")
        'If Len(Dir$(sFic)) > 0 Then
        If System.IO.File.Exists(sFic) Then
            LeerReglas(sFic)
        End If
        ' Buscar los ficheros llamados ElizaSP_*.txt            (23/ene/23 09.39)
        otrosEliza = System.IO.Directory.GetFiles(sDir, "ElizaSP_*.txt")
        For Each sFic In otrosEliza
            LeerReglas(sFic)
        Next
        ' Buscar los ficheros llamados ElizaVB_*.txt, y leerlos
        otrosEliza = System.IO.Directory.GetFiles(sDir, "ElizaVB_*.txt")
        For Each sFic In otrosEliza
            LeerReglas(sFic)
        Next

        ' Convertir las claves que tienen el formato:
        ' [clave {* xxx; yyy}] en distintas entradas.

        ' Las diferentes entradas TIENEN QUE ESTAR separadas por ;

        Dim tRegla As cRegla
        Dim tRespuestas As cRespuestas
        Dim sPalabra As String
        Dim sPalabra1 As String
        Dim sSeparador As String = ""
        Dim j As Integer

        Dim sSubKey As String
        Dim i As Integer
        Dim sTmp As String

        Dim sContenidoRegla As String
        Dim posContenidoRegla As Integer
        Dim nContenidoRegla As Integer
        Dim rContenidoRegla As cRegla = Nothing

        sSubKey = ""

        '
        ' No se puede hacer for each porque se a�aden reglas
        'For Each tRegla In m_col.Values
        ' De esta forma, aunque se a�adan m�s reglas,
        ' terminar� cuando se llegue al valor original de m_col.Values.Count - 1
        'For n = 0 To m_col.Values.Count - 1
        '

        ' De esta forma se contin�a aunque se a�adan m�s reglas
        Dim n As Integer = -1
        'Dim cuantasReglasInicial = m_col.Values.Count - 1
        Dim cuantasReglas = m_colReglas.Values.Count - 1 'cuantasReglasInicial
        Do
            n += 1
            ' ajustar el l�mite por si se a�aden nuevas reglas
            If m_colReglas.Values.Count - 1 > cuantasReglas Then
                cuantasReglas = m_colReglas.Values.Count - 1
            End If
            If n > cuantasReglas Then Exit Do

            'If n > 469 Then
            '    Debug.WriteLine("{0}, {1}, {2}", n, cuantasReglasInicial, cuantasReglas)
            'End If

            tRegla = m_colReglas.Values.ElementAt(n)

            ' Para probar si encontraba esto
            'If tRegla.Contenido.Contains("color {* son tus") Then
            '    i = 0
            'End If
            nContenidoRegla = 0

            ' Comprobar si tiene {* ...}
            i = tRegla.Contenido.IndexOf("{*")
            If i > -1 Then
                rContenidoRegla = New cRegla()
                sContenidoRegla = tRegla.Contenido
                posContenidoRegla = i
                Do While i > -1
                    ' En el caso que se ponga alguna palabra despu�s
                    ' de la llave de cierre, se usar� tambi�n
                    j = sContenidoRegla.IndexOf("}", i)
                    sPalabra1 = ""
                    If j > -1 Then
                        sPalabra1 = MidN(sContenidoRegla, j + 1).Trim()
                        If sPalabra1.Length > 0 Then
                            sPalabra1 = " " & sPalabra1
                        End If
                    End If
                    sSubKey = LeftN(sContenidoRegla, i)
                    sTmp = MidN(sContenidoRegla, i + 2)

                    ' La siguiente palabra ser� la que est� separada por
                    ' un punto y coma, de esta forma se permiten palabras
                    ' de m�s de "una palabra"
                    sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                    ' Esto es necesario, ya que en la colecci�n la clave
                    ' es el contenido anterior, es decir lo que hay en sKey

                    ' Para que no hayan dos espacios seguidos en la clave
                    sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                    With Reglas(sPalabra)
                        .Nivel = tRegla.Nivel
                        .Aleatorio = tRegla.Aleatorio
                        .Respuestas.Add("*equal:=" & tRegla.Contenido)
                    End With
                    ' Para probar
                    With rContenidoRegla.Item(sPalabra)
                        .Nivel = tRegla.Nivel
                        .Aleatorio = tRegla.Aleatorio
                        .Respuestas.Add("*equal:=" & tRegla.Contenido)
                    End With
                    nContenidoRegla += 1

                    Do While sTmp.Length > 0
                        ' Si no tiene este separador se devuelve lo mismo
                        sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                        If sPalabra.Length > 0 Then
                            i = sPalabra.IndexOf("}")
                            If i > -1 Then
                                sPalabra = LeftN(sPalabra.Trim(), i)
                                sTmp = ""
                            End If
                            sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                            With Reglas(sPalabra)
                                .Nivel = tRegla.Nivel
                                .Aleatorio = tRegla.Aleatorio
                                .Respuestas.Add("*equal:=" & tRegla.Contenido)
                            End With
                            ' Para probar
                            With rContenidoRegla.Item(sPalabra)
                                .Nivel = tRegla.Nivel
                                .Aleatorio = tRegla.Aleatorio
                                .Respuestas.Add("*equal:=" & tRegla.Contenido)
                            End With
                            nContenidoRegla += 1
                        End If
                    Loop

                    i = sContenidoRegla.IndexOf("{*", posContenidoRegla + 2)
                    posContenidoRegla = i
                Loop
                '' En el caso que se ponga alguna palabra despu�s
                '' de la llave de cierre, se usar� tambi�n
                'j = tRegla.Contenido.IndexOf("}")
                'sPalabra1 = ""
                'If j > -1 Then
                '    sPalabra1 = MidN(tRegla.Contenido, j + 1).Trim()
                '    If sPalabra1.Length > 0 Then
                '        sPalabra1 = " " & sPalabra1
                '    End If
                'End If
                'sSubKey = LeftN(tRegla.Contenido, i)
                'sTmp = MidN(tRegla.Contenido, i + 2)
                ''Dim sSubKey0 = LeftN(tRegla.Contenido, i - 1)
                ''Dim sTmp0 = MidN(tRegla.Contenido, i + 2)

                '' La siguiente palabra ser� la que est� separada por
                '' un punto y coma, de esta forma se permiten palabras
                '' de m�s de "una palabra"
                'sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                '' Esto es necesario, ya que en la colecci�n la clave
                '' es el contenido anterior, es decir lo que hay en sKey

                '' Para que no hayan dos espacios seguidos en la clave
                'sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                'With Reglas(sPalabra)
                '    .Nivel = tRegla.Nivel
                '    .Aleatorio = tRegla.Aleatorio
                '    .Respuestas.Add("*equal:=" & tRegla.Contenido)
                'End With

                'Do While sTmp.Length > 0
                '    ' Si no tiene este separador se devuelve lo mismo
                '    sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                '    If sPalabra.Length > 0 Then
                '        i = sPalabra.IndexOf("}")
                '        If i > -1 Then
                '            sPalabra = LeftN(sPalabra.Trim(), i)
                '            sTmp = ""
                '        End If
                '        sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                '        With Reglas(sPalabra)
                '            .Nivel = tRegla.Nivel
                '            .Aleatorio = tRegla.Aleatorio
                '            .Respuestas.Add("*equal:=" & tRegla.Contenido)
                '        End With
                '    End If
                'Loop
            End If

            If nContenidoRegla > 0 Then
                'Debug.WriteLine("{0}, {1}", tRegla.Contenido, nContenidoRegla)
                Debug.WriteLine("{0}, {1}", tRegla.Contenido, rContenidoRegla.LasReglas.Count)

            End If

            ' Buscar en las sub-claves *extras*

            ' Si aqu� se encuentra {* se deber� crear una clave
            ' principal, ya que los *equal:= s�lo funcionan con
            ' las claves principales.
            '---Tambi�n busca en las sub-claves extras      (10/Jun/98)

            'For Each tRespuestas In tRegla.Extras.Valores.Values
            For m = 0 To tRegla.Extras.Valores.Count - 1 ' .Values.Count - 1
                tRespuestas = tRegla.Extras.Valores.ElementAt(m) '.Values.ElementAt(m)
                i = tRespuestas.Contenido.IndexOf("{*")
                If i > -1 Then
                    ' En el caso que se ponga alguna palabra despu�s
                    ' de la llave de cierre, se usar� tambi�n
                    j = tRespuestas.Contenido.IndexOf("}")
                    sPalabra1 = ""
                    If j > -1 Then
                        sPalabra1 = MidN(tRespuestas.Contenido, j + 1).Trim()
                        If sPalabra1.Length > 0 Then
                            sPalabra1 = " " & sPalabra1
                        End If
                    End If
                    'sSubKey = LeftN(tRespuestas.Contenido, i - 1)
                    sSubKey = LeftN(tRespuestas.Contenido, i)
                    sTmp = MidN(tRespuestas.Contenido, i + 2)
                    sPalabra = SiguientePalabra(sTmp, sSeparador, ";")

                    sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                    Reglas(tRegla.Contenido).Extras.Item(sPalabra).Add("*equal:=" & tRespuestas.Contenido)
                    '$Reglas(tRegla.Contenido).Extras(sPalabra).Add "*equal:=" & tRegla.Contenido

                    Do While sTmp.Length > 0
                        sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                        If sPalabra.Length > 0 Then
                            i = sPalabra.IndexOf("}")
                            If i > -1 Then
                                'sPalabra = LeftN(sPalabra.Trim(), i - 1)
                                sPalabra = LeftN(sPalabra.Trim(), i)
                                sTmp = ""
                            End If

                            sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                            Reglas(tRegla.Contenido).Extras.Item(sPalabra).Add("*equal:=" & tRespuestas.Contenido)
                            '$Reglas(tRegla.Contenido).Extras(sPalabra).Add "*equal:=" & tRegla.Contenido
                        End If
                    Loop
                End If
            Next
        Loop 'Next
        '
        '--------------------------------------------------------------------------
        '   Formato del fichero de palabras y respuestas (Reglas)
        '--------------------------------------------------------------------------
        ' En el fichero Eliza_SPA.txt est�n explicadas las claves
        ' y dem�s cosas a usar para que Eliza entienda lo que se le dice
        '--------------------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' Lee las reglas del fichero indicado, sin analizarlas ni crear las sub-reglas.
    ''' </summary>
    ''' <param name="sFic">El path completo del fichero.</param>
    ''' <remarks>El formato del fichero debe ser UTF8.</remarks>
    Private Sub LeerReglas(sFic As String)
        ' Leer el fichero de palabras indicado en el par�metro          (17/Sep/02)
        Dim sTmp As String = ";"
        Dim sTmpLower As String
        Dim sKey As String = ""
        Dim sSubKey As String = ""
        Dim i As Integer
        Dim UsarRs As Boolean
        Dim esPeek As Boolean = False

        ' Comprobar si existe el fichero, si no es as�, salir.
        ' �No comprobarlo, ya que se altera lo que Dir$ devuelve!
        ' (se supone que existe el fichero, ya que
        ' este procedimiento es llamado desde LeerReglasEliza)
        'If Len(Dir$(sFic)) = 0 Then Exit Sub

        Using sr As New System.IO.StreamReader(sFic, System.Text.Encoding.UTF8, True)
            Do While Not sr.EndOfStream
                ' si es true es que no hay que leer el contenido
                If esPeek = False Then
                    sTmp = sr.ReadLine().Trim()
                End If
                esPeek = False
                ' Si no hay m�s entradas,
                ' esto es por si se quiere crear un fichero de prueba
                ' y se limitar� hasta d�nde se va a examinar...
                If sTmp.StartsWith(";fin", StringComparison.OrdinalIgnoreCase) Then
                    Exit Do
                End If
                ' Si hay un comentario o est� vac�a, no procesar
                If String.IsNullOrEmpty(sTmp) OrElse sTmp.StartsWith(";") Then
                    Continue Do
                End If
                'If sTmp.Length > 0 AndAlso sTmp.Substring(0, 1) <> ";" Then
                sTmpLower = sTmp.ToLower()
                ' Si no es una secci�n EXTRAS
                If sTmpLower <> "[*extras*]" Then
                    ' Si son reglas de simplificaci�n
                    If sTmpLower = "[*rs*]" Then
                        UsarRs = True
                        ' leer las reglas de simplificaci�n,
                        ' siempre deben ir por pares
                        Do While Not sr.EndOfStream
                            ' Las palabras estar�n separadas por comas
                            'Line Input #nFic, sTmp
                            sTmp = sr.ReadLine().Trim()
                            If String.IsNullOrEmpty(sTmp) OrElse sTmp.StartsWith(";") Then
                                Continue Do
                            End If
                            sTmpLower = sTmp.ToLower()
                            'If sTmp.Length > 0 AndAlso sTmp.Substring(0, 1) <> ";" Then
                            If sTmpLower = "[/rs]" Then
                                Exit Do
                            ElseIf sTmpLower = "[*simp*]" Then
                                UsarRs = False
                            End If
                            i = sTmp.IndexOf(",")
                            If i > -1 Then
                                sKey = sTmp.Substring(0, i)
                                sTmp = sTmp.Substring(i + 1)
                                ' Si tiene el signo @ al principio,
                                ' se crear� una doble entrada
                                ' Por ejemplo: @soy,eres
                                ' crear�a soy,eres y eres,soy
                                If sKey.StartsWith("@") Then
                                    sKey = sKey.Substring(1)
                                    If UsarRs Then
                                        m_colRS.Item(sTmp).Contenido = sKey
                                    Else
                                        m_colSimp.Item(sTmp).Contenido = sKey
                                    End If
                                End If
                                ' a�adirla a la colecci�n
                                If UsarRs Then
                                    m_colRS.Item(sKey).Contenido = sTmp
                                Else
                                    m_colSimp.Item(sKey).Contenido = sTmp
                                End If
                            End If
                            'End If
                        Loop
                    ElseIf sTmpLower = "[*verbos*]" Then
                        ' leer los verbos y sus terminaciones
                        Do While Not sr.EndOfStream
                            'Se pondr�n los verbos y se a�adir� a la
                            ' colecci�n sin la terminaci�n, en el formato
                            ' m_Verbos.Item("am")="ar"
                            ' m_Verbos.Item("com")="er"
                            sTmp = sr.ReadLine().Trim()
                            sTmpLower = sTmp.ToLower()
                            If String.IsNullOrEmpty(sTmp) OrElse sTmp.StartsWith(";") Then
                                Continue Do
                            End If
                            'If sTmp.Length > 0 AndAlso sTmp.Substring(0, 1) <> ";" Then
                            If sTmpLower = "[/verbos]" Then
                                Exit Do
                            End If
                            i = sTmp.Length
                            sKey = sTmp.Substring(0, i - 2)
                            sTmp = RightN(sTmp, 2)
                            If sKey.StartsWith("%") Then
                                ' quitarle el %
                                sKey = sKey.Substring(1)
                                ' cambiar la �ltima vocal
                                ' por una acentuada
                                ' s�lo se busca la 'i'
                                If RightN(sKey, 1) = "i" Then
                                    sSubKey = LeftN(sKey, sKey.Length - 1) & "�"
                                    m_Verbos.Item(sSubKey).Contenido = sTmp
                                End If
                                sSubKey = ""
                            End If
                            ' a�adirlo a la colecci�n
                            m_Verbos.Item(sKey).Contenido = sTmp
                            'End If
                        Loop
                    Else
                        ' debe ser una clave normal
                        If sTmp.StartsWith("[") Then
                            ' Es una Palabra Clave
                            ' Quitarle los corchetes
                            sKey = sTmp.Substring(1, sTmp.Length - 2)
                            sSubKey = ""
                            ' Leer el nivel
                            sTmp = sr.ReadLine().Trim()
                            ' Si tiene @ quit�rselo antes de convertir en n�mero
                            '                                       ( 9/Jun/98)
                            ' Si tiene @ es para tomarlo aleatoriamente
                            If sTmp.EndsWith("@") Then
                                sTmp = sTmp.Replace("@", "")
                                Reglas(sKey).Aleatorio = True
                            Else
                                Reglas(sKey).Aleatorio = False
                            End If

                            Dim n = 0
                            Dim __ = Integer.TryParse(sTmp, n)
                            Reglas(sKey).Nivel = n
                        Else
                            ' Sino es una clave, es una respuesta
                            ' de la �ltima clave encontrada.
                            ' A�adirla a la clave actual
                            Reglas(sKey).Respuestas.Add(sTmp)
                        End If
                    End If
                Else
                    ' Comprobar si hay palabras EXTRAS (subClaves)
                    Do While Not sr.EndOfStream
                        ' Guardar la posici�n actual del fichero
                        'pSeek = sr.Peek ' Seek(nFic)
                        'esPeek = True
                        sTmp = sr.ReadLine().Trim()
                        sTmpLower = sTmp.ToLower()
                        ' si es el final de la secci�n de EXTRAS:
                        '   salir del bucle
                        If String.IsNullOrEmpty(sTmp) OrElse sTmp.StartsWith(";") Then
                            Continue Do
                        End If
                        'If sTmp.Length > 0 AndAlso sTmp.Substring(0, 1) <> ";" Then
                        If sTmpLower = "[/extras]" Then
                            Exit Do
                        End If
                        ' Si es una clave, empezar� por [
                        If sTmp.StartsWith("[") Then
                            sSubKey = sTmp.Substring(1, sTmp.Length - 2)
                            Reglas(sKey).Extras.Item(sSubKey).Contenido = sSubKey
                        Else
                            ' si no es una clave

                            ' Si no hay subClave especificada:
                            If String.IsNullOrEmpty(sSubKey) Then
                                ' posicionar el puntero del fichero
                                ' y salir del bucle
                                'Seek nFic, pSeek
                                esPeek = True
                                Exit Do
                            Else
                                ' debe ser una respuesta para esta subClave
                                Reglas(sKey).Extras.Item(sSubKey).Add(sTmp)
                            End If
                        End If
                        'End If
                    Loop
                End If
                'End If
            Loop
        End Using
    End Sub

#Region " Se usa directamente en el formulario Eliza_claves "

    'Public Function AsignarPalabras(
    '        ByVal unList As ListBox,
    '        Optional ByVal unList1 As ListBox,
    '        Optional ByVal esClave As eTiposDeClaves = eTiposDeClaves.eClaves,
    '        Optional ByRef sClave As String = "") As Decimal 'Currency
    '    ' Asigna las palabras a los listbox del form
    '    ' Devolver� el n�mero de palabras asignadas al ListBox
    '    Dim tRegla As cRegla
    '    Dim tRespuestas As cRespuestas
    '    Dim tContenido As cContenido
    '    Dim i As Integer, j As Integer

    '    unList.Clear()
    '    Select Case esClave
    '        Case eTiposDeClaves.eClaves
    '            unList1.Clear
    '            For Each tRegla In m_col.Values
    '                unList.AddItem(tRegla.Contenido)
    '                i = i + 1
    '                For Each tRespuestas In tRegla.Extras.Valores.Values
    '                    unList1.AddItem(tRespuestas.Contenido)
    '                    j = j + 1
    '                Next
    '            Next
    '        Case eTiposDeClaves.eExtras
    '            'Set tRegla = m_col(sClave)
    '            tRegla = m_col(sClave)
    '            If tRegla.Extras.Count() > 0 Then
    '                unList.AddItem("---Extras---")
    '                For Each tRespuestas In tRegla.Extras.Valores.Values
    '                    unList.AddItem(tRespuestas.Contenido)
    '                    i = i + 1
    '                Next
    '            End If
    '            If tRegla.Respuestas.Count > 0 Then
    '                unList.AddItem("---Respuestas---")
    '                For Each tContenido In tRegla.Respuestas.Valores.Values
    '                    unList.AddItem(tContenido.Contenido)
    '                    j = j + 1
    '                Next
    '            End If
    '        Case eTiposDeClaves.eExtras2
    '            For Each tRegla In m_col.Values
    '                If tRegla.Extras.Count > 0 Then
    '                    For Each tRespuestas In tRegla.Extras.Valores.Values
    '                        If tRespuestas.Contenido = sClave Then
    '                            For Each tContenido In tRespuestas.Valores.Values
    '                                unList.AddItem(tContenido.Contenido)
    '                                j = j + 1
    '                            Next
    '                        End If
    '                    Next
    '                End If
    '            Next
    '        Case eTiposDeClaves.eVerbos
    '            For Each tContenido In m_Verbos.Valores.Values
    '                unList.AddItem(tContenido.ID & " -- " & tContenido.Contenido)
    '                i = i + 1
    '            Next
    '        Case eTiposDeClaves.eRS
    '            For Each tContenido In m_colRS.Valores.Values
    '                unList.AddItem(tContenido.ID & " -- " & tContenido.Contenido)
    '                i = i + 1
    '            Next
    '        Case eTiposDeClaves.eSimp
    '            For Each tContenido In m_colSimp.Valores.Values
    '                unList.AddItem(tContenido.ID & " -- " & tContenido.Contenido)
    '                i = i + 1
    '            Next
    '        Case eTiposDeClaves.eRec
    '            For Each tContenido In m_colRec.Valores.Values
    '                unList.AddItem(tContenido.ID & " -- " & tContenido.Contenido)
    '                i = i + 1
    '            Next
    '        Case eTiposDeClaves.eBU
    '            For Each tContenido In BaseUser.Valores.Values
    '                unList.AddItem(tContenido.ID & " -- " & tContenido.Contenido)
    '                i = i + 1
    '            Next
    '    End Select
    '    'AsignarPalabras = CDec(i + j / 1000)
    '    Return CDec(i + j / 1000)
    'End Function

#End Region

    Public Property Sexo As eSexo
        Get
            Return m_Sexo
        End Get
        Set(value As eSexo)
            Select Case value
                Case eSexo.Masculino, eSexo.Femenino
                    'nada
                    m_Sexo = value
                Case Else
                    m_Sexo = eSexo.Masculino
            End Select
            'm_Sexo = NewSexo
        End Set
    End Property

    Private Function ComprobarVerbos(sPalabra As String) As String
        ' Comprueba si cumple las nomas indicadas y busca en la lista de verbos,
        ' si lo encuentra, lo convierte convenientemente.
        ' Devolver� la nueva palabra o la original

        Dim tContenido As cContenido
        Dim hallado As Boolean
        Dim i As Integer
        Dim sInfinitivo As String
        Dim sPalabraAnt As String

        If String.IsNullOrEmpty(sPalabra) Then
            'ComprobarVerbos = ""
            'Exit Function
            Return ""
        End If

        ' Antes de convertir el verbo, comprobar si la palabra anterior
        ' es 'el' o 'un' en ese caso, no se conjugar�
        ' el juego X -> el juegas X, sino el juego -> el juego
        hallado = False
        If PalabraOrig > 1 AndAlso PalabraOrig < PalabrasOrig Then
            If " el un al del ".IndexOf(FraseOrig(PalabraOrig - 1)) > -1 Then
                hallado = True
            End If
        End If

        If Not hallado Then
            'Jugar con las terminaciones:
            'arme->arte, erme->erte, irme->irte
            'Se puede simplificar con rme->rte
            i = sPalabra.Length - 3
            'Guardar la palabra por si resulta que no es un verbo
            sPalabraAnt = sPalabra
            hallado = False
            Select Case RightN(sPalabra, 3)
                Case "rme"
                    sPalabra = LeftN(sPalabra, i) & "rte"
                    hallado = True
                Case "rte"
                    sPalabra = LeftN(sPalabra, i) & "rme"
                    hallado = True
            End Select
            'Comprobar las terminaciones telo/melo tela/mela
            Select Case RightN(sPalabra, 4)
                Case "melo"
                    sPalabra = LeftN(sPalabra, i - 1) & "telo"
                    i -= 2
                    hallado = True
                Case "telo"
                    sPalabra = LeftN(sPalabra, i - 1) & "melo"
                    i -= 2
                    hallado = True
                Case "mela"
                    sPalabra = LeftN(sPalabra, i - 1) & "tela"
                    i -= 2
                    hallado = True
                Case "tela"
                    sPalabra = LeftN(sPalabra, i - 1) & "mela"
                    i -= 2
                    hallado = True
            End Select

            'Comprobar si es uno de los verbos conocidos
            'el infinitivo ser� la longitud de la palabra - 2
            'el valor de 'i' es 3 caracteres menos de la longitud total
            If hallado Then
                'pero s�lo tomamos la parte sin la forma infinitiva,
                'de am-arme nos quedamos con am
                sInfinitivo = LeftN(sPalabra, i - 1)
                hallado = False
                For Each tContenido In m_Verbos.Valores '.Values
                    If sInfinitivo = tContenido.ID Then
                        hallado = True
                        Exit For
                    End If
                Next
                'Si no es un verbo conocido, dejamos la palabra como estaba
                If Not hallado Then
                    sPalabra = sPalabraAnt
                End If
            End If

            'Si no se ha hallado una de estas formas,
            'se comprobar� si es un verbo
            If Not hallado Then
                'primero evaluar los verbos
                For Each tContenido In m_Verbos.Valores '.Values
                    hallado = False
                    With tContenido
                        'If InStr(" " & sPalabra, " " & .ID) > 0 Then
                        If (" " & sPalabra).IndexOf(" " & .ID) > -1 Then
                            '------------------------------------------------------
                            ' Terminaciones en 'ar'
                            '------------------------------------------------------
                            If .Contenido = "ar" Then
                                ' presente

                                ' Para: doy -> das y viceversa,         (15/Sep/02)
                                ' Aunque se supone que deber�a corregirse con
                                ' las reglas de simplificaci�n.
                                If sPalabra = .ID & "oy" Then
                                    sPalabra = .ID & "as"
                                    hallado = True

                                ElseIf sPalabra = .ID & "o" Then
                                    sPalabra = .ID & "as"
                                    hallado = True
                                ElseIf sPalabra = .ID & "as" Then
                                    sPalabra = .ID & "o"
                                    hallado = True
                                    'pret�rito imperf.
                                ElseIf sPalabra = .ID & "aba" Then
                                    sPalabra = .ID & "abas"
                                    hallado = True
                                ElseIf sPalabra = .ID & "abas" Then
                                    sPalabra = .ID & "aba"
                                    hallado = True
                                    'pret�rito perf. simple (pret.indef.)
                                ElseIf sPalabra = .ID & "�" Then
                                    sPalabra = .ID & "aste"
                                    hallado = True
                                ElseIf sPalabra = .ID & "aste" Then
                                    sPalabra = .ID & "�"
                                    hallado = True
                                    'futuro (imperf.)
                                ElseIf sPalabra = .ID & "ar�" Then
                                    sPalabra = .ID & "ar�s"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ar�s" Then
                                    sPalabra = .ID & "ar�"
                                    hallado = True
                                    'condicional
                                ElseIf sPalabra = .ID & "ar�a" Then
                                    sPalabra = .ID & "ar�as"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ar�as" Then
                                    sPalabra = .ID & "ar�a"
                                    hallado = True
                                    'Presente de subjuntivo
                                ElseIf sPalabra = .ID & "e" Then
                                    sPalabra = .ID & "es"
                                    hallado = True
                                    ' No convertir de en des            (18/Sep/02)
                                    If sPalabra = "des" Then hallado = False
                                ElseIf sPalabra = .ID & "es" Then
                                    sPalabra = .ID & "e"
                                    hallado = True
                                    'Pret�r.imperf. de subjuntivo (1�)
                                ElseIf sPalabra = .ID & "ara" Then
                                    sPalabra = .ID & "aras"
                                    hallado = True
                                ElseIf sPalabra = .ID & "aras" Then
                                    sPalabra = .ID & "ara"
                                    hallado = True
                                    'Pret�r.imperf. de subjuntivo (2�)
                                ElseIf sPalabra = .ID & "ase" Then
                                    sPalabra = .ID & "ases"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ases" Then
                                    sPalabra = .ID & "ase"
                                    hallado = True
                                    'Futuro imperf. de subjuntivo
                                ElseIf sPalabra = .ID & "are" Then
                                    sPalabra = .ID & "ares"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ares" Then
                                    sPalabra = .ID & "are"
                                    hallado = True

                                ElseIf sPalabra = .ID & "�ndome" Then
                                    sPalabra = .ID & "�ndote"
                                    hallado = True
                                ElseIf sPalabra = .ID & "�ndote" Then
                                    sPalabra = .ID & "�ndome"
                                    hallado = True
                                    '$comprobar si es gen�rico
                                ElseIf sPalabra = .ID & "me" Then
                                    sPalabra = .ID & "te"
                                    hallado = True
                                ElseIf sPalabra = .ID & "te" Then
                                    sPalabra = .ID & "me"
                                    hallado = True
                                End If
                                '------------------------------------------------------
                                ' Terminaciones en 'er' e 'ir'
                                '------------------------------------------------------
                            ElseIf .Contenido = "er" OrElse .Contenido = "ir" Then
                                'presente
                                If sPalabra = .ID & "o" Then
                                    sPalabra = .ID & "es"
                                    hallado = True
                                ElseIf sPalabra = .ID & "es" Then
                                    sPalabra = .ID & "o"
                                    hallado = True
                                    'pret�rito imperf.
                                ElseIf sPalabra = .ID & "�a" Then
                                    sPalabra = .ID & "�as"
                                    hallado = True
                                ElseIf sPalabra = .ID & "�as" Then
                                    sPalabra = .ID & "�a"
                                    hallado = True
                                    'pret�rito perf. simple (pret.indef.)
                                ElseIf sPalabra = .ID & "�" Then
                                    sPalabra = .ID & "iste"
                                    hallado = True
                                ElseIf sPalabra = .ID & "iste" Then
                                    sPalabra = .ID & "�"
                                    hallado = True
                                    'futuro (imperf.) para "er"
                                ElseIf sPalabra = .ID & "er�" Then
                                    sPalabra = .ID & "er�s"
                                    hallado = True
                                ElseIf sPalabra = .ID & "er�s" Then
                                    sPalabra = .ID & "er�"
                                    hallado = True
                                    'futuro (imperf.) para "ir"
                                ElseIf sPalabra = .ID & "ir�" Then
                                    sPalabra = .ID & "ir�s"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ir�s" Then
                                    sPalabra = .ID & "ir�"
                                    hallado = True
                                    'condicional para "er"
                                ElseIf sPalabra = .ID & "er�a" Then
                                    sPalabra = .ID & "er�as"
                                    hallado = True
                                ElseIf sPalabra = .ID & "er�as" Then
                                    sPalabra = .ID & "er�a"
                                    hallado = True
                                    'condicional para "ir"
                                ElseIf sPalabra = .ID & "ir�a" Then
                                    sPalabra = .ID & "ir�as"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ir�as" Then
                                    sPalabra = .ID & "ir�a"
                                    hallado = True
                                    'Presente de subjuntivo
                                ElseIf sPalabra = .ID & "a" Then
                                    sPalabra = .ID & "as"
                                    hallado = True
                                ElseIf sPalabra = .ID & "as" Then
                                    sPalabra = .ID & "a"
                                    hallado = True
                                    'Pret�r.imperf. de subjuntivo (1�)
                                ElseIf sPalabra = .ID & "iera" Then
                                    sPalabra = .ID & "ieras"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ieras" Then
                                    sPalabra = .ID & "iera"
                                    hallado = True
                                    'Pret�r.imperf. de subjuntivo (2�)
                                ElseIf sPalabra = .ID & "iese" Then
                                    sPalabra = .ID & "ieses"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ieses" Then
                                    sPalabra = .ID & "iese"
                                    hallado = True
                                    'Futuro imperf. de subjuntivo
                                ElseIf sPalabra = .ID & "iere" Then
                                    sPalabra = .ID & "ieres"
                                    hallado = True
                                ElseIf sPalabra = .ID & "ieres" Then
                                    sPalabra = .ID & "iere"
                                    hallado = True

                                ElseIf sPalabra = .ID & "i�ndome" Then
                                    sPalabra = .ID & "i�ndote"
                                    hallado = True
                                ElseIf sPalabra = .ID & "i�ndote" Then
                                    sPalabra = .ID & "i�ndome"
                                    hallado = True
                                    '$comprobar si es gen�rico
                                ElseIf sPalabra = .ID & "me" Then
                                    sPalabra = .ID & "te"
                                    hallado = True
                                ElseIf sPalabra = .ID & "te" Then
                                    sPalabra = .ID & "me"
                                    hallado = True
                                End If
                            End If '---terminaci�n

                            'Si se ha encontrado, salir del bucle
                            If hallado Then
                                Exit For
                            End If
                        End If '---si est� contenido en la palabra
                    End With
                Next
            End If
        End If

        'devolver la nueva palabra o la original
        'ComprobarVerbos = sPalabra
        Return sPalabra
    End Function

    Private Function BuscarReglas(sPalabra As String, tRegla As cRegla) As String
        ' Busca una palabra en tRegla,
        ' devolver� la respuesta hallada o una cadena vac�a

        Dim sRespuesta As String
        Dim i As Integer
        Dim tRespuestas As cRespuestas

        ' buscar esta palabra clave en la lista
        sRespuesta = ""

        ' Primero buscar en las respuestas Extras, si hay algunas
        For Each tRespuestas In tRegla.Extras.Valores '.Values
            If tRespuestas.Contenido = sPalabra Then
                '                                                       (09/Jun/98)
                ' Si la respuesta se obtiene de forma aleatoria,
                ' s�lo se har� para buscar la primera,
                ' despu�s se continuar� secuencialmente.

                i = tRespuestas.UltimoItem + 1
                If tRegla.Aleatorio Then
                    If tRespuestas.UltimoItem < 0 Then
                        'i = Int(Rnd() * tRespuestas.Count) + 2
                        i = m_rnd.Next(tRespuestas.Count) '+ 2
                    End If
                End If
                tRespuestas.UltimoItem = i
                ' Si el siguiente item a usar es mayor que el total
                ' de respuestas disponibles
                If i >= tRespuestas.Count Then
                    ' usar la primera respuesta y reiniciar
                    ' el n�mero del item a usar la pr�xima vez
                    i = 0 '1
                    tRespuestas.UltimoItem = 0
                End If
                sRespuesta = tRespuestas.Item(i).Contenido
            End If
        Next
        ' Si no se ha encontrado una respuesta en los Extras
        ' se comprueba si esa clave es el contenido de esta Regla
        If sRespuesta.Length = 0 Then
            If tRegla.Contenido = sPalabra Then
                If tRegla.Respuestas.Count > 0 Then
                    i = tRegla.Respuestas.UltimoItem + 1
                    tRegla.Respuestas.UltimoItem = i
                    If i >= tRegla.Respuestas.Count Then
                        i = 0
                        tRegla.Respuestas.UltimoItem = 0
                    End If
                    sRespuesta = tRegla.Respuestas.Item(i).Contenido
                End If
            End If
        End If
        ' Devolver la respuesta hallada
        Return sRespuesta
    End Function

    Private Shared Function QuitarEspaciosExtras(sCadena As String) As String
        ' Quita los espacios extras dentro de la cadena                 ( 5/Jun/98)
        'Dim i As Integer

        'sCadena = sCadena.Trim()
        'Do
        '    i = sCadena.IndexOf("  ")
        '    If i > -1 Then
        '        'sCadena = Left$(sCadena, i - 1) & " " & LTrim$(Mid$(sCadena, i + 2))
        '        'sCadena = sCadena.Substring(0, i - 1) & " " & sCadena.Substring(i + 2).Trimstart()
        '        sCadena = sCadena.Substring(0, i - 1) & " " & sCadena.Substring(i + 2).TrimStart()
        '    End If
        'Loop While i > -1
        'Return sCadena

        Return sCadena.Replace("  ", " ")
    End Function

    Private Sub Entrada2Array(sEntrada As String)
        ' Esta funci�n convierte el string de entrada en un array
        ' Aqu� se comprobar� si se escribe Mi o Mis y se agregar�
        ' a la colecci�n de palabras a recordar

        Dim sPalabra As String
        Dim sSeparador As String = ""
        Dim RecordarFrase As Integer
        Dim sEntradaOrig As String

        ' Iniciar los valores del n�mero de palabras y la actual
        PalabrasOrig = 0
        PalabraOrig = 0
        sEntradaOrig = sEntrada

        Do While sEntrada.Length > 0
            sPalabra = SiguientePalabra(sEntrada, sSeparador)
            If sPalabra.Length > 0 Then
                PalabrasOrig += 1
                ReDim Preserve FraseOrig(PalabrasOrig)
                FraseOrig(PalabrasOrig) = sPalabra
                If sPalabra = "mi" OrElse sPalabra = "mis" Then
                    If RecordarFrase = 0 Then
                        RecordarFrase = PalabrasOrig
                    End If
                End If
            End If
        Loop
        ' si ha mencionado mi o mis
        If RecordarFrase > 0 Then
            ' s�lo si no es la �ltima palabra
            If RecordarFrase < PalabrasOrig Then
                ' A�adirla a la colecci�n o sustituirla por la nueva entrada
                ' S�lo se usar� la �ltima letra en caso de que sea S
                ' Para despu�s usar tu(s) xxx
                If RightN(FraseOrig(RecordarFrase), 1) = "s" Then
                    sSeparador = "s "
                Else
                    sSeparador = " "
                End If

                '$Por hacer                                 (19/Jun/98)
                ' Habr�a que comprobar que la siguiente palabra no sea
                ' algo a lo que el usuario ha hecho referencia de lo
                ' que Eliza dijera, por ejemplo:
                ' a "mi tus" problemas no me interesan... etc.
                ' Por tanto creo que se deber�a comprobar si es una
                ' de las palabras clave para que as� se pueda dirigir
                ' mejor el di�logo.
                'FraseOrig(RecordarFrase + 1)
                m_colRec.Item(sSeparador & FraseOrig(RecordarFrase + 1)).Contenido = sEntradaOrig
            End If
        End If
    End Sub

    Public Sub Releer()
        m_Releer = True
        Inicializar()
    End Sub

    Public Function Estad�sticas() As cRespuestas
        'Devolver� una colecci�n con los siguientes datos:
        '(realmente devuelve la cantidad)
        '   Palabras usadas para sustituci�n
        '   Verbos
        '   Palabras claves
        '   Sub-Claves (variantes de las claves)
        '   Respuestas en palabras claves
        '   Respuestas en Sub-Claves

        Dim colRespuestas As New cRespuestas()
        Dim i As Integer = 0, j As Integer = 0, k As Integer = 0
        Dim nMayor As Integer = 0
        Dim sMayor As String = ""

        For Each tRegla In m_colReglas.Values
            'sub-claves (extras)
            i += tRegla.Extras.Count
            'respuestas
            If tRegla.Respuestas.Count > nMayor Then
                nMayor = tRegla.Respuestas.Count
                sMayor = tRegla.Contenido
            End If
            j += tRegla.Respuestas.Count
            For Each tRespuestas In tRegla.Extras.Valores '.Values
                'Respuestas en las sub-claves
                k += tRespuestas.Count
            Next
        Next

        With colRespuestas
            .Item("Palabras usadas para simplificar").Contenido = m_colRS.Count.ToString()
            .Item("Palabras usadas para sustituci�n").Contenido = m_colSimp.Count.ToString()
            .Item("Verbos").Contenido = m_Verbos.Count.ToString()
            .Item("Palabras claves").Contenido = m_colReglas.Count.ToString()
            .Item("Sub-Claves (variantes de las claves)").Contenido = i.ToString()
            .Item("-----").Contenido = "-----"
            .Item("N�mero total de palabras reconocidas").Contenido = (m_colSimp.Count + m_colRS.Count + m_Verbos.Count + m_colReglas.Count + i).ToString()
            .Item("------").Contenido = "-----"
            .Item("Respuestas en palabras claves").Contenido = j.ToString()
            .Item("Respuestas en Sub-Claves").Contenido = k.ToString()
            .Item("--------").Contenido = "-----"
            .Item("N�mero total de Respuestas").Contenido = (k + j).ToString()
            .Item("---------").Contenido = "-----"
            .Item("Clave principal con m�s respuestas").Contenido = "'" & sMayor & "'"
            .Item("N�mero de respuestas de '" & sMayor & "'").Contenido = nMayor.ToString()
        End With
        Return colRespuestas
    End Function

    Private Function BuscarEsaClave(sPalabra As String) As String
        ' Comprobar si sPalabra es una clave, si es as�,
        ' devolver la siguiente respuesta
        Dim tRegla As cRegla
        Dim sRespuesta As String = ""
        Dim i As Integer

        ' Comprobar primero si est� en las reglas de simplificaci�n

        If m_colRS.ExisteItem(sPalabra) Then
            sRespuesta = m_colRS.Item(sPalabra).Contenido
            ' si se ha encontrado, el contenido ser� lo que est�
            ' en la lista de palabras clave
            If sRespuesta.Length > 0 Then
                sPalabra = sRespuesta
            End If
        End If
        ' Buscar esta palabra clave en la lista
        For Each tRegla In m_colReglas.Values
            ' buscar la palabra en cada una de las "claves" y subclaves
            sRespuesta = BuscarReglas(sPalabra, tRegla)
            If sRespuesta.Length > 0 Then
                Exit For
            End If
        Next

        ' Si es una clave especial, no tener en cuenta el *equal:=
        If sRespuesta.IndexOf("{*iif", StringComparison.OrdinalIgnoreCase) > -1 Then
            Return sRespuesta
        End If
        ' Si se usa {*base:=...}
        If sRespuesta.IndexOf("{*base:=", StringComparison.OrdinalIgnoreCase) > -1 Then
            Return sRespuesta
        End If

        ' Si el contenido de sRespuesta es:*equal:=xxx
        ' quiere decir que se debe buscar en la clave "xxx"

        i = sRespuesta.IndexOf("*equal:=", StringComparison.OrdinalIgnoreCase)
        ' Como ahora se pueden tener respuestas que incluyan *equal:=
        ' se debe buscar respuesta s�lo si esta "clave" est� al
        ' principio de la respuesta                                     (13/Jun/98)
        'Do While i = 0 'i = 1
        Do While sRespuesta.StartsWith("*equal:=", StringComparison.OrdinalIgnoreCase) 'i = 0 'i = 1
            'sPalabra = MidN(sRespuesta, i + 8).TrimStart()
            sPalabra = MidN(sRespuesta, 8).TrimStart()
            'tRegla = m_col(sPalabra)
            If m_colReglas.ContainsKey(sPalabra) Then
                tRegla = m_colReglas(sPalabra)
                sRespuesta = BuscarReglas(sPalabra, tRegla)
            Else
                ' No existe como clave principal,
                ' hay que buscarlo en las sub-claves
                For Each tRegla In m_colReglas.Values
                    sRespuesta = BuscarReglas(sPalabra, tRegla)
                    If sRespuesta.Length > 0 Then
                        Exit For
                    End If
                Next

            End If
            'i = sRespuesta.IndexOf("*equal:=", StringComparison.OrdinalIgnoreCase)
            ' repetir mientras en sRespuesta empiece por *equal:=
        Loop
        Return sRespuesta
    End Function

    Private Function CrearRespuestaRecordando(sPalabra As String) As String
        Dim tRegla As cRegla
        Dim i As Integer, j As Integer
        Dim sRespuesta As String
        Static UsarEstaRespuesta As Integer
        '$Para probar usar el valor de las respuestas que tenemos,
        'en casos normales usar un valor mayor
        Const NUM_RESPUESTAS As Integer = 10

        sRespuesta = ""
        If m_colReglas.ContainsKey(sPalabra) Then
            tRegla = m_colReglas(sPalabra)
        Else
            Return sRespuesta
        End If
        'tRegla = m_col(sPalabra)
        ' s�lo si son dos o m�s cosas que no ha entendido
        If tRegla.Respuestas.UltimoItem > 2 Then
            If m_colRec.Count > 0 Then
                ' tomar aleatoriamente una de las cosas que
                ' ha dicho el usuario
                j = 0
                Do
                    j += 1
                    'i = Int(Rnd() * m_colRec.Count) + 1
                    i = m_rnd.Next(m_colRec.Count) '+ 1
                    ' S�lo usar este tema si se ha mencionado antes
                    If m_colRec.UltimoItem <> i Then
                        Exit Do
                    End If
                    ' Si s�lo hay un dato en la colecci�n...
                    ' no quedarse en un bucle sin fin...
                Loop Until j > NUM_RESPUESTAS '10

                m_colRec.UltimoItem = i
                UsarEstaRespuesta += 1
                If UsarEstaRespuesta > NUM_RESPUESTAS Then
                    UsarEstaRespuesta = 1
                End If
                ' El valor aleatorio sacado indicar� si se usa o no
                ' una respuesta "del recuerdo"
                ' Esto es para que no se repitan las respuestas en el
                ' mismo orden...
                j = 0
                ' le damos m�s "peso" al uso de este tipo de respuestas
                ' de Eliza para "aparentar" m�s inteligencia
                'If Int(Rnd() * 10) > 3 Then
                If m_rnd.Next(10) > 3 Then
                    j = UsarEstaRespuesta
                End If
                ' De estos valores s�lo se tendr�n en cuenta los aqu�
                ' indicados, en caso contrario,
                ' usar una de las respuestas "predefinidas",
                ' para ello se asigna una cadena vacia a sRespuesta
                Select Case j
                    Case 1
                        sRespuesta = "Antes mencionaste tu" & m_colRec.Item(i).ID & ", hablame m�s de ello " & LeftN(m_colRec.Item(i).ID, 1).Trim() & "."
                    Case 2
                        sRespuesta = "Me comentabas sobre tu" & m_colRec.Item(i).ID & ", �c�mo te influye?"
                    Case 3
                        sRespuesta = "�Crees que la relaci�n con tu" & m_colRec.Item(i).ID & " es el motivo de tu problema?"
                    Case 4
                        sRespuesta = "�C�mo crees que " & SimplificarEntrada(m_colRec.Item(i).Contenido) & " podr�a influir en tu comportamiento?"
                    Case 5
                        sRespuesta = "H�blame m�s de tus relaciones con tu" & m_colRec.Item(i).ID
                    Case Else
                        sRespuesta = ""
                End Select
            End If
        End If
        Return sRespuesta
    End Function

    Private Shared Function EsNegativoPositivo(sEntrada As String, esNegativo As Boolean) As Boolean
        'comprobar si en la cadena de entrada hay alguna palabra
        'que denote negaci�n o afirmaci�n

        Dim palabrasNegPos As String()
        If esNegativo Then
            palabrasNegPos = {" no ", " no,", " nope", " nop", " nil", " negativo", " falso", " nada", " ya est", " ya vale"}
        Else
            palabrasNegPos = {" s� ", " si ", " s�,", " si,", " yep", " afirmativo", " efectivamente", " as� es", " asi es", " por supuesto", " ciertamente", " eso es", " vale", " ok", " o.k.", " de acuerdo", " muy bien", " ya que insistes", " claro"}
        End If

        sEntrada = " " & sEntrada & " "
        For i = 0 To palabrasNegPos.Length - 1
            If sEntrada.IndexOf(palabrasNegPos(i), StringComparison.OrdinalIgnoreCase) > -1 Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function EsNegativo(sEntrada As String) As Boolean
        'comprobar si en la cadena de entrada hay alguna palabra
        'que denote negaci�n

        Return EsNegativoPositivo(sEntrada, esNegativo:=True)

        ''Dim i As Integer
        'sEntrada = " " & sEntrada & " "
        ''Crear un array con palabras "negativas"
        'Dim aNegativo = {" no ", " no,", " nope", " nil", " negativo", " falso", " nada", " ya est", " ya vale"}
        'For i = 0 To aNegativo.Length - 1
        '    If sEntrada.IndexOf(aNegativo(i), StringComparison.OrdinalIgnoreCase) > -1 Then
        '        Return True
        '    End If
        'Next
        'Return False
    End Function

    Private Shared Function EsAfirmativo(sEntrada As String) As Boolean
        'comprobar si en la cadena de entrada hay alguna palabra
        'que denote afirmaci�n
        Return EsNegativoPositivo(sEntrada, esNegativo:=False)

        'Dim i As Integer
        'sEntrada = " " & sEntrada & " "
        'Dim aAfirmativo = {" s� ", " si ", " s�,", " si,", " afirmativo", " efectivamente", " as� es", " asi es", " por supuesto", " ciertamente", " eso es", " vale", " ok", " o.k.", " de acuerdo", " muy bien", " ya que insistes", " claro"}
        'For i = 0 To aAfirmativo.Length - 1
        '    If sEntrada.IndexOf(aAfirmativo(i), StringComparison.OrdinalIgnoreCase) > -1 Then
        '        Return True
        '    End If
        'Next
        'Return False
    End Function

    Private Function ComprobarEspeciales(sRespuesta As String, sEntrada As String, sPalabra As String) As String
        'Comprobar las claves especiales de sustituci�n y otras
        'que puedan estar en la respuesta generada          (13/Jun/98)
        Dim i, j As Integer
        Static restoAnt As String

        'comprueba si la respuesta contiene caracteres especiales
        If String.IsNullOrEmpty(sRespuesta) = False Then
            'Comprobar si hay que poner la hora
            ' si se indica *LA_HORA* solo poner la hora         (25/ene/23 14.40)
            If sRespuesta.Contains("*LA_HORA*", StringComparison.OrdinalIgnoreCase) Then
                'sRespuesta = Left$(sRespuesta, i - 1) & Format$(Now, "hh:mm") & Mid$(sRespuesta, i + Len("*HORA*"))
                sRespuesta = sRespuesta.Replace("*LA_HORA*", Date.Now.ToString("H"), StringComparison.OrdinalIgnoreCase)
            End If

            'i = sRespuesta.IndexOf("*HORA*", StringComparison.OrdinalIgnoreCase)
            'If i > -1 Then
            If sRespuesta.Contains("*HORA*", StringComparison.OrdinalIgnoreCase) Then
                'sRespuesta = Left$(sRespuesta, i - 1) & Format$(Now, "hh:mm") & Mid$(sRespuesta, i + Len("*HORA*"))
                sRespuesta = sRespuesta.Replace("*HORA*", Date.Now.ToString("HH:mm"), StringComparison.OrdinalIgnoreCase)
            End If
            'Comprobar si hay que poner el d�a de hoy
            'i = sRespuesta.IndexOf("*HOY*", StringComparison.OrdinalIgnoreCase)
            'If i > -1 Then
            If sRespuesta.Contains("*HOY*", StringComparison.OrdinalIgnoreCase) Then
                'sRespuesta = Left$(sRespuesta, i - 1) & Format$(Now, "dddd, dd") & " de " & StrConv(Format$(Now, "mmmm"), vbProperCase) & Mid$(sRespuesta, i + Len("*HOY*"))
                sRespuesta = sRespuesta.Replace("*HOY*", Date.Now.ToString("dddd, dd MMMM"), StringComparison.OrdinalIgnoreCase)
            End If

            'comprobar si hay que a�adir el RESTO
            If sRespuesta.IndexOf("*RESTO*", StringComparison.OrdinalIgnoreCase) > -1 Then
                i = sEntrada.IndexOf(sPalabra, StringComparison.OrdinalIgnoreCase)
                If i > -1 Then
                    sEntrada = MidN(sEntrada, i + sPalabra.Length + 1)
                End If
                ' Sustituir *RESTO* por sEntrada
                i = sRespuesta.IndexOf("*RESTO*", StringComparison.OrdinalIgnoreCase)
                If i > -1 Then
                    sEntrada = SimplificarEntrada(sEntrada)
                    ' Guardar la respuesta anterior,                    (17/Sep/02)
                    ' por si se usa pra asignar a la base de datos del usuario.
                    restoAnt = sEntrada.Trim()
                    sRespuesta = sRespuesta.Substring(0, i - 1) & sEntrada & sRespuesta.Substring(i + "*RESTO*".Length)
                End If
            End If
        Else
            sRespuesta = sEntrada
        End If
        'Cambiar los *ea* por el correspondiente seg�n el sexo
        'If sRespuesta.IndexOf("*ea*", StringComparison.OrdinalIgnoreCase) > -1 Then
        If sRespuesta.Contains("*ea*", StringComparison.OrdinalIgnoreCase) Then
            sPalabra = "e"
            If m_Sexo = eSexo.Femenino Then
                sPalabra = "a"
            End If
            'Cambiar las posibles ocurrencias de *ea* por sPalabra
            sRespuesta = QuitarCaracterEx(sRespuesta, "*ea*", sPalabra)
        End If
        'Cambiar los *oa* por el correspondiente seg�n el sexo
        'If sRespuesta.IndexOf("*oa*", StringComparison.OrdinalIgnoreCase) > -1 Then
        If sRespuesta.Contains("*oa*", StringComparison.OrdinalIgnoreCase) Then
            sPalabra = "o"
            If m_Sexo = eSexo.Femenino Then
                sPalabra = "a"
            End If
            'Cambiar las posibles ocurrencias de *oa* por sPalabra
            sRespuesta = QuitarCaracterEx(sRespuesta, "*oa*", sPalabra)
        End If

        'Si el primer caracter es una �, el �ltimo debe ser ?
        If sRespuesta.StartsWith("�") Then
            If sRespuesta.Contains("?"c) = False Then
                sRespuesta &= "?"
            End If
        End If
        'Si existen dos caracteres iguales al final, dejar s�lo uno
        's�lo si no es el punto, por aquello de los ...     (11/Jun/98)
        If sRespuesta.EndsWith(".") = False Then
            i = sRespuesta.Length
            Dim comoTermina = RightN(sRespuesta, 1)
            If sRespuesta.EndsWith(comoTermina & comoTermina) Then
                sRespuesta = sRespuesta.Substring(0, i - 1)
            End If
            'If sRespuesta.Substring(i - 1, 1) = RightN(sRespuesta, 1) Then
            '    sRespuesta = sRespuesta.Substring(0, i - 1)
            'End If
        End If

        ' si se indica *mi_edad*, calcular la edad                      (18/Sep/02)
        'i = sRespuesta.IndexOf("*mi_edad*", StringComparison.OrdinalIgnoreCase)
        'If i > -1 Then
        If sRespuesta.Contains("*mi_edad*", StringComparison.OrdinalIgnoreCase) Then
            'sRespuesta = sRespuesta.Substring(0, i - 1) & (Date.Now.Year - 1998).ToString() & sRespuesta.Substring(i + 9)
            sRespuesta = sRespuesta.Replace("*mi_edad*", (Date.Now.Year - 1998).ToString(), StringComparison.OrdinalIgnoreCase)
        End If

        'Cambiar *NOMBRE* por el nombre
        i = sRespuesta.IndexOf("*NOMBRE*", StringComparison.OrdinalIgnoreCase)
        'If i > -1 Then
        If sRespuesta.Contains("*NOMBRE*", StringComparison.OrdinalIgnoreCase) Then
            'Usar siempre el nombre
            'sRespuesta = Left$(sRespuesta, i - 1) & Nombre & Mid$(sRespuesta, i + Len("*NOMBRE*"))
            sRespuesta = sRespuesta.Replace("*NOMBRE*", Nombre, StringComparison.OrdinalIgnoreCase)
        End If

        '$Comprobar si despu�s de un sigo de separaci�n no hay espacio
        'Lo que se hace es a�adirle el espacio, ya que posteriormente
        'se le quitar�an los espacios extras
        sRespuesta = QuitarCaracterEx(sRespuesta, ",", ", ")
        'm�s arreglos del texto
        sRespuesta = QuitarCaracterEx(sRespuesta, " , ", ", ")

        'quitarle los dobles espacios que haya
        sRespuesta = QuitarEspaciosExtras(sRespuesta)
        'quitar los espacios de delante de la interrogaci�n final
        sRespuesta = QuitarCaracterEx(sRespuesta, " ?", "?")
        'quitar los espacios de despu�s de la interrogaci�n inicial
        sRespuesta = QuitarCaracterEx(sRespuesta, "� ", "�")

        'Si la respuesta contiene {*iif(
        i = sRespuesta.IndexOf("{*iif(", StringComparison.OrdinalIgnoreCase)
        sUsarPregunta = ""
        If i > -1 Then
            'El formato ser�: {*iif(condici�n; ES-TRUE)(ES-FALSE)}
            sUsarPregunta = sRespuesta.Substring(i)
            sRespuesta = sRespuesta.Substring(0, i - 1)
            j = sUsarPregunta.IndexOf(";")
            If j > -1 Then
                i = sUsarPregunta.IndexOf("(", j)
                If i > -1 Then
                    sRespuestas(cNegativa) = sUsarPregunta.Substring(i + 1, sUsarPregunta.Length - i - 2)
                    sRespuestas(cAfirmativa) = sUsarPregunta.Substring(j + 1, i - j - 2)
                    sUsarPregunta = sUsarPregunta.Substring(6, j - 6)
                End If
            End If
        End If
        'Si la respuesta contiene {*base:=
        sUsarBaseDatos = ""
        i = sRespuesta.IndexOf("{*base:=", StringComparison.OrdinalIgnoreCase)
        If i > -1 Then
            'El formato ser�: {*base:=clave_base}
            'sUsarBaseDatos contendr� la clave de la base de datos
            sUsarBaseDatos = MidN(sRespuesta, i + 8)
            sRespuesta = LeftN(sRespuesta, i - 1)
            'Quitarle el } del final
            i = sUsarBaseDatos.IndexOf("}")
            If i > -1 Then
                ' si a continuaci�n sigue un := asignar el valor indicado
                j = sUsarBaseDatos.IndexOf(":=*restoant*", StringComparison.OrdinalIgnoreCase)
                If j > -1 Then
                    sUsarBaseDatos = LeftN(sUsarBaseDatos, j - 1)
                    ValidarDatosParaBase(restoAnt)
                Else
                    sUsarBaseDatos = LeftN(sUsarBaseDatos, i - 1)
                End If
            End If
        End If
        'Si la respuesta incluye: *iif(*base*
        'se comprobar� si el dato est� en la base de datos,
        'de se as� se usar� lo que venga despu�s del ;
        'en caso contrario se usar� lo que est� despu�s de )(
        '*iif(*base*signo_zodiaco;*usarbase:=signo_zodiaco*)
        '                         (*equal:=cual es tu signo)
        i = sRespuesta.IndexOf("*iif(*base*", StringComparison.OrdinalIgnoreCase)
        If i > -1 Then
            Dim sClave As String
            Dim sTrue As String = ""
            Dim sFalse As String = ""

            sEntrada = LeftN(sRespuesta, i - 1)
            sClave = MidN(sRespuesta, i + 11)
            j = sClave.IndexOf(";")
            If j > -1 Then
                sClave = LeftN(sClave, j - 1)
                j = InStr(sRespuesta, ";")
                sRespuesta = MidN(sRespuesta, j + 1)
                j = sRespuesta.IndexOf(")(")
                If j > -1 Then
                    sTrue = LeftN(sRespuesta, j - 1)
                    sFalse = MidN(sRespuesta, j + 2)
                    If RightN(sFalse, 1) = ")" Then
                        sFalse = LeftN(sFalse, sFalse.Length - 1)
                    End If
                End If
                If BaseUser.ExisteItem(sClave) Then
                    'Comprobar si hay que sustituir el dato
                    '*usarbase:=signo_zodiaco*
                    'Si despu�s de la clave se
                    i = sTrue.IndexOf("*usarbase:=", StringComparison.OrdinalIgnoreCase)
                    If i > -1 Then
                        'j = InStr(i + 1, sTrue, "*")
                        j = sTrue.IndexOf("*", i + 1)
                        sClave = MidN(sTrue, i + "*usarbase:=".Length, j - (i + "*usarbase:=".Length))
                        If BaseUser.ExisteItem(sClave) Then
                            sTrue = LeftN(sTrue, i - 1) & " " & BaseUser.Item(sClave).Contenido & " " & MidN(sTrue, j + 1)
                        Else
                            sTrue = LeftN(sTrue, i - 1) & " " & MidN(sTrue, j + 1)
                        End If
                    End If
                    sRespuesta = sTrue
                Else
                    sRespuesta = sFalse
                End If
                sRespuesta = sEntrada & sRespuesta
                i = sRespuesta.IndexOf("*equal:=", StringComparison.OrdinalIgnoreCase)
                If i > -1 Then
                    sRespuesta = MidN(sRespuesta, i + 8).Trim()
                    sRespuesta = BuscarEsaClave(sRespuesta)
                    're-entrar para comprobar nuevas claves
                    If sRespuesta.Length > 0 Then
                        sRespuesta = ComprobarEspeciales(sRespuesta, sRespuesta, "")
                    End If
                End If
            End If
        End If
        Return sRespuesta
    End Function

    Public Property Iniciado As Boolean
    '    Get
    '        Return m_Iniciado
    '    End Get
    '    Set(value As Boolean)
    '        m_Iniciado = value
    '    End Set
    'End Property

    Private Sub DatosUsuario(Optional AccionLeer As Boolean = True)
        'Leer� la base de datos de este usuario.            (14/Jun/98)
        'El formato del fichero ser�:
        '   clave=valor
        Dim sFic As String
        Dim sTmp As String
        Dim i As Integer
        Dim sClave As String
        Dim sPath As String

        sPath = System.IO.Path.Combine(AppPath(), "Bases")
        If System.IO.Directory.Exists(sPath) = False Then
            System.IO.Directory.CreateDirectory(sPath)
        End If
        sFic = System.IO.Path.Combine(sPath, "Datos_" & Nombre & ".txt")

        If AccionLeer Then
            BaseUser = New cRespuestas
            'Para que tenga algunos datos
            BaseUser.Item("Nombre").Contenido = Nombre
            BaseUser.Item("Sexo").Contenido = If(m_Sexo = eSexo.Femenino, "Femenino", "Masculino")
            'Leer los datos, si hay...
            If System.IO.File.Exists(sFic) Then
                Using sr As New System.IO.StreamReader(sFic, System.Text.Encoding.UTF8, True)
                    Do While Not sr.EndOfStream
                        sTmp = sr.ReadLine().Trim()
                        If String.IsNullOrEmpty(sTmp) = False AndAlso sTmp.StartsWith(";") = False Then
                            i = sTmp.IndexOf("=")
                            If i > -1 Then
                                sClave = sTmp.Substring(0, i - 1).Trim()
                                sTmp = sTmp.Substring(i + 1).Trim()
                                BaseUser.Item(sClave).Contenido = sTmp
                            End If
                        End If
                    Loop
                End Using
            End If
        Else
            'Guardar los datos
            Using sw As New System.IO.StreamWriter(sFic, False, System.Text.Encoding.UTF8)
                For Each tContenido In BaseUser.Valores '.Values
                    sw.WriteLine($"{tContenido.ID}={tContenido.Contenido}")
                Next
                'For i = 0 To BaseUser.Count - 1
                '    sw.WriteLine($"{BaseUser(i).ID}={BaseUser(i).Contenido}")
                'Next
            End Using
        End If
    End Sub

    Private Function ValidarDatosParaBase(sEntrada As String) As String
        ' Se validar� la respuesta del usuario a una pregunta para
        ' a�adir a la base de datos.
        Dim i As Integer
        Dim hallado As Boolean

        Select Case sUsarBaseDatos
            Case "signo_zodiaco"
                Dim vArray = {" aries", " g�minis", " geminis", " tauro", " c�ncer", " cancer", " leo", " virgo", " libra", " scorpio", " escorpi�n", " escorpio", " escorpion", " sagitario", " capricornio", " acuario", " piscis"}
                hallado = False
                For i = 0 To vArray.Length - 1
                    If sEntrada.IndexOf(vArray(i), StringComparison.OrdinalIgnoreCase) > -1 Then
                        BaseUser.Item(sUsarBaseDatos).Contenido = vArray(i).TrimStart()
                        hallado = True
                        Exit For
                    End If
                Next
                ' Se puede devolver esto como respuesta
                If Not hallado Then
                    sEntrada = "Creo que no has usado un signo del zod�aco..."
                End If
            Case "edad"
                'i = CInt(sEntrada)
                i = 0
                If Integer.TryParse(sEntrada, i) = False Then
                    sEntrada = "Por favor indica la edad con n�meros, gracias."
                Else
                    If i = 0 Then
                        sEntrada = "�Acabas de nacer? :-)"
                    ElseIf i < 1 Then
                        sEntrada = "�Es que a�n no has nacido? :-)"
                    End If
                End If
            Case Else
                BaseUser.Item(sUsarBaseDatos).Contenido = sEntrada
        End Select
        ' Guardar los datos
        DatosUsuario(False)

        Return sEntrada
    End Function

    Private Function AppPath() As String
        Return m_AppPath
    End Function

    Public Shared Function VersionDLL() As String
        Dim ensamblado = GetType(cEliza).Assembly
        Dim fvi = FileVersionInfo.GetVersionInfo(ensamblado.Location)
        ' FileDescription en realidad muestra (o eso parece) lo mismo de ProductName
        Dim s = $"{fvi.ProductName} v{fvi.ProductVersion} ({fvi.FileVersion})" &
            $"{vbCrLf}{fvi.Comments}"

        Return s
    End Function
End Class
