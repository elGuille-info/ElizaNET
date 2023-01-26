'------------------------------------------------------------------------------
' Eliza para Visual Basic
'
' Primer intento:    Sábado, 30/May/98 17:30
'
' La idea original de Eliza data del año 1966 y su autor fue Joseph Weizenbaum
'
' Formato de las reglas y simplificación de entradas basadas en
'   ELIZA in Prolog de Viren Patel
'
' Gracias a Svetlana por proporcionarme esta dirección de Internet,
' ya que no encontré el libro en el que vi el primer código para
' hacer un programa en BASIC sobre ELIZA.
' El autor era: Tim Hartnell y lo publicó Anaya Multimedia en la
' serie "Microinformática", el título del libro es:
' Inteligencia artificial: conceptos y programas.
'
' ©Guillermo 'guille' Som, 1998-2002, 2023
'------------------------------------------------------------------------------
' Modificaciones y cambios:
'
' Quito la opción del idioma                                        (08/Jun/98)
'
' En la revisión 00.11.08 añado lo de poder partir la palabra
' de forma adecuada cuando se muestran más caracteres de la cuenta.
'
' Revisión:  00.14.00                                               (27/Jun/98)
'   Con la opción de "decir" (o hablar) lo que escribe...
' Revisión:  00.15.00                                               (06/Dic/99)
'   Usando Direct Speech Synthesis Control (DirectSS)
' Revisión:  00.16.00                                               (15/Sep/02)
'   Quito la opción de Speech, ya que aunque ha funcionado en Windows XP,
'   me ha dado algún que otro problemilla...
'   Quito la pantalla con scroll y utilizo dos TextBoxes.
' Revisión: 00.17.00                                                (17/Sep/02)
'   La base de datos de palabras/respuestas pueden estar en varios ficheros
'   que empezarán por ElizaVB_xxx y tendrán la extensión .txt
'   El fichero que se ha usado hasta ahora, se llamará ElizaVB_SPA.txt
'------------------------------------------------------------------------------
'   POR HACER:
'   La intención es crear una base de datos del tipo Access,
'   para guardar las palabras y acceder a ella en lugar de usar las clases,
'   ya que al tener que estar en la memoria, tarda mucho en cargar.
'   He compilado en código nativo y el ejecutable va algo más rápido.
'   (en el entorno tardaba 7 seg y compilado 1 seg.)
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On
Option Compare Text

Imports ElizaVB

Friend Class fEliza
    Inherits System.Windows.Forms.Form

    Private sNombre As String = "" ' asignarle una cadena vacía, 24-ene-2023 12.24
    'Private m_Sexo As cEliza.eSexo

    'Private WithEvents Eliza As cEliza
    Private Eliza As cEliza
    Private m_Terminado As Boolean
    'Private m_Idioma As eIdioma

    Private SesionGuardada As Boolean
    Private sEntradaAnterior As String

    Private Sub cmdNuevo_Click(sender As Object, e As EventArgs) Handles cmdNuevo.Click
        Dim tSexo As cEliza.eSexo
        Dim sSexo As String
        Dim tmpSexo As Integer
        Dim sMsgTmp As String
        Dim m_rnd As New Random()

        ' Si no se ha guardado la sesión anterior, preguntar si se quiere guardar.
        If Not SesionGuardada Then
            If Dialogos.MessageBoxShow(sNombre & " ¿Quieres guardar el contenido de la sesión actual?",
                      "Guardar la sesión actual", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                GuardarSesion()
            End If

        End If
        SesionGuardada = True

        txtSalida.Text = ""
        txtEntrada.Text = "> "

        sMsgTmp = ""
        ' Preguntar el nombre y el sexo
        Do
            If UtilidadesDialog.UtilDialog.InputBox("Por favor dime tu nombre, o la forma en que quieres que te llame, (deja la respuesta en blanco para terminar)",
                                                    "Saber quién eres", sNombre) <> DialogResult.OK Then
                sNombre = ""
            End If
            sNombre = sNombre.Trim()
            If sNombre.Length = 0 Then
                m_Terminado = False
                sMsgTmp = "Adios, hasta la próxima sesión."
                Exit Do
            End If
            ' Comprobar si está en la lista
            tmpSexo = -1
            With List2
                For i = 0 To .Items.Count - 1
                    If List2.Items(i).ToString() = sNombre Then
                        tmpSexo = i
                        Exit For
                    End If
                Next
            End With
            If tmpSexo > -1 Then
                tSexo = CType(tmpSexo, cEliza.eSexo)
                Exit Do
            End If
            ' tener una serie de nombres para no pecar de "tonto"
            sSexo = "masculino"
            tSexo = cEliza.eSexo.Masculino
            If sNombre.Length > 0 Then
                tSexo = SexoNombre()
                sSexo = If(tSexo = cEliza.eSexo.Femenino, "femenino", "masculino")
                If tSexo = cEliza.eSexo.Ninguno Then
                    If sNombre.EndsWith("a") Then
                        sSexo = "femenino"
                        tSexo = cEliza.eSexo.Femenino
                    Else
                        sSexo = "masculino"
                        tSexo = cEliza.eSexo.Masculino
                    End If
                    If Dialogos.MessageBoxShow(sNombre & " por favor confirmame que tu sexo es: " & sSexo,
                                               "", MessageBoxButtons.YesNo) = DialogResult.No Then
                        If tSexo = cEliza.eSexo.Femenino Then
                            tSexo = cEliza.eSexo.Masculino
                            sSexo = "masculino"
                        Else
                            tSexo = cEliza.eSexo.Femenino
                            sSexo = "femenino"
                        End If
                    End If
                End If
            End If
            If Dialogos.MessageBoxShow("Por favor confirma que estos datos son correctos:" & vbCrLf & "Nombre: " & sNombre & vbCrLf & "Sexo: " & sSexo,
                                       "", MessageBoxButtons.YesNoCancel) <> DialogResult.No Then
                Exit Do
            End If
        Loop
        ' Si no se escribe el nombre, terminar el programa
        If sNombre.Length = 0 Then
            If sMsgTmp.Length = 0 Then
                sMsgTmp = "Me parece que no confías lo suficiente en mí, así que no hay sesión que valga, ¡ea!"
            End If
            Dialogos.MessageBoxShow(sMsgTmp, "", MessageBoxButtons.OK)
            Close()
            Return
            'End
        End If

        ' Comprobar si está en la lista
        tmpSexo = -1
        With List2
            For i = 0 To .Items.Count - 1
                If List2.Items(i).ToString() = sNombre Then
                    tmpSexo = i
                    Exit For
                End If
            Next
        End With
        If tmpSexo = -1 Then
            'añadirlo
            With List2
                .Items.Add(sNombre)
            End With
            'guardar los nombres
            GuardarNombres()
        End If

        List1.Items.Clear()
        List1.Items.Add($"Sesión iniciada el: {Date.Now:dddd, dd/MMM/yyyy HH:mm}")
        List1.Items.Add("-----------------------------------------------")

        sMsgTmp = "Hola " & sNombre & ", soy Eliza para Visual Basic"
        ImprimirDOS(sMsgTmp)
        sMsgTmp = "Por favor, intenta evitar los monosílabos y tuteame, yo así lo haré."
        ImprimirDOS(sMsgTmp)

        Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Inicializar los valores, mientras el usuario escribe
        With Eliza
            If .Iniciado Then
                ' Si es la primera vez... dar un poco de tiempo.
                Select Case m_rnd.Next(10)
                    Case Is > 6
                        sMsgTmp = "Espera un momento, mientras ordeno mi base de datos..."
                    Case Is > 3
                        sMsgTmp = "Espera un momento, mientras busco un bolígrafo..."
                    Case Else
                        sMsgTmp = "Espera un momento, mientras lleno mis chips de conocimiento..."
                End Select
                ImprimirDOS(sMsgTmp)
            End If
            .Sexo = tSexo
            .Nombre = sNombre
            Dim sw = Stopwatch.StartNew()
            .Inicializar()
            sw.Stop()
            If Not .Iniciado Then
                sMsgTmp = "Tiempo en inicializar (y asignar las palabras): " & sw.Elapsed.ToString("mm\:ss\.fff")
                ImprimirDOS(sMsgTmp)
            Else
                sMsgTmp = "Tiempo en re-inicializar las palabras: " & sw.Elapsed.ToString("mm\:ss\.fff")
                ImprimirDOS(sMsgTmp)
            End If
        End With
        Cursor = System.Windows.Forms.Cursors.Default

        txtEntrada.Text = "> "
        txtEntrada.SelectionStart = 2

        sMsgTmp = "Vamos a ello..., ¿en qué puedo ayudarte?"
        ImprimirDOS(sMsgTmp)
        If txtEntrada.Visible Then
            txtEntrada.Focus()
        End If
    End Sub

    'Private Sub Eliza_Terminado() Handles Eliza.Terminado
    '    'El usuario ha decidido terminar
    '    m_Terminado = True
    'End Sub

    Private Sub fEliza_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 300
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Inicializar()
    End Sub

    Private Sub Inicializar()
        ' Inicializar las variables
        txtSalida.Text = ""
        With txtEntrada
            .Text = ""
            .TabIndex = 0
            .Left = txtSalida.Left
        End With

        SesionGuardada = True

        List2.Items.Clear()

        If Date.Now.Year > 2023 Then
            LabelInfo.Text = "Eliza para Visual Basic ©Guillermo Som (Guille), 1998-2002, 2023-" & Date.Now.Year.ToString()
        Else
            LabelInfo.Text = "Eliza para Visual Basic ©Guillermo Som (Guille), 1998-2002, 2023"
        End If

        Show()

        Eliza = New cEliza(AppPath()) With {
            .Sexo = cEliza.eSexo.Ninguno
        }

        ' leer la lista de nombres que han usado el programa
        LeerNombres()
        ' Empezar una nueva sesión
        cmdNuevo_Click(cmdNuevo, New System.EventArgs())
    End Sub

    Public Sub mnuEliza_claves_Click(sender As Object, e As EventArgs) Handles mnuEliza_claves.Click
        Hide()
        Dim fClaves As New Eliza_claves(Eliza)
        fClaves.ShowDialog()
        Show()
    End Sub

    Public Sub mnuEstadísticas_Click(sender As Object, e As EventArgs) Handles mnuEstadísticas.Click
        Dim tRespuestas As cRespuestas
        Dim tContenido As cContenido
        Dim sMsg As String

        tRespuestas = Eliza.Estadísticas
        sMsg = "Datos estadísticos de Eliza para Visual Basic:" & vbCrLf & vbCrLf
        For Each tContenido In tRespuestas.Valores '.Values
            sMsg = sMsg & tContenido.ID & " = " & tContenido.Contenido & vbCrLf
        Next tContenido

        Dialogos.MessageBoxShow(sMsg, "Estadísticas de Eliza", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Sub mnuAcercaDe_Click(sender As Object, e As EventArgs) Handles mnuAcercaDe.Click
        ' Mostrar la información del programa
        Dim msg As New System.Text.StringBuilder()
        Dim ensamblado = GetType(fEliza).Assembly
        Dim fvi = FileVersionInfo.GetVersionInfo(ensamblado.Location)

        msg.Append("Eliza para Visual Basic,")
        msg.AppendLine($"versión {fvi.ProductVersion} ({fvi.FileVersion})")
        msg.AppendLine(fvi.ProductName)
        msg.AppendLine($"{fvi.LegalCopyright}")
        msg.AppendLine()
        msg.AppendLine($"{fvi.Comments}")
        msg.AppendLine("Versión para VB5    iniciada el Sábado, 30/May/1998 17:30")
        msg.AppendLine("Versión para VB6    iniciada el Miércoles, 18/Sep/2002 04:30")
        msg.AppendLine("Versión para VB.NET iniciada el Domingo, 22/Ene/2023 10:08")
        msg.AppendLine("Versión para C#     iniciada el Jueves, 26/Ene/2023 19:20")
        msg.AppendLine()
        msg.AppendLine("La idea del formato de las reglas y simplificación de entradas, están basadas en 'ELIZA in Prolog' de Viren Patel.")
        msg.AppendLine()
        msg.Append("Agradecimiento especial a Svetlana por toda la información aportada, ")
        msg.Append("además de ampliar la base de conocimientos de Eliza y darle ")
        msg.Append("un toque más femenino del que yo jamás le hubiese podido dar... ")
        msg.AppendLine("y, sobre todo, por motivarme a hacer este programa, sin su ayuda no hubiera sido posible...")

        Dialogos.MessageBoxShow(msg.ToString(), "Acerca de Eliza para VB", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Sub mnuFileReleer_Click(sender As Object, e As EventArgs) Handles mnuFileReleer.Click
        ' Releer el fichero de palabras,
        ' esto es útil por si se añaden nuevas
        Cursor = System.Windows.Forms.Cursors.WaitCursor
        Eliza.Releer()
        Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Public Sub mnuSalir_Click(sender As Object, e As EventArgs) Handles mnuSalir.Click
        ' Si no se ha guardado la sesión anterior, preguntar            (16/Sep/02)
        ' si se quiere guardar.
        If Not SesionGuardada Then
            If Dialogos.MessageBoxShow(sNombre & " ¿Quieres guardar el contenido de la sesión actual?",
                                       "Guardar la sesión actual", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                GuardarSesion()
            End If
        End If
        SesionGuardada = True

        Me.Close()
    End Sub

    Private Sub ProcesarEntrada()
        ' Toma lo que ha escrito el usuario y lo envía a la clase
        ' para procesar lo escrito y obtener la respuesta
        Dim sTmp As String

        If String.IsNullOrEmpty(txtEntrada.Text) Then
            sTmp = ""
        Else
            sTmp = txtEntrada.Text.Trim()
        End If

        ' Sólo si se ha escrito algo
        If String.IsNullOrEmpty(sTmp) = False Then
            txtEntrada.Text = "> "
            txtEntrada.SelectionStart = 2

            Dim noRepitas As Boolean
            If sEntradaAnterior = sTmp Then noRepitas = True

            ' Mostrar el aviso que no te repitas si no es no, sí, etc.

            ' Las palabras que se pueden repetir están en Eliza.SiNo
            ' Simplificando:
            '	si la respuesta tiene menos de 5 caracteres no considerarlo repetición
            If sTmp.Length < 5 Then
                noRepitas = False
            End If
            'If sAnt = sTmp Then
            If noRepitas Then
                ImprimirDOS("Por favor, no te repitas.")
            Else
                ' guardar la última entrada
                sEntradaAnterior = sTmp
                ' mostrar en la lista lo que se ha escrito
                ImprimirDOS(sTmp)
                If sTmp.StartsWith("> ") Then
                    sTmp = sTmp.Substring(2).TrimStart()
                End If
                ' si se escribe ?, -?, --?, -h o --help			(24/ene/23 17.58)
                ' mostrar la ayuda de comandos en principio ponerlo en modo consulta.
                If {"?", "-?", "--?", "-h", "--h", "--help", "-help"}.Contains(sTmp) Then
                    ImprimirDOS("No hay ayuda definida, puedes usar *consulta* para buscar en las palabras clave.")
                    Return
                End If
                ' Si se escribe *consulta*, usar lo que viene después
                ' para buscar en las palabras claves de Eliza.
                ' De esa comprobación se encarga cEliza.ProcesarEntrada
                ' Una vez que se entra en el modo de consulta, lo que
                ' se escriba se buscará en las claves y sub-claves,
                ' para salir del modo consulta, hay que escribir de
                ' nuevo *consulta*
                '
                ' Procesar la entrada del usuario y
                ' mostrar la respuesta de Eliza
                sTmp = Eliza.ProcesarEntrada(sTmp)
                ImprimirDOS(sTmp)
                '---Esto tampoco soluciona el GPF
                Application.DoEvents()

                If sTmp.StartsWith("adios", StringComparison.OrdinalIgnoreCase) Then
                    sEntradaAnterior = ""
                    SesionGuardada = False
                    m_Terminado = False
                    cmdNuevo_Click(cmdNuevo, New System.EventArgs())
                End If
                '' Si se ha producido el evento de Terminar
                'If m_Terminado Then
                '    sEntradaAnterior = ""
                '    SesionGuardada = False
                '    m_Terminado = False
                '    cmdNuevo_Click(cmdNuevo, New System.EventArgs())
                'End If
            End If
        Else
            ImprimirDOS("¿Te lo estás pensando?")
        End If

        SesionGuardada = False
    End Sub

    Private Sub GuardarSesion()
        ' preguntar el nombre del fichero
        ' o crearlo automáticamente

        Dim sDir = System.IO.Path.Combine(AppPath(), "sesiones")
        Dim sFic = System.IO.Path.Combine(sDir, sNombre & "_" & Date.Now.ToString("ddMMMyyyy_HHmm") & ".txt")
        If UtilidadesDialog.UtilDialog.InputBox(sNombre & " escribe el nombre del fichero:", "Guardar sesión", sFic) <> DialogResult.OK Then
            sFic = ""
        Else
            sFic = sFic.Trim()
        End If
        If String.IsNullOrEmpty(sFic) = False Then
            List1.Items.Add("-----------------------------------------------")
            List1.Items.Add($"Sesión guardada el: {Date.Now.ToString("dddd, dd/MMM/yyyy HH:mm")}")

            ' Crear los directorios indicados en el nombre del archivo  (18/Sep/02)
            If System.IO.Directory.Exists(sDir) = False Then
                System.IO.Directory.CreateDirectory(sDir)
            End If

            Using sw As New System.IO.StreamWriter(sFic, False, System.Text.Encoding.UTF8)
                With List1
                    For i = 0 To .Items.Count - 1
                        sw.WriteLine(.Items(i).ToString())
                    Next
                End With
            End Using
        End If
    End Sub

    Private Function SexoNombre() As cEliza.eSexo
        ' devolverá el sexo según el nombre introducido
        Dim i As Integer
        Dim tSexo As cEliza.eSexo

        Dim Mujeres = {" adela", " alicia", " amalia", " amanda", " " & "ana", " anita", " asunción", " aurora", " belinda", " " & "berioska", " carmen", " carmeli", " caty", " celia", " " & "delia", " diana", " dolores", " elena", " elisa", " " & "eva", " felisa", " gabriela", " gemma", " guillermina", " " & "inma", " isa", " josef", " julia", " juana", " juanita", " " & "laura", " luisa", " maite", " manoli", " manuela", " mari", " " & "maría", " marta", " merce", " mónica", " nadia", " " & "paqui", " pepa", " rita", " rosa", " sara", " silvia", " " & "sonia", " susan", " svetlana", " tere", " vane", " " & "vero", " verónica", " vivian"}
        Dim Hombres = {" adán", " alvaro", " andrés", " bartolo", " " & "borja", " cándido", " carlos", " dámaso", " damián", " " & "daniel", " darío", " félix", " gabriel", " guillermo", " " & "harvey", " jaime", " javier", " joaquín", " joe", " jorge", " " & "jose", " josé", " juan", " luis", " manuel", " miguel", " " & "pepe", " ramón", " santiago", " tomás"}
        'masculino si acaba con 'o', femenino si acaba con 'a'
        Dim Ambos = {" albert", " antoni", " armand", " bernard", " " & "carmel", " dionisi", " ernest", " fernand", " " & "francisc", " gerard", " ignaci", " manol", " maurici", " " & "pac", " rosend", " venanci"}

        sNombre = " " & sNombre.Trim()
        tSexo = cEliza.eSexo.Ninguno
        For i = 0 To Mujeres.Length - 1
            If sNombre.IndexOf(Mujeres(i)) > -1 Then
                tSexo = cEliza.eSexo.Femenino
                Exit For
            End If
        Next
        If tSexo = cEliza.eSexo.Ninguno Then
            For i = 0 To Hombres.Length - 1
                If sNombre.IndexOf(Hombres(i)) > -1 Then
                    tSexo = cEliza.eSexo.Masculino
                    Exit For
                End If
            Next
        End If
        If tSexo = cEliza.eSexo.Ninguno Then
            For i = 0 To Ambos.Length - 1
                If sNombre.IndexOf(Ambos(i)) > -1 Then
                    If sNombre.IndexOf(Ambos(i) & "a") > -1 Then
                        tSexo = cEliza.eSexo.Femenino
                    ElseIf sNombre.IndexOf(Ambos(i) & "o") > -1 Then
                        tSexo = cEliza.eSexo.Masculino
                    End If
                    Exit For
                End If
            Next
        End If
        sNombre = sNombre.Trim()
        Return tSexo
    End Function

    Private Sub LeerNombres()
        Dim sFic As String
        Dim tmpNombre As String

        sFic = AppPath() & "ListaDeNombres.txt"
        If System.IO.File.Exists(sFic) Then
            With List2
                .Items.Clear()
                Using sr As New System.IO.StreamReader(sFic, System.Text.Encoding.UTF8, True)
                    Do While Not sr.EndOfStream
                        tmpNombre = sr.ReadLine().Trim()
                        ' Da error si no está inicializado sNombre, 24-ene-2023 12.24
                        If sNombre.Length = 0 Then
                            sNombre = tmpNombre
                        End If
                        .Items.Add(tmpNombre)
                        tmpNombre = sr.ReadLine()
                    Loop
                End Using
            End With
        End If
    End Sub

    Private Sub GuardarNombres()
        Dim sFic As String

        sFic = AppPath() & "ListaDeNombres.txt"
        Using sw As New System.IO.StreamWriter(sFic, False, System.Text.Encoding.UTF8)
            For i = 0 To List2.Items.Count - 1
                sw.WriteLine(List2.Items(i).ToString)
                sw.WriteLine("0") ' el sexo
            Next
        End Using
    End Sub

    Private Sub ImprimirDOS(sText As String, Optional NuevaLinea As Boolean = True)
        ' Imprimir el texto de entrada en el TextBox de salida
        ' Si se NuevaLinea tiene un valor True (valor por defecto)
        ' lo siguiente que se imprima se hará en una nueva línea

        Dim s As String

        s = txtSalida.Text & sText
        If NuevaLinea Then
            s &= vbCrLf
        End If
        txtSalida.Text = s
        List1.Items.Add(sText)
        ' Posicionar el cursor al final de la caja de texto
        txtSalida.SelectionStart = s.Length
        txtSalida.SelectionLength = 0
        txtSalida.ScrollToCaret()

    End Sub

    Public Shared Function AppPath() As String
        ' Devuelve el path del ejecutable con la barra final            (15/Sep/02)
        Dim ensamblado = GetType(fEliza).Assembly
        Dim elPath = System.IO.Path.GetDirectoryName(ensamblado.Location)
        Return elPath & If(elPath.EndsWith("\"), "", "\")
    End Function

    ' Comprobar si se ha pulsado la tecla "arriba",				(24/ene/23 13.16)
    ' si es así poner el texto anterior.

    Private Sub txtEntrada_KeyUp(sender As Object, e As KeyEventArgs) Handles txtEntrada.KeyUp
        If e.KeyData = Keys.Return Then
            e.Handled = True
            txtEntrada.Text = txtEntrada.Text.Replace(vbCrLf, "")
            ProcesarEntrada()
        ElseIf e.KeyData = Keys.Up Then
            txtEntrada.Text = sEntradaAnterior
            txtEntrada.SelectionStart = txtEntrada.Text.Length
            txtEntrada.SelectionLength = 0
            e.Handled = True
        End If
    End Sub
End Class