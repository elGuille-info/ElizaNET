VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cEliza"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' cEliza.cls                                                        (30/May/98)
'
' Clase para mantener colecciones del tipo cRegla
' y procesar la entrada del usuario y devolver una respuesta,
' es decir el "coco" de ELIZA...
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
'
' Revisiones, normalmente en las comprobaciones de las palabras.
' 00.05.00  Las terminaciones rte,rme sólo en los verbos conocidos
' 00.06.00  Se mantiene un array con las palabras de las frases
' 00.06.03  He quitado la opción del idioma.
' 00.07.04  Se cambia melo -> telo en los verbos conocidos
' 00.07.06  Si se especifica @ después del nivel,
'           la primera respuesta será aleatoria
' 00.07.11  Añado conjugaciones y corrijo las que estaban mal
' 00.08.00  Cambio la forma de analizar la frase.
'           Las sustituciones sólo se harán al usar *RESTO*
'           aunque esto implica el cambio de las palabras claves
' 00.09.00  Las palabras especificadas en {* se separarán por ;
'           de esta forma se puede usar más de una palabra.
'           He cambiado la forma de usar SiguientePalabra.
'-----------
' 00.09.xx  En el entorno del VB me da un GPF al terminar de forma
'           normal, esto no ocurre con el programa compilado y
'           parece ser que sólo ocurre en el 200 MMX (el de mi casa)
'           Las modificaciones hechas en las declaraciones de los
'           objetos en las clases ha sido para ver si se evitaba...
'-----------
' 00.09.07  Se permite comprobar una clave si el usuario escribe *consulta*
' 00.10.00  Eliza puede recordar algo de lo que dijo el usuario
'           Inicialmente se guardará cuando el user diga:
'           "mi xxx" o "mis xxx" y se usará cuando no entienda
' 00.11.00  Se puede usar {*iif para tener en cuenta la respuesta
'           del usuario, usando lo indicado en cada una de las
'           partes, según se cumpla o no.
'           {*iff(*true*|*false*;PARTE-VERDADERA)(PARTE-FALSA)}
' 00.12.00  Empiezo a usar la base de datos del usuario.
'           Para almacenar datos se hará de esta forma:
'           Pregunta{*base:=clave_BaseDatos}
' 00.12.10  Cambio la forma de guardar las subclaves cuando se usan
'           {* xxx; yyy} de forma que el *equal:= haga referencia
'           directa a la clave superior.
'           ===LO QUITO===
' 00.13.00  Añado un form para consulta de claves y respuestas
'
' 00.14.00  Con la opción de "decir" (o hablar) lo que escribe...
' 00.15.00  Añado opción de 'hablar' con el Direct Speech Synthesis Control (DirectSS)
' 00.16.00  Quito el DirectSS
'           Cambio la forma de entrada/salida por "simples" TextBoxes
' 00.17.00  Se permiten varios ficheros de palabras, el nombre será ElizaVB_xxx.txt
'           El primero que se leerá será Eliza_SPA.txt, de esta forma, se pueden
'           aplicar nuevas reglas que sustituyan a las definidas en Eliza_SPA.txt
'
' POR HACER: (16/Sep/02)
'           Si una palabra va acentuada, se puede indicar de esta forma:
'           "que'" para indicar que se usarán las dos formas: qué y que
'           Permitir en las palabras claves el uso de *oa* para crear las
'           dos formas: masculina y femenina.
'------------------------------------------------------------------------------
Option Explicit
Option Compare Text

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
Private BaseUser As cRespuestas
Private sUsarBaseDatos As String

' Para interactuar con el usuario, se usará un array cuando se le
' haga una pregunta a la que deberá responder con dos tipos de
' respuesta, normalmente una positiva y otra negativa
'Const cAfirmativa As Long = -1
'Const cNegativa As Long = 0
'Private sRespuestas(cAfirmativa To cNegativa) As String
Const cAfirmativa As Long = 1
Const cNegativa As Long = 0
'Private sRespuestas(cNegativa To cAfirmativa) As String
' de cero a uno
Private sRespuestas(cAfirmativa) As String
' Esta variable servirá de 'flag' y contendrá la condición que
' usará Eliza para evaluar una "respuesta" que contenga "{*iif("
Private sUsarPregunta As String

Private m_Iniciado As Boolean
Private m_Releer As Boolean

' Para la revisión 00.06.00
' Estos arrays tendrán cada palabra de la frase original
Private FraseOrig() As String
' y la convertida
'Private FraseConv() As String
' el número de la palabra actual
Private PalabraOrig As Long 'Integer
'Private PalabraConv As Integer
' aunque el número de palabras se puede conseguir con UBound(array)
' es conveniente mantener una variable, entre otras cosas para saber
' cuál es la que se añade
Private PalabrasOrig As Long 'Integer
'Private PalabrasConv As Integer

Public Nombre As String

' NOTA:
'   Cuando se usa un mismo Nombre en distintas enumeraciones
'   hay que tener la precaución de asignarle el mismo valor
'   si no se hiciera, al compilarlo puede que no de el resultado
'   deseado, a pesar de que en el IDE funcione correctamente.
'   Siempre se puede "prevenir" este fallillo anteponiendo a ese
'   nombre el nombre de la enumeración,
'   por ejemplo, si hubiera que distinguir entre los dos Ninguno:
'   eSexo.Ninguno o eIdioma.Ninguno
'
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
'    'sólo se deben admitir los valores indicados
'    'de no ser así se asignará Spanish
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

' Colección para recordar lo que ha dicho el usuario                (12/Jun/98)
Private m_colRec As cRespuestas
' colección para los verbos
Private m_Verbos As cRespuestas
' Colección para las palabras de las reglas de simplificación
' Estas serán las que se usarán en SustituirEnEntrada
Private m_colRS As cRespuestas
' Estas serán las que se usarán en SimplificarEntrada
Private m_colSimp As cRespuestas

' Colección de Reglas
Private m_col As Collection
Private sSeparadores As String

Public Function Reglas(ByVal newRegla As Variant) As cRegla
Attribute Reglas.VB_UserMemId = 0
    Dim tRegla As cRegla
    '
    On Local Error Resume Next
    '
    Set Reglas = m_col.Item(newRegla)
    If Err > 0 Then
        ' Si no existe añadirlo
        Set tRegla = New cRegla
        tRegla.Contenido = newRegla
        m_col.Add tRegla, newRegla
        Set Reglas = tRegla
    End If
    '
    Err = 0
End Function

Private Sub Class_Initialize()
    Set m_col = New Collection
    Set m_colSimp = New cRespuestas
    Set m_colRS = New cRespuestas
    Set m_Verbos = New cRespuestas
    Set m_colRec = New cRespuestas
    Set BaseUser = New cRespuestas
    sSeparadores = " .,;:¿?¡!()[]/-" & Chr$(9) & Chr$(34) & vbCr & vbLf
    'm_Idioma = Spanish
    m_Sexo = Masculino
    m_Iniciado = False
    m_Releer = False
End Sub

Private Sub Class_Terminate()
    ' Destruir los objetos manetenidos por esta clase
    Set BaseUser = Nothing
    Set m_colRec = Nothing
    Set m_Verbos = Nothing
    Set m_colRS = Nothing
    Set m_colSimp = Nothing
    Set m_col = Nothing
End Sub

Public Function ProcesarEntrada(ByVal sEntrada As String) As String
    ' Se procesará la entrada del usuario y devolverá la cadena con la respuesta
    ' A este procedimiento se llamará desde el formulario,
    ' después de que el usuario escriba.
    '
    Dim sEntradaSimp As String
    Dim sClaves As String
    Dim sCopiaEntrada As String
    Static ModoConsulta As Boolean
    '
    Dim i As Long
    Dim sPalabra As String
    Dim tRegla As cRegla
    '
    ' Convertirla en minúscula
    ' (no es necesario ya que se usa Option Compare Text)
    'sEntrada = LCase(sEntrada)
    '
    ' Si se escribe *consulta* usar lo que viene a continuación
    ' para buscar en las claves y subclaves
    '
    sEntrada = Trim$(sEntrada)
    '
    ' Quitar los dobles espacios que haya
    sEntrada = QuitarEspaciosExtras(sEntrada)
    '
    If Left$(sEntrada, 10) = "*consulta*" Then
        ModoConsulta = Not ModoConsulta
        If ModoConsulta Then
            sEntradaSimp = "Escribe la clave o sub-clave a comprobar, para terminar de consultar, escribe nuevamente *consulta*"
        Else
            sEntradaSimp = "Salimos del modo consulta, ya puedes continuar normalmente."
        End If
        ProcesarEntrada = sEntradaSimp
        Exit Function
    End If
    If ModoConsulta Then
        ' buscar la clave y mostrar si existe o no
        ' si existe, mostrar la siguiente respuesta
        sClaves = BuscarEsaClave(sEntrada)
        If Len(sClaves) Then
            ProcesarEntrada = "Respuesta: " & sClaves
        Else
            ProcesarEntrada = "No existe '" & sEntrada & "' en las claves y sub-claves"
        End If
        Exit Function
    End If
    '
    ' Quitar los signos de separación del principio
    Do While InStr(sSeparadores, Left$(sEntrada, 1))
        sEntrada = Mid$(sEntrada, 2)
    Loop
    ' Quitar también los del final                                  (16/Sep/02)
    Do While InStr(sSeparadores, Right$(sEntrada, 1))
        sEntrada = Left$(sEntrada, Len(sEntrada) - 1)
    Loop
    '
    ' Si hay que usar una respuesta, procesar aquí para el caso de
    ' que esa respuesta sea sí o no...
    If Len(sUsarPregunta) Then
        ' Se puede usar *afirmativo*, *true*
        If InStr(sUsarPregunta, "*afirmativo*") > 0 Or InStr(sUsarPregunta, "*true*") > 0 Then
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
            ' procesar la comparación
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
        ' de ser así, buscar la respuesta adecuada,
        ' en otro caso esa es la respuesta
        i = InStr(sUsarPregunta, "*equal:=")
        Do While i = 1
            sPalabra = LTrim$(Mid$(sUsarPregunta, i + 8))
            On Local Error Resume Next
            Set tRegla = m_col(sPalabra)
            If Err = 0 Then
                sUsarPregunta = BuscarReglas(sPalabra, tRegla)
            Else
                Err = 0
                ' No existe como clave principal,
                ' hay que buscarlo en las sub-claves
                For Each tRegla In m_col
                    sUsarPregunta = BuscarReglas(sPalabra, tRegla)
                    If Len(sUsarPregunta) > 0 Then
                        Exit For
                    End If
                Next
                '
            End If
            i = InStr(sUsarPregunta, "*equal:=")
        ' repetir mientras en sRespuesta hay un *equal:=
        Loop
        'ProcesarEntrada = sUsarPregunta
        i = 0
        If InStr(sUsarPregunta, "{*iif") Then
            i = 1
        End If
        ' Si el usuario no ha contestado con lo esperado
        If sUsarPregunta <> sEntrada Then
            ProcesarEntrada = ComprobarEspeciales(sUsarPregunta, sEntrada, sPalabra)
            If i = 0 Then
                sUsarPregunta = ""
            End If
            Exit Function
        End If
    End If
    '
    If Len(sUsarBaseDatos) Then
        ' antes de almacenar los datos, se debería chequear
        ' para que el usuario no nos de 'datos erróneos'
        ' Por ejemplo si se le pregunta el signo del zodiaco
        ' que no nos diga cualquier cosa...
        sCopiaEntrada = sUsarBaseDatos
        sUsarBaseDatos = ValidarDatosParaBase(sEntrada)
        ' Si el valor es diferente es que se ha modificado,
        ' para indicarle algo al usuario
        If sUsarBaseDatos <> sEntrada Then
            ProcesarEntrada = sUsarBaseDatos
            Exit Function
        End If
        sUsarBaseDatos = ""
        ' Aquí debería salir, para que no diga una chorrada
        ' cuando el usuario contesta
        If Int(Rnd * 5) > 2 Then
            sCopiaEntrada = "Gracias por indicarme tu " & sCopiaEntrada
        Else
            sCopiaEntrada = "Gracias por decirme que tu " & sCopiaEntrada & " es " & sEntrada
        End If
        ProcesarEntrada = sCopiaEntrada & "."
        Exit Function
    End If
    '
    ' quitar los "sí," y "no," del principio
    If Left$(sEntrada, 3) = "sí," Then
        sEntrada = LTrim$(Mid$(sEntrada, 4))
    End If
    If Left$(sEntrada, 3) = "no," Then
        sEntrada = LTrim$(Mid$(sEntrada, 4))
    End If
    '
    sCopiaEntrada = sEntrada
    ' Convertir la entrada en un array de palabras       (07/Jun/98)
    ' En este procedimiento es dónde se asignarán las palabras que
    ' debe recordar de lo que ha dicho el usuario        (12/Jun/98)
    Entrada2Array sEntrada
    '
    ' Sustituir las palabras de simplificación,
    ' ahora ya no se usan todas, ni los verbos, eso se hará sólo
    ' cuando haya que usar parte de la entrada del usuario
    ' en la respuesta, es decir donde está *RESTO*
    sEntradaSimp = SustituirEnEntrada(sEntrada)
    '
    'Buscar las claves incluidas en la entrada y devolverlas
    'ordenadas según el Nivel
    'sClaves = BuscarClaves(sEntradaSimp)
    '---Ahora devolverá el Nivel más alto usado
    '   sClaves se pasará como parámetro
    '
    '--------------------------------------------------------------------------
    ' Nota:                                                         (16/Sep/02)
    '   Aunque al hacer esta comparación se haga lo mismo,
    '   es importante dejarla, ya que la variable sClaves se modifica
    '   en el caso de que haya claves.
    '--------------------------------------------------------------------------
    ' Si el 'Nivel mayor' es Cero...
    If BuscarClaves(sEntradaSimp, sClaves) = 0 Then
        '
'        'De forma aleatoria dejarla como está o buscar
'        'una respuesta predefinida
'        '
'        'darle pocas posibilidades, ya que no es plan...
'        If Rnd > 0.09 Then
'            ProcesarEntrada = CrearRespuesta(sClaves, sEntradaSimp)
'        Else
'            'Simplificar la entrada, es decir, sustituir las
'            'palabras originales por las que debe usar Eliza
'            'al responder
'            sEntrada = sEntradaSimp
'            sEntradaSimp = SimplificarEntrada(sEntradaSimp)
'            'Si el valor devuelto ha cambiado después de
'            'simplificar, se usará como respuesta,
'            'en caso contrario, crear una respuesta
'            If sCopiaEntrada <> Trim$(sEntradaSimp) Then
'                ProcesarEntrada = "¿" & sEntradaSimp & "?"
'            Else
'                'darle el valor antes de simplificar
'                sEntradaSimp = sEntrada
'                ProcesarEntrada = CrearRespuesta(sClaves, sEntradaSimp)
'            End If
'        End If
        ProcesarEntrada = CrearRespuesta(sClaves, sEntradaSimp)
    Else
        ' Crear la respuesta correspondiente
        ProcesarEntrada = CrearRespuesta(sClaves, sEntradaSimp)
    End If
    '
End Function

Public Sub Inicializar()
    ' Pone a cero todos los valores de la última respuesta usada
    Dim tRespuestas As cRespuestas
    Dim tRegla As cRegla
    Dim sUserAnt As String
    '
    ' Generar una semilla aleatoria para cuando se necesite
    Randomize 'Timer
    '
    If m_Releer Then
        m_Releer = False
        m_Iniciado = False
    End If
    '
    DoEvents
    If Not m_Iniciado Then
        m_Iniciado = True
        ' Leer el fichero de palabras y respuestas
        LeerReglasEliza
    Else
        ' Inicializar el valor del último item usado
        For Each tRegla In m_col
            ' poner a cero las respuestas normales
            tRegla.Respuestas.UltimoItem = 0
            ' poner a cero las respuestas de la sección Extras
            For Each tRespuestas In tRegla.Extras
                tRespuestas.UltimoItem = 0
            Next
        Next
    End If
    ' Si el usuario es uno nuevo, leer la base de datos del usuario
    If sUserAnt <> Nombre Then
        DatosUsuario
        sUserAnt = Nombre
    End If
End Sub

Private Function SustituirEnEntrada(ByVal sEntrada As String) As String
    ' Cambia las palabras de la entrada por las de la colección
    ' de palabras simplificadas
    Dim sPalabra As String
    Dim sPalabra1 As String
    Dim sPalabra1Ant As String
    Dim sSeparador As String
    Dim sSeparador1 As String
    Dim nuevaEntrada As String
    '
    nuevaEntrada = ""
    Do
        ' Buscar dos palabras seguidas
        sPalabra = SiguientePalabra(sEntrada, sSeparador)
        sPalabra1 = SiguientePalabra(sEntrada, sSeparador1)
        sPalabra1Ant = sPalabra1
        '
        PalabraOrig = PalabraOrig + 1
        '
        If Len(sPalabra) <> 0 And Len(sPalabra1) <> 0 Then
            ' Si existen las dos palabras juntas
            If m_colRS.ExisteItem(sPalabra & sSeparador & sPalabra1) Then
                sPalabra = m_colRS(sPalabra & sSeparador & sPalabra1)
                nuevaEntrada = nuevaEntrada & sPalabra & sSeparador1
            Else
                ' sino, tomar la primera y seguir el proceso
                If m_colRS.ExisteItem(sPalabra) Then
                    sPalabra = m_colRS(sPalabra)
                End If
                nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
                ' dejar la entrada como estaba
                ' invertir la palabra, en caso de que antes se
                ' haya encontrado en los verbos
                PalabraOrig = PalabraOrig - 1
                sPalabra1 = sPalabra1Ant
                sEntrada = sPalabra1 & sSeparador1 & sEntrada
            End If
        Else
            ' Sólo debería cumplirse esta cláusula
            If Len(sPalabra) Then
                If m_colRS.ExisteItem(sPalabra) Then
                    sPalabra = m_colRS(sPalabra)
                End If
                nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
            End If
            If Len(sPalabra1) Then
                If m_colRS.ExisteItem(sPalabra1) Then
                    sPalabra1 = m_colRS(sPalabra1)
                End If
                nuevaEntrada = nuevaEntrada & sPalabra1 & sSeparador1
            End If
        End If
    Loop While Len(sEntrada) > 0
    SustituirEnEntrada = nuevaEntrada
End Function

Private Function SimplificarEntrada(ByVal sEntrada As String) As String
    ' Cambia las palabras de la entrada por las de la colección
    ' de palabras simplificadas
    Dim sPalabra As String
    Dim sPalabra1 As String
    Dim sPalabra1Ant As String
    Dim sSeparador As String
    Dim sSeparador1 As String
    Dim nuevaEntrada As String
    '
    nuevaEntrada = ""
    Do
        ' Buscar dos palabras seguidas
        sPalabra = SiguientePalabra(sEntrada, sSeparador)
        sPalabra1 = SiguientePalabra(sEntrada, sSeparador1)
        sPalabra1Ant = sPalabra1
        '
        '==========================================================
        ' Creo que NO se debería dar por hallada una palabra
        ' cuando se ha "conjugado", ya que puede que esté entre
        ' las palabras claves y de esta forma no la encontraría
        '==========================================================
        '
        PalabraOrig = PalabraOrig + 1
        sPalabra = ComprobarVerbos(sPalabra)
        If Len(sPalabra1) Then
            PalabraOrig = PalabraOrig + 1
            sPalabra1 = ComprobarVerbos(sPalabra1)
        End If
        '
        ' De esta forma no funciona:
        'If Len(sPalabra) And Len(sPalabra1) Then
        If Len(sPalabra) <> 0 And Len(sPalabra1) <> 0 Then
            'Si existen las dos palabras juntas
            If m_colSimp.ExisteItem(sPalabra & sSeparador & sPalabra1) Then
                sPalabra = m_colSimp(sPalabra & sSeparador & sPalabra1)
                nuevaEntrada = nuevaEntrada & sPalabra & sSeparador1
            Else
                ' sino, tomar la primera y seguir el proceso
                If m_colSimp.ExisteItem(sPalabra) Then
                    sPalabra = m_colSimp(sPalabra)
                End If
                nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
                ' dejar la entrada como estaba
                ' invertir la palabra, en caso de que antes se
                ' haya encontrado en los verbos
                PalabraOrig = PalabraOrig - 1
                sPalabra1 = sPalabra1Ant
                sEntrada = sPalabra1 & sSeparador1 & sEntrada
            End If
        Else
            ' Sólo debería cumplirse esta cláusula
            If Len(sPalabra) Then
                If m_colSimp.ExisteItem(sPalabra) Then
                    sPalabra = m_colSimp(sPalabra)
                End If
                nuevaEntrada = nuevaEntrada & sPalabra & sSeparador
            End If
            If Len(sPalabra1) Then
                If m_colSimp.ExisteItem(sPalabra1) Then
                    sPalabra1 = m_colSimp(sPalabra1)
                End If
                nuevaEntrada = nuevaEntrada & sPalabra1 & sSeparador1
            End If
        End If
    Loop While Len(sEntrada)
    SimplificarEntrada = nuevaEntrada
End Function

Private Function BuscarClaves(ByVal sEntrada As String, ByRef sClaves As String) As Long
    ' Devuelve las claves halladas
    ' y asigna la variable Claves, devolviendo el número de claves halladas
    Dim sPalabra As String
    Dim sSeparador As String
    '
    Dim tRegla As cRegla
    Dim tRespuestas As cRespuestas
    Dim i As Long
    '
    sClaves = ""
    For Each tRegla In m_col
        '
        i = InStr(sEntrada, tRegla.Contenido)
        If i Then
            ' comprobar si es una palabra completa
            If InStr(sSeparadores, Mid$(sEntrada, i + Len(tRegla.Contenido), 1)) Then
                ' Si el carácter anterior es un separador o el principio de la palabra
                If i > 1 Then
                    i = InStr(sSeparadores, Mid$(sEntrada, i - 1, 1))
                End If
                If i Then
                    For Each tRespuestas In tRegla.Extras
                        i = InStr(sEntrada, tRespuestas.Contenido)
                        If i Then
                            ' Añadir al principio las "subclaves"
                            ' aunque no es necesario, ya que se le da un nivel mayor
                            If InStr(sSeparadores, Mid$(sEntrada, i + Len(tRespuestas.Contenido), 1)) Then
                                sPalabra = tRespuestas.Contenido
                                'sClaves = sClaves & "{" & tRegla.Nivel & "}" & sPalabra & ","
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
    BuscarClaves = OrdenarClaves(sClaves)
End Function

Private Function OrdenarClaves(ByRef sClaves As String) As Long
    ' Ordena las claves según el Nivel
    ' El nivel estará indicado entre llaves y cada palabra separada separada por una coma.
    '
    ' Nota: Esta forma de indicar el nivel no es como se hace en el fichero de palabras,
    '       ya que en el fichero se indica después de la palabra que está entre corchetes.
    '       Es la forma que se hace al llamar a esta función.
    '
    Dim numPalabras As Long
    Dim i As Long
    'Dim tContenidos As New cRespuestas
    Dim tContenidos As cRespuestas
    Dim sNivel As String
    Dim sPalabra As String
    Dim aPalabras() As String
    Dim aOrden() As Long
    'Dim tClassif As New cClasificar
    Dim tClassif As cClasificar
    Dim sTmp As String
    Dim LaMayor As Long
    '
    Set tContenidos = New cRespuestas
    Set tClassif = New cClasificar
    '
    sTmp = sClaves
    Do
        i = InStr(sClaves, "}")
        If i Then
            sNivel = Left$(sClaves, i)
            sClaves = Mid$(sClaves, i + 1)
            i = InStr(sClaves, ",")
            If i Then
                sPalabra = Left$(sClaves, i - 1)
                sClaves = Mid$(sClaves, i + 1)
                tContenidos(sPalabra).Contenido = sNivel
            End If
        End If
    Loop While Len(sClaves)
    numPalabras = tContenidos.Count
    If numPalabras Then
        ReDim aPalabras(1 To numPalabras) As String
        ReDim aOrden(1 To numPalabras) As Long
        For i = 1 To numPalabras
            ' se asignará el nivel, (antes se añadía la palabra)
            aPalabras(i) = tContenidos(i).Contenido
        Next
        ' Clasificar de mayor a menor                               (01/Jun/98)
        tClassif.Classif aPalabras(), aOrden(), False
        LaMayor = Val(Mid$(tContenidos(aOrden(1)).Contenido, 2))
        sTmp = ""
        For i = 1 To numPalabras
            sTmp = sTmp & tContenidos(aOrden(i)).ID & ","
        Next
    End If
    sClaves = sTmp
    ' Se devuelve el Nivel mayor de las claves
    OrdenarClaves = LaMayor
End Function

Private Function CrearRespuesta(ByVal sClaves As String, ByVal sEntrada As String) As String
    ' Devuelve la respuesta,
    ' las claves estarán separadas por comas
    '
    Dim sRespuesta As String
    Dim sPalabra As String
    Dim i As Long, j As Long
    Dim tRegla As cRegla
    '
    ' tomar la primera palabra y buscar la respuesta adecuada
    i = InStr(sClaves, ",")
    If i > 0 Then
        sPalabra = Trim$(Left$(sClaves, i - 1))
    End If
    ' Si no hay una palabra clave
    If Len(sPalabra) = 0 Then
        'sPalabra = "none"
        sPalabra = "respuestas-aleatorias"
        ' en principio sólo "recordará" si no ha entendido
        '
        ' Crear una respuesta con algo que dijo antes...
        ' si es que se tiene constancia de ello...
        sRespuesta = CrearRespuestaRecordando(sPalabra)
    End If
    ' Si no se ha encontrado una respuesta "aleatoria"
    If Len(sRespuesta) = 0 Then
        ' Aquí se puede usar la nueva función BuscarEsaClave        (11/Jun/98)
        sRespuesta = BuscarEsaClave(sPalabra)
    End If
    '
    sRespuesta = ComprobarEspeciales(sRespuesta, sEntrada, sPalabra)
    '
    CrearRespuesta = sRespuesta
    '
    ' Si la respuesta es la despedida, disparar un evento indicando
    ' que se termina
    If Left$(sRespuesta, 5) = "adios" Then
        RaiseEvent Terminado
    End If
End Function

Private Function QuitarCaracterEx(ByVal sValor As String, ByVal sCaracter As String, Optional ByVal sPoner As Variant) As String
    '--------------------------------------------------------------------------
    ' Cambiar/Quitar caracteres                                     (17/Sep/97)
    ' Si se especifica sPoner, se cambiará por ese carácter
    '
    ' Esta versión permite cambiar los caracteres    (17/Sep/97)
    ' y sustituirlos por el/los indicados
    ' a diferencia de QuitarCaracter, no se buscan uno a uno,
    ' sino todos juntos
    '--------------------------------------------------------------------------
    Dim i As Long
    Dim sCh As String
    Dim bPoner As Boolean
    Dim iLen As Long
    '
    bPoner = False
    If Not IsMissing(sPoner) Then
        sCh = sPoner
        bPoner = True
    End If
    iLen = Len(sCaracter)
    If iLen = 0 Then
        QuitarCaracterEx = sValor
        Exit Function
    End If
    '
    ' Si el caracter a quitar/cambiar es Chr$(0), usar otro método
    If Asc(sCaracter) = 0 Then
        ' Quitar todos los chr$(0) del final
        Do While Right$(sValor, 1) = Chr$(0)
            sValor = Left$(sValor, Len(sValor) - 1)
            If Len(sValor) = 0 Then Exit Do
        Loop
        iLen = 1
        Do
            i = InStr(iLen, sValor, sCaracter)
            If i Then
                If bPoner Then
                    sValor = Left$(sValor, i - 1) & sCh & Mid$(sValor, i + 1)
                Else
                    sValor = Left$(sValor, i - 1) & Mid$(sValor, i + 1)
                End If
                iLen = i
            Else
                ' ya no hay más, salir del bucle
                Exit Do
            End If
        Loop
    Else
        i = 1
        Do While i <= Len(sValor)
            'Debug.Print Mid$(sValor, i, 1); Asc(Mid$(sValor, i, 1));
            If Mid$(sValor, i, iLen) = sCaracter Then
                If bPoner Then
                    sValor = Left$(sValor, i - 1) & sCh & Mid$(sValor, i + iLen)
                    i = i - 1
                    ' Si lo que hay que poner está incluido en
                    ' lo que se busca, incrementar el puntero
                    '                                   (11/Jun/98)
                    If InStr(sCh, sCaracter) Then
                        i = i + 1
                    End If
                Else
                    sValor = Left$(sValor, i - 1) & Mid$(sValor, i + iLen)
                End If
            End If
            i = i + 1
        Loop
    End If
    '
    QuitarCaracterEx = sValor
End Function

Private Function SiguientePalabra( _
            sFrase As String, _
            sSeparador As String, _
            Optional ByVal queSeparador As String = "") As String
    ' Busca la siguiente palabra de la frase de entrada
    ' En la frase se devolverá el resto sin la palabra hallada
    '
    ' Si se especifica un caracter (o varios) en queSeparador
    ' se usarán esos para comprobar cual es la siguiente palabra,
    ' sino, se usará el contenido de sSeparadores
    Dim i As Long
    Dim sPalabra As String
    Dim sLosSeparadores As String
    '
    ' Nueva forma de comprobar una nueva palabra                    (10/Jun/98)
    If Len(queSeparador) Then
        sLosSeparadores = queSeparador
    Else
        sLosSeparadores = sSeparadores
    End If
    '
    sPalabra = ""
    sFrase = Trim$(sFrase) & " "
    For i = 1 To Len(sFrase)
        If InStr(sLosSeparadores, Mid$(sFrase, i, 1)) Then
            ' Devolver el resto de la frase y el separador
            sSeparador = Mid$(sFrase, i, 1)
            sFrase = Mid$(sFrase, i + 1)
            Exit For
        Else
            sPalabra = sPalabra & Mid$(sFrase, i, 1)
        End If
    Next
    SiguientePalabra = sPalabra
End Function

Private Sub LeerReglasEliza()
    ' Leer el/los ficheros de palabras clave y respuestas,
    ' así como las reglas de simplificación
    '
    Dim sFic As String
    Dim sDir As String
    '
    ' El directorio de datos
    sDir = AppPath
    '
    ' El primer fichero en leer será Eliza_SPA.txt
    ' El resto se leerán a continuación, permitiendo de esta forma
    ' sustituir algunas reglas y palabras existentes
    sFic = sDir & "Eliza_SPA.txt"
    If Len(Dir$(sFic)) > 0 Then
        LeerReglas sFic
    End If
    ' Buscar los ficheros llamados ElizaVB_*.txt, y leerlos
    sFic = Dir$(sDir & "ElizaVB_*.txt")
    Do While Len(sFic)
        sFic = sDir & sFic
        ' Leer el contenido del fichero
        LeerReglas sFic
        sFic = Dir$
    Loop
    '
    '
    ' Convertir las claves que tienen el formato:
    ' [clave {* xxx; yyy}] en distintas entradas.
    '
    ' Las diferentes entradas TIENEN QUE ESTAR separadas por ;
    '
    Dim tRegla As cRegla
    Dim tRespuestas As cRespuestas
    Dim sPalabra As String
    Dim sPalabra1 As String
    Dim sSeparador As String
    Dim j As Long
    '
    Dim sSubKey As String
    Dim i As Long
    Dim sTmp As String
    '
    sSubKey = ""
    For Each tRegla In m_col
        i = InStr(tRegla.Contenido, "{*")
        If i > 0 Then
            ' En el caso que se ponga alguna palabra después
            ' de la llave de cierre, se usará también
            j = InStr(tRegla.Contenido, "}")
            sPalabra1 = ""
            If j > 0 Then
                sPalabra1 = Trim$(Mid$(tRegla.Contenido, j + 1))
                If Len(sPalabra1) > 0 Then
                    sPalabra1 = " " & sPalabra1
                End If
            End If
            sSubKey = Left$(tRegla.Contenido, i - 1)
            sTmp = Mid$(tRegla.Contenido, i + 2)
            ' La siguiente palabra será la que esté separada por
            ' un punto y coma, de esta forma se permiten palabras
            ' de más de "una palabra"
            sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
            ' Esto es necesario, ya que en la colección la clave
            ' es el contenido anterior, es decir lo que hay en sKey
            '
            ' Para que no hayan dos espacios seguidos en la clave
            sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
            With Reglas(sPalabra)
                .Nivel = tRegla.Nivel
                .Aleatorio = tRegla.Aleatorio
                .Respuestas.Add "*equal:=" & tRegla.Contenido
            End With
            '
            Do While Len(sTmp) > 0
                ' Si no tiene este separador se devuelve lo mismo
                sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                If Len(sPalabra) > 0 Then
                    i = InStr(sPalabra, "}")
                    If i > 0 Then
                        sPalabra = Left$(Trim$(sPalabra), i - 1)
                        sTmp = ""
                    End If
                    sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                    With Reglas(sPalabra)
                        .Nivel = tRegla.Nivel
                        .Aleatorio = tRegla.Aleatorio
                        .Respuestas.Add "*equal:=" & tRegla.Contenido
                    End With
                End If
            Loop
        End If
        ' Buscar en las sub-claves *extras*
        '
        ' Si aquí se encuentra {* se deberá crear una clave
        ' principal, ya que los *equal:= sólo funcionan con
        ' las claves principales.
        '---También busca en las sub-claves extras      (10/Jun/98)
        '
        For Each tRespuestas In tRegla.Extras
            i = InStr(tRespuestas.Contenido, "{*")
            If i > 0 Then
                ' En el caso que se ponga alguna palabra después
                ' de la llave de cierre, se usará también
                j = InStr(tRespuestas.Contenido, "}")
                sPalabra1 = ""
                If j > 0 Then
                    sPalabra1 = Trim$(Mid$(tRespuestas.Contenido, j + 1))
                    If Len(sPalabra1) > 0 Then
                        sPalabra1 = " " & sPalabra1
                    End If
                End If
                sSubKey = Left$(tRespuestas.Contenido, i - 1)
                sTmp = Mid$(tRespuestas.Contenido, i + 2)
                sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                '
                sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                Reglas(tRegla.Contenido).Extras(sPalabra).Add "*equal:=" & tRespuestas.Contenido
                '$Reglas(tRegla.Contenido).Extras(sPalabra).Add "*equal:=" & tRegla.Contenido
                '
                Do While Len(sTmp) > 0
                    sPalabra = SiguientePalabra(sTmp, sSeparador, ";")
                    If Len(sPalabra) Then
                        i = InStr(sPalabra, "}")
                        If i > 0 Then
                            sPalabra = Left$(Trim$(sPalabra), i - 1)
                            sTmp = ""
                        End If
                        '
                        sPalabra = QuitarEspaciosExtras(sSubKey & sPalabra & sPalabra1)
                        Reglas(tRegla.Contenido).Extras(sPalabra).Add "*equal:=" & tRespuestas.Contenido
                        '$Reglas(tRegla.Contenido).Extras(sPalabra).Add "*equal:=" & tRegla.Contenido
                    End If
                Loop
            End If
        Next
    Next
    '
    '--------------------------------------------------------------------------
    '   Formato del fichero de palabras y respuestas (Reglas)
    '--------------------------------------------------------------------------
    ' En el fichero Eliza_SPA.txt están explicadas las claves
    ' y demás cosas a usar para que Eliza entienda lo que se le dice
    '--------------------------------------------------------------------------
End Sub

Private Sub LeerReglas(ByVal sFic As String)
    ' Leer el fichero de palabras indicado en el parámetro          (17/Sep/02)
    Dim nFic As Long
    Dim sTmp As String
    Dim pSeek As Long
    Dim sKey As String
    Dim sSubKey As String
    Dim i As Long
    Dim UsarRs As Boolean
    '
    ' Comprobar si existe el fichero, si no es así, salir.
    ' ¡No comprobarlo, ya que se altera lo que Dir$ devuelve!
    ' (se supone que existe el fichero, ya que
    ' este procedimiento es llamado desde LeerReglasEliza)
    'If Len(Dir$(sFic)) = 0 Then Exit Sub
    '
    nFic = FreeFile
    Open sFic For Input As nFic
    Do While Not EOF(nFic)
        Line Input #nFic, sTmp
        'sTmp = LCase$(Trim$(sTmp))
        sTmp = Trim$(sTmp)
        ' Si no hay más entradas,
        ' esto es por si se quiere crear un fichero de prueba
        ' y se limitará hasta dónde se va a examinar...
        If Left$(sTmp, 4) = ";fin" Then
            Exit Do
        End If
        ' Si hay un comentario o está vacía, no procesar
        If Left$(sTmp, 1) <> ";" And Len(sTmp) > 0 Then
            ' Si no es una sección EXTRAS
            If sTmp <> "[*extras*]" Then
                ' Si son reglas de simplificación
                If sTmp = "[*rs*]" Then
                    UsarRs = True
                    ' leer las reglas de simplificación,
                    ' siempre deben ir por pares
                    Do While Not EOF(nFic)
                        ' Las palabras estarán separadas por comas
                        Line Input #nFic, sTmp
                        sTmp = Trim$(sTmp)
                        If Left$(sTmp, 1) <> ";" And Len(sTmp) > 0 Then
                            If sTmp = "[/rs]" Then
                                Exit Do
                            ElseIf sTmp = "[*simp*]" Then
                                UsarRs = False
                            End If
                            i = InStr(sTmp, ",")
                            If i > 0 Then
                                sKey = Left$(sTmp, i - 1)
                                sTmp = Mid$(sTmp, i + 1)
                                ' Si tiene el signo @ al principio,
                                ' se creará una doble entrada
                                ' Por ejemplo: @soy,eres
                                ' crearía soy,eres y eres,soy
                                If Left$(sKey, 1) = "@" Then
                                    sKey = Mid$(sKey, 2)
                                    If UsarRs Then
                                        m_colRS.Item(sTmp) = sKey
                                    Else
                                        m_colSimp.Item(sTmp) = sKey
                                    End If
                                End If
                                ' añadirla a la colección
                                If UsarRs Then
                                    m_colRS.Item(sKey) = sTmp
                                Else
                                    m_colSimp.Item(sKey) = sTmp
                                End If
                            End If
                        End If
                    Loop
                ElseIf sTmp = "[*verbos*]" Then
                    ' leer los verbos y sus terminaciones
                    Do While Not EOF(nFic)
                        'Se pondrán los verbos y se añadirá a la
                        ' colección sin la terminación, en el formato
                        ' m_Verbos.Item("am")="ar"
                        ' m_Verbos.Item("com")="er"
                        Line Input #nFic, sTmp
                        sTmp = Trim$(sTmp)
                        If Left$(sTmp, 1) <> ";" And Len(sTmp) > 0 Then
                            If sTmp = "[/verbos]" Then
                                Exit Do
                            End If
                            i = Len(sTmp)
                            sKey = Left$(sTmp, i - 2)
                            sTmp = Right$(sTmp, 2)
                            If Left$(sKey, 1) = "%" Then
                                ' quitarle el %
                                sKey = Mid$(sKey, 2)
                                ' cambiar la última vocal
                                ' por una acentuada
                                ' sólo se busca la 'i'
                                If Right$(sKey, 1) = "i" Then
                                    sSubKey = Left$(sKey, Len(sKey) - 1) & "í"
                                    m_Verbos.Item(sSubKey) = sTmp
                                End If
                                sSubKey = ""
                            End If
                            ' añadirlo a la colección
                            m_Verbos.Item(sKey) = sTmp
                        End If
                    Loop
                Else
                    ' debe ser una clave normal
                    If Left$(sTmp, 1) = "[" Then
                        ' Es una Palabra Clave
                        ' Quitarle los corchetes
                        sKey = Mid$(sTmp, 2, Len(sTmp) - 2)
                        sSubKey = ""
                        ' Leer el nivel
                        Line Input #nFic, sTmp
                        sTmp = Trim$(sTmp)
                        Reglas(sKey).Nivel = Val(sTmp)
                        '                                       ( 9/Jun/98)
                        ' Si tiene @ es para tomarlo aleatoriamente
                        If Right$(sTmp, 1) = "@" Then
                            Reglas(sKey).Aleatorio = True
                        Else
                            Reglas(sKey).Aleatorio = False
                        End If
                    Else
                        ' Sino es una clave, es una respuesta
                        ' de la última clave encontrada.
                        ' Añadirla a la clave actual
                        Reglas(sKey).Respuestas.Add sTmp
                    End If
                End If
            Else
                ' Comprobar si hay palabras EXTRAS (subClaves)
                Do While Not EOF(nFic)
                    ' Guardar la posición actual del fichero
                    pSeek = Seek(nFic)
                    Line Input #nFic, sTmp
                    sTmp = Trim$(sTmp)
                    ' si es el final de la sección de EXTRAS:
                    '   salir del bucle
                    If Left$(sTmp, 1) <> ";" And Len(sTmp) > 0 Then
                        If sTmp = "[/extras]" Then
                            Exit Do
                        End If
                        ' Si es una clave, empezará por [
                        If Left$(sTmp, 1) = "[" Then
                            sSubKey = Mid$(sTmp, 2, Len(sTmp) - 2)
                            Reglas(sKey).Extras(sSubKey).Contenido = sSubKey
                        Else
                            ' si no es una clave
                            '
                            ' Si no hay subClave especificada:
                            If Len(sSubKey) = 0 Then
                                ' posicionar el puntero del fichero
                                ' y salir del bucle
                                Seek nFic, pSeek
                                Exit Do
                            Else
                                ' debe ser una respuesta para esta subClave
                                Reglas(sKey).Extras(sSubKey).Add sTmp
                            End If
                        End If
                    End If
                Loop
            End If
        End If
    Loop
    Close nFic
End Sub

Public Function AsignarPalabras( _
            ByVal unList As ListBox, _
            Optional ByVal unList1 As ListBox, _
            Optional ByVal esClave As eTiposDeClaves = eClaves, _
            Optional ByRef sClave As String) As Currency
    ' Asigna las palabras a los listbox del form
    ' Devolverá el número de palabras asignadas al ListBox
    Dim tRegla As cRegla
    Dim tRespuestas As cRespuestas
    Dim tContenido As cContenido
    Dim i As Long, j As Long
    '
    unList.Clear
    Select Case esClave
    Case eClaves
        unList1.Clear
        For Each tRegla In m_col
            unList.AddItem tRegla.Contenido
            i = i + 1
            For Each tRespuestas In tRegla.Extras
                unList1.AddItem tRespuestas.Contenido
                j = j + 1
            Next
        Next
    Case eExtras
        Set tRegla = m_col(sClave)
        If tRegla.Extras.Count Then
            unList.AddItem "---Extras---"
            For Each tRespuestas In tRegla.Extras
                unList.AddItem tRespuestas.Contenido
                i = i + 1
            Next
        End If
        If tRegla.Respuestas.Count Then
            unList.AddItem "---Respuestas---"
            For Each tContenido In tRegla.Respuestas
                unList.AddItem tContenido.Contenido
                j = j + 1
            Next
        End If
    Case eExtras2
        For Each tRegla In m_col
            If tRegla.Extras.Count Then
                For Each tRespuestas In tRegla.Extras
                    If tRespuestas.Contenido = sClave Then
                        For Each tContenido In tRespuestas
                            unList.AddItem tContenido.Contenido
                            j = j + 1
                        Next
                    End If
                Next
            End If
        Next
    Case eVerbos
        For Each tContenido In m_Verbos
            unList.AddItem tContenido.ID & " -- " & tContenido.Contenido
            i = i + 1
        Next
    Case eRS
        For Each tContenido In m_colRS
            unList.AddItem tContenido.ID & " -- " & tContenido.Contenido
            i = i + 1
        Next
    Case eSimp
        For Each tContenido In m_colSimp
            unList.AddItem tContenido.ID & " -- " & tContenido.Contenido
            i = i + 1
        Next
    Case eRec
        For Each tContenido In m_colRec
            unList.AddItem tContenido.ID & " -- " & tContenido.Contenido
            i = i + 1
        Next
    Case eBU
        For Each tContenido In BaseUser
            unList.AddItem tContenido.ID & " -- " & tContenido.Contenido
            i = i + 1
        Next
    End Select
    AsignarPalabras = i + j / 1000
End Function

Public Property Get Sexo() As eSexo
    Sexo = m_Sexo
End Property

Public Property Let Sexo(ByVal NewSexo As eSexo)
    'sólo se deben admitir los valores indicados
    'de no ser así se asignará Masculino
    Select Case NewSexo
    Case Masculino, Femenino
        'nada
    Case Else
        NewSexo = Masculino
    End Select
    m_Sexo = NewSexo
End Property

Private Function ComprobarVerbos(ByVal sPalabra As String) As String
    ' Comprueba si cumple las nomas indicadas y busca en la lista de verbos,
    ' si lo encuentra, lo convierte convenientemente.
    ' Devolverá la nueva palabra o la original
    '
    Dim tContenido As cContenido
    Dim hallado As Boolean
    Dim i As Long
    Dim sInfinitivo As String
    Dim sPalabraAnt As String
    '
    If Len(sPalabra) = 0 Then
        ComprobarVerbos = ""
        Exit Function
    End If
    '
    ' Antes de convertir el verbo, comprobar si la palabra anterior
    ' es 'el' o 'un' en ese caso, no se conjugará
    ' el juego X -> el juegas X, sino el juego -> el juego
    hallado = False
    If PalabraOrig > 1 And PalabraOrig < PalabrasOrig Then
        If InStr(" el un al del ", FraseOrig(PalabraOrig - 1)) > 0 Then
            hallado = True
        End If
'        If FraseOrig(PalabraOrig - 1) = "el" Then
'            hallado = True
'        ElseIf FraseOrig(PalabraOrig - 1) = "al" Then
'            hallado = True
'        ElseIf FraseOrig(PalabraOrig - 1) = "del" Then
'            hallado = True
'        ElseIf FraseOrig(PalabraOrig - 1) = "un" Then
'            hallado = True
'        End If
    End If
    '
    If Not hallado Then
        '
        'Jugar con las terminaciones:
        'arme->arte, erme->erte, irme->irte
        'Se puede simplificar con rme->rte
        i = Len(sPalabra) - 3
        'Guardar la palabra por si resulta que no es un verbo
        sPalabraAnt = sPalabra
        hallado = False
        Select Case Right$(sPalabra, 3)
        Case "rme"
            sPalabra = Left$(sPalabra, i) & "rte"
            hallado = True
        Case "rte"
            sPalabra = Left$(sPalabra, i) & "rme"
            hallado = True
        End Select
        'Comprobar las terminaciones telo/melo tela/mela
        Select Case Right$(sPalabra, 4)
        Case "melo"
            sPalabra = Left$(sPalabra, i - 1) & "telo"
            i = i - 2
            hallado = True
        Case "telo"
            sPalabra = Left$(sPalabra, i - 1) & "melo"
            i = i - 2
            hallado = True
        Case "mela"
            sPalabra = Left$(sPalabra, i - 1) & "tela"
            i = i - 2
            hallado = True
        Case "tela"
            sPalabra = Left$(sPalabra, i - 1) & "mela"
            i = i - 2
            hallado = True
        End Select
        
        'Comprobar si es uno de los verbos conocidos
        'el infinitivo será la longitud de la palabra - 2
        'el valor de 'i' es 3 caracteres menos de la longitud total
        If hallado Then
            'pero sólo tomamos la parte sin la forma infinitiva,
            'de am-arme nos quedamos con am
            sInfinitivo = Left$(sPalabra, i - 1)
            hallado = False
            For Each tContenido In m_Verbos
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
        '
        'Si no se ha hallado una de estas formas,
        'se comprobará si es un verbo
        If Not hallado Then
            'primero evaluar los verbos
            For Each tContenido In m_Verbos
                hallado = False
                With tContenido
                    If InStr(" " & sPalabra, " " & .ID) Then
                        '------------------------------------------------------
                        ' Terminaciones en 'ar'
                        '------------------------------------------------------
                        If .Contenido = "ar" Then
                            ' presente
                            '
                            ' Para: doy -> das y viceversa,         (15/Sep/02)
                            ' Aunque se supone que debería corregirse con
                            ' las reglas de simplificación.
                            If sPalabra = .ID & "oy" Then
                                sPalabra = .ID & "as"
                                hallado = True
                            '
                            ElseIf sPalabra = .ID & "o" Then
                                sPalabra = .ID & "as"
                                hallado = True
                            ElseIf sPalabra = .ID & "as" Then
                                sPalabra = .ID & "o"
                                hallado = True
                            'pretérito imperf.
                            ElseIf sPalabra = .ID & "aba" Then
                                sPalabra = .ID & "abas"
                                hallado = True
                            ElseIf sPalabra = .ID & "abas" Then
                                sPalabra = .ID & "aba"
                                hallado = True
                            'pretérito perf. simple (pret.indef.)
                            ElseIf sPalabra = .ID & "é" Then
                                sPalabra = .ID & "aste"
                                hallado = True
                            ElseIf sPalabra = .ID & "aste" Then
                                sPalabra = .ID & "é"
                                hallado = True
                            'futuro (imperf.)
                            ElseIf sPalabra = .ID & "aré" Then
                                sPalabra = .ID & "arás"
                                hallado = True
                            ElseIf sPalabra = .ID & "arás" Then
                                sPalabra = .ID & "aré"
                                hallado = True
                            'condicional
                            ElseIf sPalabra = .ID & "aría" Then
                                sPalabra = .ID & "arías"
                                hallado = True
                            ElseIf sPalabra = .ID & "arías" Then
                                sPalabra = .ID & "aría"
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
                            'Pretér.imperf. de subjuntivo (1ª)
                            ElseIf sPalabra = .ID & "ara" Then
                                sPalabra = .ID & "aras"
                                hallado = True
                            ElseIf sPalabra = .ID & "aras" Then
                                sPalabra = .ID & "ara"
                                hallado = True
                            'Pretér.imperf. de subjuntivo (2ª)
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
                            '
                            ElseIf sPalabra = .ID & "ándome" Then
                                sPalabra = .ID & "ándote"
                                hallado = True
                            ElseIf sPalabra = .ID & "ándote" Then
                                sPalabra = .ID & "ándome"
                                hallado = True
                            '$comprobar si es genérico
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
                        ElseIf .Contenido = "er" Or .Contenido = "ir" Then
                            'presente
                            If sPalabra = .ID & "o" Then
                                sPalabra = .ID & "es"
                                hallado = True
                            ElseIf sPalabra = .ID & "es" Then
                                sPalabra = .ID & "o"
                                hallado = True
                            'pretérito imperf.
                            ElseIf sPalabra = .ID & "ía" Then
                                sPalabra = .ID & "ías"
                                hallado = True
                            ElseIf sPalabra = .ID & "ías" Then
                                sPalabra = .ID & "ía"
                                hallado = True
                            'pretérito perf. simple (pret.indef.)
                            ElseIf sPalabra = .ID & "í" Then
                                sPalabra = .ID & "iste"
                                hallado = True
                            ElseIf sPalabra = .ID & "iste" Then
                                sPalabra = .ID & "í"
                                hallado = True
                            'futuro (imperf.) para "er"
                            ElseIf sPalabra = .ID & "eré" Then
                                sPalabra = .ID & "erás"
                                hallado = True
                            ElseIf sPalabra = .ID & "erás" Then
                                sPalabra = .ID & "eré"
                                hallado = True
                            'futuro (imperf.) para "ir"
                            ElseIf sPalabra = .ID & "iré" Then
                                sPalabra = .ID & "irás"
                                hallado = True
                            ElseIf sPalabra = .ID & "irás" Then
                                sPalabra = .ID & "iré"
                                hallado = True
                            'condicional para "er"
                            ElseIf sPalabra = .ID & "ería" Then
                                sPalabra = .ID & "erías"
                                hallado = True
                            ElseIf sPalabra = .ID & "erías" Then
                                sPalabra = .ID & "ería"
                                hallado = True
                            'condicional para "ir"
                            ElseIf sPalabra = .ID & "iría" Then
                                sPalabra = .ID & "irías"
                                hallado = True
                            ElseIf sPalabra = .ID & "irías" Then
                                sPalabra = .ID & "iría"
                                hallado = True
                            'Presente de subjuntivo
                            ElseIf sPalabra = .ID & "a" Then
                                sPalabra = .ID & "as"
                                hallado = True
                            ElseIf sPalabra = .ID & "as" Then
                                sPalabra = .ID & "a"
                                hallado = True
                            'Pretér.imperf. de subjuntivo (1ª)
                            ElseIf sPalabra = .ID & "iera" Then
                                sPalabra = .ID & "ieras"
                                hallado = True
                            ElseIf sPalabra = .ID & "ieras" Then
                                sPalabra = .ID & "iera"
                                hallado = True
                            'Pretér.imperf. de subjuntivo (2ª)
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
                            '
                            ElseIf sPalabra = .ID & "iéndome" Then
                                sPalabra = .ID & "iéndote"
                                hallado = True
                            ElseIf sPalabra = .ID & "iéndote" Then
                                sPalabra = .ID & "iéndome"
                                hallado = True
                            '$comprobar si es genérico
                            ElseIf sPalabra = .ID & "me" Then
                                sPalabra = .ID & "te"
                                hallado = True
                            ElseIf sPalabra = .ID & "te" Then
                                sPalabra = .ID & "me"
                                hallado = True
                            End If
                        End If '---terminación
                        '
                        'Si se ha encontrado, salir del bucle
                        If hallado Then
                            Exit For
                        End If
                    End If '---si está contenido en la palabra
                End With
            Next
        End If
    End If
    '
    'devolver la nueva palabra o la original
    ComprobarVerbos = sPalabra
End Function

Private Function BuscarReglas(ByVal sPalabra As String, ByVal tRegla As cRegla) As String
    ' Busca una palabra en tRegla,
    ' devolverá la respuesta hallada o una cadena vacía
    '
    Dim sRespuesta As String
    Dim i As Long
    Dim tRespuestas As cRespuestas
    '
    ' buscar esta palabra clave en la lista
    sRespuesta = ""
    '
    ' Primero buscar en las respuestas Extras, si hay algunas
    For Each tRespuestas In tRegla.Extras
        If tRespuestas.Contenido = sPalabra Then
            '                                                       (09/Jun/98)
            ' Si la respuesta se obtiene de forma aleatoria,
            ' sólo se hará para buscar la primera,
            ' después se continuará secuencialmente.
            '
            i = tRespuestas.UltimoItem + 1
            If tRegla.Aleatorio Then
                If tRespuestas.UltimoItem = 0 Then
                    i = Int(Rnd * tRespuestas.Count) + 2
                End If
            End If
            tRespuestas.UltimoItem = i
            ' Si el siguiente item a usar es mayor que el total
            ' de respuestas disponibles
            If i > tRespuestas.Count Then
                ' usar la primera respuesta y reiniciar
                ' el número del item a usar la próxima vez
                i = 1
                tRespuestas.UltimoItem = 0
            End If
            sRespuesta = tRespuestas(i).Contenido
        End If
    Next
    ' Si no se ha encontrado una respuesta en los Extras
    ' se comprueba si esa clave es el contenido de esta Regla
    If Len(sRespuesta) = 0 Then
        If tRegla.Contenido = sPalabra Then
            If tRegla.Respuestas.Count Then
                i = tRegla.Respuestas.UltimoItem + 1
                tRegla.Respuestas.UltimoItem = i
                If i > tRegla.Respuestas.Count Then
                    i = 1
                    tRegla.Respuestas.UltimoItem = 0
                End If
                sRespuesta = tRegla.Respuestas(i).Contenido
            End If
        End If
    End If
    ' Devolver la respuesta hallada
    BuscarReglas = sRespuesta
End Function

Private Function QuitarEspaciosExtras(ByVal sCadena As String) As String
    ' Quita los espacios extras dentro de la cadena                 ( 5/Jun/98)
    Dim i As Long
    '
    sCadena = Trim$(sCadena)
    Do
        i = InStr(sCadena, "  ")
        If i > 0 Then
            sCadena = Left$(sCadena, i - 1) & " " & LTrim$(Mid$(sCadena, i + 2))
        End If
    Loop While i > 0
    QuitarEspaciosExtras = sCadena
End Function

Private Sub Entrada2Array(ByVal sEntrada As String)
    ' Esta función convierte el string de entrada en un array
    ' Aquí se comprobará si se escribe Mi o Mis y se agregará
    ' a la colección de palabras a recordar
    '
    Dim sPalabra As String
    Dim sSeparador As String
    Dim RecordarFrase As Long
    Dim sEntradaOrig As String
    '
    ' Iniciar los valores del número de palabras y la actual
    PalabrasOrig = 0
    PalabraOrig = 0
    sEntradaOrig = sEntrada
    '
    Do While Len(sEntrada) > 0
        sPalabra = SiguientePalabra(sEntrada, sSeparador)
        If Len(sPalabra) Then
            PalabrasOrig = PalabrasOrig + 1
            ReDim Preserve FraseOrig(PalabrasOrig)
            FraseOrig(PalabrasOrig) = sPalabra '& sSeparador
            If sPalabra = "mi" Or sPalabra = "mis" Then
                If RecordarFrase = 0 Then
                    RecordarFrase = PalabrasOrig
                End If
            End If
        End If
    Loop
    ' si ha mencionado mi o mis
    If RecordarFrase Then
        ' sólo si no es la última palabra
        If RecordarFrase < PalabrasOrig Then
            ' Añadirla a la colección o sustituirla por la nueva entrada
            ' Sólo se usará la última letra en caso de que sea S
            ' Para después usar tu(s) xxx
            If Right$(FraseOrig(RecordarFrase), 1) = "s" Then
                sSeparador = "s "
            Else
                sSeparador = " "
            End If
            '
            '$Por hacer                                 (19/Jun/98)
            ' Habría que comprobar que la siguiente palabra no sea
            ' algo a lo que el usuario ha hecho referencia de lo
            ' que Eliza dijera, por ejemplo:
            ' a "mi tus" problemas no me interesan... etc.
            ' Por tanto creo que se debería comprobar si es una
            ' de las palabras clave para que así se pueda dirigir
            ' mejor el diálogo.
            'FraseOrig(RecordarFrase + 1)
            m_colRec(sSeparador & FraseOrig(RecordarFrase + 1)).Contenido = sEntradaOrig
        End If
    End If
End Sub

Public Sub Releer()
    m_Releer = True
    Inicializar
End Sub

Public Function Estadísticas() As cRespuestas
    'Devolverá una colección con los siguientes datos:
    '(realmente devuelve la cantidad)
    '   Palabras usadas para sustitución
    '   Verbos
    '   Palabras claves
    '   Sub-Claves (variantes de las claves)
    '   Respuestas en palabras claves
    '   Respuestas en Sub-Claves
    '
    'Dim colRespuestas As New cRespuestas
    Dim colRespuestas As cRespuestas
    Dim tRespuestas As cRespuestas
    Dim tContenido As cContenido
    Dim tRegla As cRegla
    Dim i As Long, j As Long, k As Long
    Dim nMayor As Long
    Dim sMayor As String
    '
    Set colRespuestas = New cRespuestas
    '
    For Each tRegla In m_col
        'sub-claves (extras)
        i = i + tRegla.Extras.Count
        'respuestas
        If tRegla.Respuestas.Count > nMayor Then
            nMayor = tRegla.Respuestas.Count
            sMayor = tRegla.Contenido
        End If
        j = j + tRegla.Respuestas.Count
        For Each tRespuestas In tRegla.Extras
            'Respuestas en las sub-claves
            k = k + tRespuestas.Count
        Next
    Next
    
    With colRespuestas
        .Item("Palabras usadas para simplificar").Contenido = m_colRS.Count
        .Item("Palabras usadas para sustitución").Contenido = m_colSimp.Count
        .Item("Verbos").Contenido = m_Verbos.Count
        .Item("Palabras claves").Contenido = m_col.Count
        .Item("Sub-Claves (variantes de las claves)").Contenido = i
        .Item("-----").Contenido = "-----"
        .Item("Número total de palabras reconocidas").Contenido = m_colSimp.Count + m_colRS.Count + m_Verbos.Count + m_col.Count + i
        .Item("------").Contenido = "-----"
        .Item("Respuestas en palabras claves").Contenido = j
        .Item("Respuestas en Sub-Claves").Contenido = k
        .Item("--------").Contenido = "-----"
        .Item("Número total de Respuestas").Contenido = k + j
        .Item("---------").Contenido = "-----"
        .Item("Clave principal con más respuestas").Contenido = "'" & sMayor & "'"
        .Item("Número de respuestas de '" & sMayor & "'").Contenido = nMayor
    End With
    Set Estadísticas = colRespuestas
    Set colRespuestas = Nothing
End Function

Private Function BuscarEsaClave(ByVal sPalabra As String) As String
    ' Comprobar si sPalabra es una clave, si es así,
    ' devolver la siguiente respuesta
    Dim tRegla As cRegla
    Dim sRespuesta As String
    Dim i As Long
    '
    ' Comprobar primero si está en las reglas de simplificación
    '
    If m_colRS.ExisteItem(sPalabra) Then
        sRespuesta = m_colRS(sPalabra)
        ' si se ha encontrado, el contenido será lo que esté
        ' en la lista de palabras clave
        If Len(sRespuesta) Then
            sPalabra = sRespuesta
        End If
    End If
    ' Buscar esta palabra clave en la lista
    For Each tRegla In m_col
        ' buscar la palabra en cada una de las "claves" y subclaves
        sRespuesta = BuscarReglas(sPalabra, tRegla)
        If Len(sRespuesta) Then
            Exit For
        End If
    Next
    '
    ' Si es una clave especial, no tener en cuenta el *equal:=
    If InStr(sRespuesta, "{*iif") Then
        BuscarEsaClave = sRespuesta
        Exit Function
    End If
    ' Si se usa {*base:=...}
    If InStr(sRespuesta, "{*base:=") Then
        BuscarEsaClave = sRespuesta
        Exit Function
    End If
    '
    ' Si el contenido de sRespuesta es:*equal:=xxx
    ' quiere decir que se debe buscar en la clave "xxx"
    '
    i = InStr(sRespuesta, "*equal:=")
    ' Como ahora se pueden tener respuestas que incluyan *equal:=
    ' se debe buscar respuesta sólo si esta "clave" está al
    ' principio de la respuesta                                     (13/Jun/98)
    Do While i = 1
        sPalabra = LTrim$(Mid$(sRespuesta, i + 8))
        On Local Error Resume Next
        Set tRegla = m_col(sPalabra)
        If Err = 0 Then
            sRespuesta = BuscarReglas(sPalabra, tRegla)
        Else
            Err = 0
            ' No existe como clave principal,
            ' hay que buscarlo en las sub-claves
            For Each tRegla In m_col
                sRespuesta = BuscarReglas(sPalabra, tRegla)
                If Len(sRespuesta) Then
                    Exit For
                End If
            Next
            '
        End If
        i = InStr(sRespuesta, "*equal:=")
    ' repetir mientras en sRespuesta hay un *equal:=
    Loop
    BuscarEsaClave = sRespuesta
End Function

Private Function CrearRespuestaRecordando(ByVal sPalabra As String) As String
    Dim tRegla As cRegla
    Dim i As Long, j As Long
    Dim sRespuesta As String
    Static UsarEstaRespuesta As Long
    '$Para probar usar el valor de las respuestas que tenemos,
    'en casos normales usar un valor mayor
    Const NUM_RESPUESTAS As Long = 10
    '
    sRespuesta = ""
    Set tRegla = m_col(sPalabra)
    ' sólo si son dos o más cosas que no ha entendido
    If tRegla.Respuestas.UltimoItem > 2 Then
        If m_colRec.Count > 0 Then
            ' tomar aleatoriamente una de las cosas que
            ' ha dicho el usuario
            j = 0
            Do
                j = j + 1
                i = Int(Rnd * m_colRec.Count) + 1
                ' Sólo usar este tema si se ha mencionado antes
                If m_colRec.UltimoItem <> i Then
                    'j = 20
                    Exit Do
                End If
                ' Si sólo hay un dato en la colección...
                ' no quedarse en un bucle sin fin...
            Loop Until j > 10
            '
            m_colRec.UltimoItem = i
            UsarEstaRespuesta = UsarEstaRespuesta + 1
            If UsarEstaRespuesta > NUM_RESPUESTAS Then
                UsarEstaRespuesta = 1
            End If
            ' El valor aleatorio sacado indicará si se usa o no
            ' una respuesta "del recuerdo"
            ' Esto es para que no se repitan las respuestas en el
            ' mismo orden...
            j = 0
            ' le damos más "peso" al uso de este tipo de respuestas
            ' de Eliza para "aparentar" más inteligencia
            If Int(Rnd * 10) > 3 Then
                j = UsarEstaRespuesta
            End If
            ' De estos valores sólo se tendrán en cuenta los aquí
            ' indicados, en caso contrario,
            ' usar una de las respuestas "predefinidas",
            ' para ello se asigna una cadena vacia a sRespuesta
            Select Case j
            Case 1
                sRespuesta = "Antes mencionaste tu" & m_colRec(i).ID & ", hablame más de ello " & Trim$(Left$(m_colRec(i).ID, 1)) & "."
            Case 2
                sRespuesta = "Me comentabas sobre tu" & m_colRec(i).ID & ", ¿cómo te influye?"
            Case 3
                sRespuesta = "¿Crees que la relación con tu" & m_colRec(i).ID & " es el motivo de tu problema?"
            Case 4
                sRespuesta = "¿Cómo crees que " & SimplificarEntrada(m_colRec(i).Contenido) & " podría influir en tu comportamiento?"
            Case 5
                sRespuesta = "Háblame más de tus relaciones con tu" & m_colRec(i).ID
            Case Else
                sRespuesta = ""
            End Select
        End If
    End If
    CrearRespuestaRecordando = sRespuesta
End Function

Private Function EsNegativo(ByVal sEntrada As String) As Boolean
    'comprobar si en la cadena de entrada hay alguna palabra
    'que denote negación
    Dim aNegativo As Variant
    Dim i As Long
    
    sEntrada = " " & sEntrada & " "
    EsNegativo = False
    'Crear un array con palabras "negativas"
    aNegativo = Array(" no ", " no,", " nope", " nil", " negativo", " falso", " nada", " ya est", " ya vale")
    For i = 0 To UBound(aNegativo)
        If InStr(sEntrada, aNegativo(i)) Then
            EsNegativo = True
            Exit For
        End If
    Next
End Function

Private Function EsAfirmativo(ByVal sEntrada As String) As Boolean
    'comprobar si en la cadena de entrada hay alguna palabra
    'que denote afirmación
    Dim aAfirmativo As Variant
    Dim i As Long
    
    sEntrada = " " & sEntrada & " "
    EsAfirmativo = False
    aAfirmativo = Array(" sí ", " si ", " sí,", " si,", " afirmativo", " efectivamente", " así es", " asi es", " por supuesto", " ciertamente", " eso es", " vale", " ok", " o.k.", " de acuerdo", " muy bien", " ya que insistes", " claro")
    For i = 0 To UBound(aAfirmativo)
        If InStr(sEntrada, aAfirmativo(i)) Then
            EsAfirmativo = True
            Exit For
        End If
    Next
End Function

Private Function ComprobarEspeciales(ByVal sRespuesta As String, ByVal sEntrada As String, ByVal sPalabra As String) As String
    'Comprobar las claves especiales de sustitución y otras
    'que puedan estar en la respuesta generada          (13/Jun/98)
    Dim i As Long, j As Long
    Static restoAnt As String
    '
    'comprueba si la respuesta contiene caracteres especiales
    If Len(sRespuesta) Then
        'Comprobar si hay que poner la hora
        i = InStr(sRespuesta, "*HORA*")
        If i Then
            sRespuesta = Left$(sRespuesta, i - 1) & Format$(Now, "hh:mm") & Mid$(sRespuesta, i + Len("*HORA*"))
        End If
        'Comprobar si hay que poner el día de hoy
        i = InStr(sRespuesta, "*HOY*")
        If i Then
            sRespuesta = Left$(sRespuesta, i - 1) & Format$(Now, "dddd, dd") & " de " & StrConv(Format$(Now, "mmmm"), vbProperCase) & Mid$(sRespuesta, i + Len("*HOY*"))
        End If
        
        'comprobar si hay que añadir el RESTO
        If InStr(sRespuesta, "*RESTO*") Then
            i = InStr(sEntrada, sPalabra)
            If i Then
                sEntrada = Mid$(sEntrada, i + Len(sPalabra) + 1)
            End If
            ' Sustituir *RESTO* por sEntrada
            i = InStr(sRespuesta, "*RESTO*")
            If i Then
                sEntrada = SimplificarEntrada(sEntrada)
                ' Guardar la respuesta anterior,                    (17/Sep/02)
                ' por si se usa pra asignar a la base de datos del usuario.
                restoAnt = Trim$(sEntrada)
                sRespuesta = Left$(sRespuesta, i - 1) & sEntrada & Mid$(sRespuesta, i + Len("*RESTO*"))
            End If
        End If
    Else
        sRespuesta = sEntrada
    End If
    'Cambiar los *ea* por el correspondiente según el sexo
    If InStr(sRespuesta, "*ea*") Then
        sPalabra = "e"
        If m_Sexo = Femenino Then
            sPalabra = "a"
        End If
        'Cambiar las posibles ocurrencias de *ea* por sPalabra
        sRespuesta = QuitarCaracterEx(sRespuesta, "*ea*", sPalabra)
    End If
    'Cambiar los *oa* por el correspondiente según el sexo
    If InStr(sRespuesta, "*oa*") Then
        sPalabra = "o"
        If m_Sexo = Femenino Then
            sPalabra = "a"
        End If
        'Cambiar las posibles ocurrencias de *oa* por sPalabra
        sRespuesta = QuitarCaracterEx(sRespuesta, "*oa*", sPalabra)
    End If
    
    'Si el primer caracter es una ¿, el último debe ser ?
    If Left$(sRespuesta, 1) = "¿" Then
        'If Right$(sRespuesta, 1) <> "?" Then
        If InStr(sRespuesta, "?") = 0 Then
            sRespuesta = sRespuesta & "?"
        End If
    End If
    'Si existen dos caracteres iguales al final, dejar sólo uno
    'sólo si no es el punto, por aquello de los ...     (11/Jun/98)
    If Right$(sRespuesta, 1) <> "." Then
        i = Len(sRespuesta)
        If Mid$(sRespuesta, i - 1, 1) = Right$(sRespuesta, 1) Then
            sRespuesta = Left$(sRespuesta, i - 1)
        End If
    End If
    '
    ' si se indica *mi_edad*, calcular la edad                      (18/Sep/02)
    i = InStr(sRespuesta, "*mi_edad*")
    If i > 0 Then
        sRespuesta = Left$(sRespuesta, i - 1) & CStr(Year(Now) - 1998) & Mid$(sRespuesta, i + 9)
    End If
    '
    'Cambiar *NOMBRE* por el nombre
    '---Ahora se hará de forma aleatoria
    i = InStr(sRespuesta, "*NOMBRE*")
    If i Then
        'Usar siempre el nombre
        'If Rnd > 0.4 Then
            'Usar el nombre
            sRespuesta = Left$(sRespuesta, i - 1) & Nombre & Mid$(sRespuesta, i + Len("*NOMBRE*"))
        'Else
        '    'No usar el nombre
        '    '
        '    'si tiene una coma, quitarla                ( 3/Jun/98)
        '    j = InStr(sRespuesta, "*NOMBRE*,")
        '    If j Then
        '        sRespuesta = Left$(sRespuesta, j - 1) & Mid$(sRespuesta, j + Len("*NOMBRE*,"))
        '    Else
        '        sRespuesta = Left$(sRespuesta, i - 1) & Mid$(sRespuesta, i + Len("*NOMBRE*"))
        '    End If
        'End If
    End If
    
    '$Comprobar si después de un sigo de separación no hay espacio
    'Lo que se hace es añadirle el espacio, ya que posteriormente
    'se le quitarían los espacios extras
    sRespuesta = QuitarCaracterEx(sRespuesta, ",", ", ")
    'más arreglos del texto
    sRespuesta = QuitarCaracterEx(sRespuesta, " , ", ", ")
    
    'quitarle los dobles espacios que haya
    sRespuesta = QuitarEspaciosExtras(sRespuesta)
    'quitar los espacios de delante de la interrogación final
    sRespuesta = QuitarCaracterEx(sRespuesta, " ?", "?")
    'quitar los espacios de después de la interrogación inicial
    sRespuesta = QuitarCaracterEx(sRespuesta, "¿ ", "¿")
    '
    'Si la respuesta contiene {*iif(
    i = InStr(sRespuesta, "{*iif(")
    sUsarPregunta = ""
    If i Then
        'El formato será: {*iif(condición; ES-TRUE)(ES-FALSE)}
        '
        sUsarPregunta = Mid$(sRespuesta, i)
        sRespuesta = Left$(sRespuesta, i - 1)
        j = InStr(sUsarPregunta, ";")
        i = InStr(j, sUsarPregunta, "(")
        sRespuestas(cNegativa) = Mid$(sUsarPregunta, i + 1, Len(sUsarPregunta) - i - 2)
        sRespuestas(cAfirmativa) = Mid$(sUsarPregunta, j + 1, i - j - 2)
        sUsarPregunta = Mid$(sUsarPregunta, 7, j - 7)
    End If
    'Si la respuesta contiene {*base:=
    sUsarBaseDatos = ""
    i = InStr(sRespuesta, "{*base:=")
    If i Then
        'El formato será: {*base:=clave_base}
        'sUsarBaseDatos contendrá la clave de la base de datos
        sUsarBaseDatos = Mid$(sRespuesta, i + 8)
        sRespuesta = Left$(sRespuesta, i - 1)
        'Quitarle el } del final
        i = InStr(sUsarBaseDatos, "}")
        If i Then
            ' si a continuación sigue un := asignar el valor indicado
            j = InStr(sUsarBaseDatos, ":=*restoant*")
            If j Then
                sUsarBaseDatos = Left$(sUsarBaseDatos, j - 1)
                ValidarDatosParaBase restoAnt
            Else
                sUsarBaseDatos = Left$(sUsarBaseDatos, i - 1)
            End If
        End If
    End If
    'Si la respuesta incluye: *iif(*base*
    'se comprobará si el dato está en la base de datos,
    'de se así se usará lo que venga después del ;
    'en caso contrario se usará lo que esté después de )(
    '*iif(*base*signo_zodiaco;*usarbase:=signo_zodiaco*)
    '                         (*equal:=cual es tu signo)
    i = InStr(sRespuesta, "*iif(*base*")
    If i Then
        Dim sClave As String
        Dim sTrue As String
        Dim sFalse As String
        
        sEntrada = Left$(sRespuesta, i - 1)
        sClave = Mid$(sRespuesta, i + 11)
        j = InStr(sClave, ";")
        If j Then
            sClave = Left$(sClave, j - 1)
            j = InStr(sRespuesta, ";")
            sRespuesta = Mid$(sRespuesta, j + 1)
            j = InStr(sRespuesta, ")(")
            If j Then
                sTrue = Left$(sRespuesta, j - 1)
                sFalse = Mid$(sRespuesta, j + 2)
                If Right$(sFalse, 1) = ")" Then
                    sFalse = Left$(sFalse, Len(sFalse) - 1)
                End If
            End If
            If BaseUser.ExisteItem(sClave) Then
                'Comprobar si hay que sustituir el dato
                '*usarbase:=signo_zodiaco*
                'Si después de la clave se
                i = InStr(sTrue, "*usarbase:=")
                If i Then
                    j = InStr(i + 1, sTrue, "*")
                    sClave = Mid$(sTrue, i + Len("*usarbase:="), j - (i + Len("*usarbase:=")))
                    If BaseUser.ExisteItem(sClave) Then
                        sTrue = Left$(sTrue, i - 1) & " " & BaseUser(sClave).Contenido & " " & Mid$(sTrue, j + 1)
                    Else
                        sTrue = Left$(sTrue, i - 1) & " " & Mid$(sTrue, j + 1)
                    End If
                End If
                sRespuesta = sTrue
            Else
                sRespuesta = sFalse
            End If
            sRespuesta = sEntrada & sRespuesta
            i = InStr(sRespuesta, "*equal:=")
            If i Then
                sRespuesta = Trim$(Mid$(sRespuesta, i + 8))
                sRespuesta = BuscarEsaClave(sRespuesta)
                're-entrar para comprobar nuevas claves
                If Len(sRespuesta) Then
                    sRespuesta = ComprobarEspeciales(sRespuesta, sRespuesta, "")
                End If
            End If
'            're-entrar para comprobar nuevas claves
'            If Len(sRespuesta) Then
'                sRespuesta = ComprobarEspeciales(sRespuesta, sEntrada, "")
'            End If
        End If
    End If
    ComprobarEspeciales = sRespuesta
End Function

Public Property Get Iniciado() As Boolean
    Iniciado = m_Iniciado
End Property

Public Property Let Iniciado(ByVal NewValue As Boolean)
    m_Iniciado = NewValue
End Property

Private Sub DatosUsuario(Optional ByVal AccionLeer As Boolean = True)
    'Leerá la base de datos de este usuario.            (14/Jun/98)
    'El formato del fichero será:
    '   clave=valor
    Dim sFic As String
    Dim nFic As Long
    Dim sTmp As String
    Dim i As Long
    Dim sClave As String
    Dim sPath As String
    '
    'sPath = IIf(Right$(App.Path, 1) = "\", App.Path, App.Path & "\")
    sPath = AppPath & "Bases"
    On Local Error Resume Next
    MkDir sPath
    Err = 0
    On Local Error GoTo 0
    sFic = sPath & "\Datos_" & Nombre & ".txt"
    '
    If AccionLeer Then
        Set BaseUser = Nothing
        Set BaseUser = New cRespuestas
        'Para que tenga algunos datos
        BaseUser("Nombre") = Nombre
        BaseUser("Sexo") = IIf(m_Sexo = Femenino, "Femenino", "Masculino")
        'Leer los datos, si hay...
        If Len(Dir$(sFic)) Then
            nFic = FreeFile
            Open sFic For Input As nFic
            Do While Not EOF(nFic)
                Line Input #nFic, sTmp
                sTmp = Trim$(sTmp)
                If Len(sTmp) > 0 And Left$(sTmp, 1) <> ";" Then
                    i = InStr(sTmp, "=")
                    If i Then
                        sClave = RTrim$(Left$(sTmp, i - 1))
                        sTmp = LTrim$(Mid$(sTmp, i + 1))
                        BaseUser(sClave) = sTmp
                    End If
                End If
            Loop
            Close nFic
        End If
    Else
        'Guardar los datos
        nFic = FreeFile
        Open sFic For Output As nFic
        For i = 1 To BaseUser.Count
            With BaseUser(i)
                Print #nFic, .ID & "=" & .Contenido
            End With
        Next
        Close nFic
    End If
End Sub

Private Function ValidarDatosParaBase(ByVal sEntrada As String) As String
    ' Se validará la respuesta del usuario a una pregunta para
    ' añadir a la base de datos.
    Dim vArray As Variant
    Dim i As Long
    Dim hallado As Boolean
    '
    Select Case sUsarBaseDatos
    Case "signo_zodiaco"
        vArray = Array(" aries", " géminis", " geminis", " tauro", " cáncer", " cancer", " leo", " virgo", " libra", " scorpio", " escorpión", " escorpio", " escorpion", " sagitario", " capricornio", " acuario", " piscis")
        hallado = False
        For i = 0 To UBound(vArray)
            If InStr(sEntrada, vArray(i)) Then
                BaseUser(sUsarBaseDatos) = LTrim$(vArray(i))
                hallado = True
                Exit For
            End If
        Next
        ' Se puede devolver esto como respuesta
        If Not hallado Then
            sEntrada = "Creo que no has usado un signo del zodíaco..."
        End If
    Case "edad"
        i = Val(sEntrada)
        If i = 0 Then
            sEntrada = "Por favor indica la edad con números, gracias."
        ElseIf i < 1 Then
            sEntrada = "¿Es que aún no has nacido? :-)"
        End If
    'Case "nombre_pareja"
    '    BaseUser(sUsarBaseDatos) = sEntrada
    Case Else
        BaseUser(sUsarBaseDatos) = sEntrada
    End Select
    ' Guardar los datos
    DatosUsuario False
    '
    ValidarDatosParaBase = sEntrada
End Function

Private Function AppPath() As String
    ' Devuelve el path del ejecutable con la barra final            (15/Sep/02)
    AppPath = App.Path & IIf(Right$(App.Path, 1) = "\", "", "\")
End Function

