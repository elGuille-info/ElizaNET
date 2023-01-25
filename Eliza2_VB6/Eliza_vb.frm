VERSION 5.00
Begin VB.Form fEliza 
   Caption         =   "Eliza para Visual Basic (c)Guillermo 'guille' Som, 1998"
   ClientHeight    =   6045
   ClientLeft      =   60
   ClientTop       =   630
   ClientWidth     =   9345
   Icon            =   "Eliza_vb.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6045
   ScaleWidth      =   9345
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtSalida 
      BackColor       =   &H00000000&
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "Fixedsys"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H0000FF00&
      Height          =   4815
      Left            =   60
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   4
      Text            =   "Eliza_vb.frx":030A
      Top             =   60
      Width           =   9195
   End
   Begin VB.TextBox txtEntrada 
      BackColor       =   &H00000000&
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "Fixedsys"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H0000FF00&
      Height          =   555
      Left            =   60
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   3
      Text            =   "Eliza_vb.frx":0316
      Top             =   4890
      Width           =   9195
   End
   Begin VB.ListBox List1 
      Height          =   450
      ItemData        =   "Eliza_vb.frx":0321
      Left            =   2670
      List            =   "Eliza_vb.frx":0328
      TabIndex        =   2
      Top             =   5640
      Visible         =   0   'False
      Width           =   1695
   End
   Begin VB.ListBox List2 
      Height          =   450
      ItemData        =   "Eliza_vb.frx":033E
      Left            =   1020
      List            =   "Eliza_vb.frx":0345
      TabIndex        =   1
      Top             =   5670
      Visible         =   0   'False
      Width           =   1545
   End
   Begin VB.CommandButton cmdNuevo 
      Caption         =   "Iniciar una nueva sesi�n"
      Height          =   405
      Left            =   6960
      TabIndex        =   0
      Top             =   5610
      Width           =   2325
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&Configuraci�n"
      Begin VB.Menu mnuFileReleer 
         Caption         =   "&Releer el fichero actual"
         Shortcut        =   ^R
      End
      Begin VB.Menu mnuFileSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEstad�sticas 
         Caption         =   "&Estad�sticas"
         Shortcut        =   ^E
      End
      Begin VB.Menu mnuFileSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEliza_claves 
         Caption         =   "&Formulario de consulta..."
         Shortcut        =   ^F
      End
      Begin VB.Menu mnuFileSep3 
         Caption         =   "-"
      End
      Begin VB.Menu mnuAcercaDe 
         Caption         =   "&Acerca de..."
         Shortcut        =   {F1}
      End
      Begin VB.Menu mnuFileSep5 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSalir 
         Caption         =   "&Salir"
      End
   End
End
Attribute VB_Name = "fEliza"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' Eliza para Visual Basic
'
' Primer intento:    S�bado, 30/May/98 17:30
'
' La idea original de Eliza data del a�o 1966 y su autor fue Joseph Weizenbaum
'
' Formato de las reglas y simplificaci�n de entradas basadas en
'   ELIZA in Prolog de Viren Patel
'
' Gracias a Svetlana por proporcionarme esta direcci�n de Internet,
' ya que no encontr� el libro en el que vi el primer c�digo para
' hacer un programa en BASIC sobre ELIZA.
' El autor era: Tim Hartnell y lo public� Anaya Multimedia en la
' serie "Microinform�tica", el t�tulo del libro es:
' Inteligencia artificial: conceptos y programas.
'
' �Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
' Modificaciones y cambios:
'
' Quito la opci�n del idioma                                        (08/Jun/98)
'
' En la revisi�n 00.11.08 a�ado lo de poder partir la palabra
' de forma adecuada cuando se muestran m�s caracteres de la cuenta.
'
' Revisi�n:  00.14.00                                               (27/Jun/98)
'   Con la opci�n de "decir" (o hablar) lo que escribe...
' Revisi�n:  00.15.00                                               (06/Dic/99)
'   Usando Direct Speech Synthesis Control (DirectSS)
' Revisi�n:  00.16.00                                               (15/Sep/02)
'   Quito la opci�n de Speech, ya que aunque ha funcionado en Windows XP,
'   me ha dado alg�n que otro problemilla...
'   Quito la pantalla con scroll y utilizo dos TextBoxes.
' Revisi�n: 00.17.00                                                (17/Sep/02)
'   La base de datos de palabras/respuestas pueden estar en varios ficheros
'   que empezar�n por ElizaVB_xxx y tendr�n la extensi�n .txt
'   El fichero que se ha usado hasta ahora, se llamar� ElizaVB_SPA.txt
'------------------------------------------------------------------------------
'   POR HACER:
'   La intenci�n es crear una base de datos del tipo Access,
'   para guardar las palabras y acceder a ella en lugar de usar las clases,
'   ya que al tener que estar en la memoria, tarda mucho en cargar.
'   He compilado en c�digo nativo y el ejecutable va algo m�s r�pido.
'   (en el entorno tardaba 7 seg y compilado 1 seg.)
'------------------------------------------------------------------------------
Option Explicit
Option Compare Text

Private sNombre As String
Private m_Sexo As eSexo

Private WithEvents Eliza As cEliza
Attribute Eliza.VB_VarHelpID = -1
Private m_Terminado As Boolean
'Private m_Idioma As eIdioma

Private SesionGuardada As Boolean

Private Sub cmdNuevo_Click()
    Dim tSexo As eSexo
    Dim sSexo As String
    Dim i As Long
    Dim tmpSexo As Long
    Dim tmpNombre As String
    Dim sMsgTmp As String
    '
    ' Si no se ha guardado la sesi�n anterior, preguntar si se quiere guardar.
    If Not SesionGuardada Then
        If MsgBox(sNombre & " �Quieres guardar el contenido de la sesi�n actual?", vbYesNo, "Guardar la sesi�n actual") = vbYes Then
            GuardarSesion
        End If
    End If
    SesionGuardada = True
    '
    txtSalida.Text = ""
    txtEntrada.Text = "> "
    '
    sMsgTmp = ""
    ' Preguntar el nombre y el sexo
    Do
        sNombre = InputBox("Por favor dime tu nombre, o la forma en que quieres que te llame, (deja la respuesta en blanco para terminar)", "", sNombre)
        sNombre = Trim$(sNombre)
        If Len(sNombre) = 0 Then
            m_Terminado = False
            sMsgTmp = "Adios, hasta la pr�xima sesi�n."
            'Unload Me
            'End
            Exit Do
        End If
        ' Comprobar si est� en la lista
        tmpSexo = -1
        With List2
            For i = 0 To .ListCount - 1
                If .List(i) = sNombre Then
                    tmpSexo = .ItemData(i)
                    Exit For
                End If
            Next
        End With
        If tmpSexo > -1 Then
            'MsgBox "Hola de nuevo " & sNombre & ", empezamos cuando quieras."
            'ImprimirDOS "Hola de nuevo " & sNombre & ", empezamos cuando quieras."
            tSexo = tmpSexo
            Exit Do
        End If
        ' tener una serie de nombres para no pecar de "tonto"
        sSexo = "masculino"
        tSexo = Masculino
        If Len(sNombre) Then
            tSexo = SexoNombre()
            sSexo = IIf(tSexo = Femenino, "femenino", "masculino")
            If tSexo = eSexo.Ninguno Then
                If Right$(sNombre, 1) = "a" Then
                    sSexo = "femenino"
                    tSexo = Femenino
                Else
                    sSexo = "masculino"
                    tSexo = Masculino
                End If
                If MsgBox(sNombre & " por favor confirmame que tu sexo es: " & sSexo, vbYesNo) = vbNo Then
                    If tSexo = Femenino Then
                        tSexo = Masculino
                        sSexo = "masculino"
                    Else
                        tSexo = Femenino
                        sSexo = "femenino"
                    End If
                End If
            End If
        End If
        If MsgBox("Por favor confirma que estos datos son correctos:" & vbCrLf & "Nombre: " & sNombre & vbCrLf & "Sexo: " & sSexo, vbYesNoCancel) <> vbNo Then
            Exit Do
        End If
    Loop
    ' Si no se escribe el nombre, terminar el programa
    If Len(sNombre) = 0 Then
        If Len(sMsgTmp) = 0 Then
            sMsgTmp = "Me parece que no conf�as lo suficiente en m�, as� que no hay sesi�n que valga, �ea!"
        End If
        MsgBox sMsgTmp
        Unload Me
        End
    End If
    '
    ' Comprobar si est� en la lista
    tmpSexo = -1
    With List2
        For i = 0 To .ListCount - 1
            If .List(i) = sNombre Then
                tmpSexo = i
                Exit For
            End If
        Next
    End With
    If tmpSexo = -1 Then
        'a�adirlo
        With List2
            .AddItem sNombre
            .ItemData(.NewIndex) = tSexo
        End With
        'guardar los nombres
        GuardarNombres
    End If
    '
    List1.Clear
    List1.AddItem "Sesi�n iniciada el: " & Format$(Now, "dddd, dd/mmm/yyyy hh:mm")
    List1.AddItem "-----------------------------------------------"
    '
    sMsgTmp = "Hola " & sNombre & ", soy Eliza para Visual Basic"
    ImprimirDOS sMsgTmp
    sMsgTmp = "Por favor, intenta evitar los monos�labos y tuteame, yo as� lo har�."
    ImprimirDOS sMsgTmp
    '
    MousePointer = vbHourglass
    '
    'Inicializar los valores, mientras el usuario escribe
    With Eliza
        i = .Iniciado
        If Not i Then
            ' Si es la primera vez... dar un poco de tiempo.
            i = CInt(Rnd * 9 + 1)
            Select Case i
            Case Is > 6
                sMsgTmp = "Espera un momento, mientras ordeno mi base de datos..."
            Case Is > 3
                sMsgTmp = "Espera un momento, mientras busco un bol�grafo..."
            Case Else
                sMsgTmp = "Espera un momento, mientras lleno mis chips de conocimiento..."
            End Select
            ImprimirDOS sMsgTmp
        End If
        .Sexo = tSexo
        .Nombre = sNombre
'        tTimer.StartTimer
        .Inicializar
'        tTimer.StopTimer
'        If Not i Then
'            sMsgTmp = "Tiempo en inicializar (y asignar las palabras): " & tTimer.ElapsedTimer & " segundos."
'            ImprimirDOS sMsgTmp
'        Else
'            sMsgTmp = "Tiempo en re-inicializar las palabras: " & tTimer.ElapsedTimer & " segundos."
'            ImprimirDOS sMsgTmp
'        End If
    End With
    MousePointer = vbDefault
    '
    txtEntrada.Text = "> "
    txtEntrada.SelStart = 2
    '
    sMsgTmp = "Vamos a ello..., �en qu� puedo ayudarte?"
    ImprimirDOS sMsgTmp
    If txtEntrada.Visible Then
        txtEntrada.SetFocus
    End If
    '
'    Set tTimer = Nothing
End Sub

Private Sub Eliza_Terminado()
    'El usuario ha decidido terminar
    m_Terminado = True
End Sub

Private Sub Form_Load()
    ' Inicializar las variables
    '
    txtSalida = ""
    With txtEntrada
        .Text = ""
        .TabIndex = 0
        .Left = txtSalida.Left
    End With
    '
    SesionGuardada = True
    '
    List2.Clear
    '
    If Year(Now) > 2002 Then
        Caption = "Eliza para Visual Basic � Guillermo 'guille' Som, 1998-" & CStr(Year(Now))
    Else
        Caption = "Eliza para Visual Basic � Guillermo 'guille' Som, 1998-2002"
    End If
    '
    Show
    '
    Set Eliza = New cEliza
    Eliza.Sexo = eSexo.Ninguno
    ' Centrar manualmente, ya que si tarda un poco al Inicializar
    ' se nota que se "posiciona"
    Move (Screen.Width - Width) \ 2, (Screen.Height - Height) \ 2
    '
    ' leer la lista de nombres que han usado el programa
    LeerNombres
    ' Empezar una nueva sesi�n
    cmdNuevo_Click
End Sub

Private Sub Form_Resize()
    If WindowState <> vbMinimized Then
        ' posicionar el command y las cajas de texto
        With cmdNuevo
            .Top = ScaleHeight - (.Height + 120) '540
            .Left = ScaleWidth - .Width - 120
            '
            txtEntrada.Top = .Top - (txtEntrada.Height + 90)
            txtEntrada.Width = ScaleWidth - txtEntrada.Left - 60
            '
            txtSalida.Height = txtEntrada.Top - txtSalida.Top - 30
            txtSalida.Width = txtEntrada.Width
        End With
    End If
End Sub

Private Sub mnuEliza_claves_Click()
    Hide
    With Eliza_claves
        Set .Eliza = Eliza
        .Show vbModal
    End With
    Show
End Sub

Private Sub mnuEstad�sticas_Click()
    Dim tRespuestas As cRespuestas
    Dim tContenido As cContenido
    Dim sMsg As String
    '
    Set tRespuestas = Eliza.Estad�sticas
    sMsg = "Datos estad�sticos de Eliza para Visual Basic:" & vbCrLf & vbCrLf
    For Each tContenido In tRespuestas
        sMsg = sMsg & tContenido.ID & " = " & tContenido.Contenido & vbCrLf
    Next
    '
    MsgBox sMsg, vbInformation, "Estad�sticas de Eliza"
End Sub

Private Sub Form_Unload(Cancel As Integer)
    ' Eliminar las referencias o lo que es lo mismo: �cargarse al psiquiatra!
    Set Eliza = Nothing
    '
    Set fEliza = Nothing
End Sub

Private Sub mnuAcercaDe_Click()
    ' Mostrar la informaci�n del programa
    Dim msg As String
    '
    msg = "Eliza para Visual Basic," & vbCrLf & " versi�n: "
    msg = msg & _
        Format$(App.Major, "00") & "." & Format$(App.Minor, "00") & "." & _
        Format$(App.Revision, "00") & ", "
    msg = msg & App.ProductName & vbCrLf & vbCrLf & _
        App.Comments & vbCrLf & vbCrLf
    msg = msg & _
        "La idea del formato de las reglas y simplificaci�n de entradas," & vbCrLf & _
        "est�n basadas en 'ELIZA in Prolog' de Viren Patel." & vbCrLf & vbCrLf
    msg = msg & _
        App.LegalCopyright & _
        ", iniciado el S�bado, 30/May/98 17:30" & vbCrLf & vbCrLf
    msg = msg & _
        "Agradecimiento especial a Svetlana por toda la informaci�n aportada," & vbCrLf & _
        "adem�s de ampliar la base de conocimientos de Eliza y darle" & vbCrLf & _
        "un toque m�s femenino del que yo jam�s le hubiese podido dar..." & vbCrLf & _
        "y, sobre todo, por motivarme a hacer este programa, sin su ayuda no hubiera sido posible..."
    '
    MsgBox msg, vbInformation, "Acerca de Eliza para VB"
End Sub

Private Sub mnuFileReleer_Click()
    ' Releer el fichero de palabras,
    ' esto es �til por si se a�aden nuevas
    MousePointer = vbHourglass
    Eliza.Releer
    MousePointer = vbDefault
End Sub

Private Sub mnuSalir_Click()
    ' Si no se ha guardado la sesi�n anterior, preguntar            (16/Sep/02)
    ' si se quiere guardar.
    If Not SesionGuardada Then
        If MsgBox(sNombre & " �Quieres guardar el contenido de la sesi�n actual?", vbYesNo, "Guardar la sesi�n actual") = vbYes Then
            GuardarSesion
        End If
    End If
    SesionGuardada = True
    '
    Unload Me
    'End
End Sub

Private Sub ProcesarEntrada()
    ' Toma lo que ha escrito el usuario y lo env�a a la clase
    ' para procesar lo escrito y obtener la respuesta
    Static sAnt As String
    Dim sTmp As String
    '
    sTmp = Trim$(txtEntrada.Text)
    ' S�lo si se ha escrito algo
    If Len(sTmp) > 0 Then
        txtEntrada.Text = "> "
        txtEntrada.SelStart = 2
        If sAnt = sTmp Then
            ImprimirDOS "Por favor, no te repitas."
        Else
            ' guardar la �ltima entrada
            sAnt = sTmp
            ' mostrar en la lista lo que se ha escrito
            ImprimirDOS sTmp
            If Left$(sTmp, 2) = "> " Then
                sTmp = Mid$(sTmp, 3)
            End If
            ' Si se escribe *consulta*, usar lo que viene despu�s
            ' para buscar en las palabras claves de Eliza.
            ' De esa comprobaci�n se encarga cEliza.ProcesarEntrada
            ' Una vez que se entra en el modo de consulta, lo que
            ' se escriba se buscar� en las claves y sub-claves,
            ' para salir del modo consulta, hay que escribir de
            ' nuevo *consulta*
            '
            ' Procesar la entrada del usuario y
            ' mostrar la respuesta de Eliza
            sTmp = Eliza.ProcesarEntrada(sTmp)
            ImprimirDOS sTmp
            '---Esto tampoco soluciona el GPF
            DoEvents
            '
            ' Si se ha producido el evento de Terminar
            If m_Terminado Then
                sAnt = ""
                SesionGuardada = False
                m_Terminado = False
                cmdNuevo_Click
            End If
        End If
    Else
        ImprimirDOS "�Te lo est�s pensando?"
    End If
    '
    SesionGuardada = False
End Sub

Private Sub GuardarSesion()
    ' preguntar el nombre del fichero
    ' o crearlo autom�ticamente
    Dim sFic As String
    Dim nFic As Long 'Integer
    Dim i As Long
    '
    sFic = AppPath & "sesiones\" & sNombre & "_" & Format$(Now, "ddmmmyyyy_hhmm") & ".txt"
    sFic = InputBox(sNombre & " escribe el nombre del fichero:", "Guardar sesi�n", sFic)
    sFic = Trim$(sFic)
    If Len(sFic) Then
        List1.AddItem "-----------------------------------------------"
        List1.AddItem "Sesi�n guardada el: " & Format$(Now, "dddd, dd/mmm/yyyy hh:mm")
        '
        ' Crear los directorios indicados en el nombre del archivo  (18/Sep/02)
        crearDirectorios sFic
        '
        nFic = FreeFile
        Open sFic For Output As nFic
        'Print #nFic, txtSalida.Text
        With List1
            For i = 0 To .ListCount - 1
                Print #nFic, .List(i)
            Next
        End With
        Close nFic
    End If
End Sub

Private Function SexoNombre() As eSexo
    ' devolver� el sexo seg�n el nombre introducido
    Dim Mujeres As Variant
    Dim Hombres As Variant
    Dim Ambos As Variant
    Dim i As Long 'Integer
    Dim hallado As Boolean
    Dim tSexo As eSexo
    '
    Mujeres = Array(" adela", " alicia", " amalia", " amanda", " " & _
            "ana", " anita", " asunci�n", " aurora", " belinda", " " & _
            "berioska", " carmen", " carmeli", " caty", " celia", " " & _
            "delia", " diana", " dolores", " elena", " elisa", " " & _
            "eva", " felisa", " gabriela", " gemma", " guillermina", " " & _
            "inma", " isa", " josef", " julia", " juana", " juanita", " " & _
            "laura", " luisa", " maite", " manoli", " manuela", " mari", " " & _
            "mar�a", " marta", " merce", " m�nica", " nadia", " " & _
            "paqui", " pepa", " rita", " rosa", " sara", " silvia", " " & _
            "sonia", " susan", " svetlana", " tere", " vane", " " & _
            "vero", " ver�nica", " vivian")
    Hombres = Array(" ad�n", " alvaro", " andr�s", " bartolo", " " & _
            "borja", " c�ndido", " carlos", " d�maso", " dami�n", " " & _
            "daniel", " dar�o", " f�lix", " gabriel", " guillermo", " " & _
            "harvey", " jaime", " javier", " joaqu�n", " joe", " jorge", " " & _
            "jose", " jos�", " juan", " luis", " manuel", " miguel", " " & _
            "pepe", " ram�n", " santiago", " tom�s")
    'masculino si acaba con 'o', femenino si acaba con 'a'
    Ambos = Array(" albert", " antoni", " armand", " bernard", " " & _
            "carmel", " dionisi", " ernest", " fernand", " " & _
            "francisc", " gerard", " ignaci", " manol", " maurici", " " & _
            "pac", " rosend", " venanci")
    '
    sNombre = " " & Trim$(sNombre)
    tSexo = eSexo.Ninguno
    For i = 1 To UBound(Mujeres)
        If InStr(sNombre, Mujeres(i)) Then
            tSexo = Femenino
            Exit For
        End If
    Next
    If tSexo = eSexo.Ninguno Then
        For i = 1 To UBound(Hombres)
            If InStr(sNombre, Hombres(i)) Then
                tSexo = Masculino
                Exit For
            End If
        Next
    End If
    If tSexo = eSexo.Ninguno Then
        For i = 1 To UBound(Ambos)
            If InStr(sNombre, Ambos(i)) Then
                If InStr(sNombre, Ambos(i) & "a") Then
                    tSexo = Femenino
                ElseIf InStr(sNombre, Ambos(i) & "o") Then
                    tSexo = Masculino
                End If
                Exit For
            End If
        Next
    End If
    sNombre = Trim$(sNombre)
    SexoNombre = tSexo
End Function

Private Sub LeerNombres()
    Dim sFic As String
    Dim nFic As String
    Dim tmpNombre As String
    Dim tmpSexo As Long
    '
    sFic = AppPath & "ListaDeNombres.txt"
    If Len(Dir$(sFic)) Then
        With List2
            .Clear
            nFic = FreeFile
            Open sFic For Input As nFic
            Do While Not EOF(nFic)
                Line Input #nFic, tmpNombre
                Input #nFic, tmpSexo
                If Len(sNombre) = 0 Then
                    sNombre = tmpNombre
                End If
                .AddItem tmpNombre
                .ItemData(.NewIndex) = tmpSexo
            Loop
        End With
        Close nFic
    End If
End Sub

Private Sub GuardarNombres()
    Dim sFic As String
    Dim nFic As String
    Dim tmpNombre As String
    Dim tmpSexo As Long
    Dim i As Long
    '
    sFic = AppPath & "ListaDeNombres.txt"
    nFic = FreeFile
    Open sFic For Output As nFic
    With List2
        For i = 0 To .ListCount - 1
            tmpNombre = .List(i)
            tmpSexo = .ItemData(i)
            Print #nFic, tmpNombre
            Print #nFic, tmpSexo
        Next
    End With
    Close nFic
End Sub

Private Sub ImprimirDOS(ByVal sText As String, Optional ByVal NuevaLinea As Boolean = True)
    ' Imprimir el texto de entrada en el TextBox de salida
    ' Si se NuevaLinea tiene un valor True (valor por defecto)
    ' lo siguiente que se imprima se har� en una nueva l�nea
    '
    Dim s As String
    '
    s = txtSalida.Text & sText
    If NuevaLinea Then
        s = s & vbCrLf
    End If
    txtSalida.Text = s
    List1.AddItem sText
    ' Posicionar el cursor al final de la caja de texto
    txtSalida.SelStart = Len(s)
    txtSalida.SelLength = 0
    '
End Sub

Private Sub txtEntrada_KeyPress(KeyAscii As Integer)
    ' Cuando se pulse INTRO se procesar� la entrada
    ' Se llamar� a un procedimiento separado
    If KeyAscii = vbKeyReturn Then
        ' Evitar el pitido
        KeyAscii = 0
        ProcesarEntrada
    End If
End Sub

Private Function AppPath() As String
    ' Devuelve el path del ejecutable con la barra final            (15/Sep/02)
    AppPath = App.Path & IIf(Right$(App.Path, 1) = "\", "", "\")
End Function

Private Sub crearDirectorios(ByVal sFic As String)
    ' Crear los directorios indicados en el nombre del archivo      (18/Sep/02)
    ' se crear�n todos los niveles indicados
    Dim sDir As String, sDir2 As String
    Dim i As Long
    Dim b As Boolean
    '
    i = InStrRev(sFic, "\")
    If i > 0 Then
        sDir = Left$(sFic, i)
        If Left$(sFic, 2) = "\\" Then
            i = 3
        Else
            i = 1
        End If
        '
        On Local Error Resume Next
        '
        ' No tener en cuenta el primer path
        i = InStr(i, sDir, "\")
        ' Recorrer todos los paths indicados en el directorio
        Do
            i = InStr(i + 1, sDir, "\")
            If i = 0 Then Exit Do
            ' Comprobar si existe, se producir� un error si no existe.
            sDir2 = Left$(sDir, i - 1)
            Err.Number = 0
            b = ((GetAttr(sDir2) And vbDirectory) = vbDirectory)
            If Err > 0 Then
                b = False
            End If
            If Not b Then
                Err.Number = 0
                MkDir sDir2
                ' Si se produce un error aqu�, es cuesti�n de avisarlo
                If Err.Number > 0 Then
                    VBA.MsgBox "ERROR al crear el directorio: " & sDir2
                End If
            End If
        Loop While i > 0
    End If
End Sub
