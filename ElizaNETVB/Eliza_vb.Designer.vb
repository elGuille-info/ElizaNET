<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class fEliza
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents mnuTextoAnalizado As System.Windows.Forms.ToolStripMenuItem
    Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileReleer As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileSep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuEstadísticas As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileSep2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuEliza_claves As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileSep3 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuAcercaDe As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileSep5 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuSalir As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fEliza))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTextoAnalizado = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileReleer = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuEstadísticas = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuEliza_claves = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAcercaDe = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSep5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSalir = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtSalida = New System.Windows.Forms.TextBox()
        Me.txtEntrada = New System.Windows.Forms.TextBox()
        Me.cmdNuevo = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.LabelInfo = New System.Windows.Forms.Label()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(1002, 33)
        Me.MainMenu1.TabIndex = 5
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileReleer, Me.mnuFileSep1, Me.mnuEstadísticas, Me.mnuFileSep2, Me.mnuEliza_claves, Me.mnuFileSep3, Me.mnuTextoAnalizado, Me.toolStripSeparator1, Me.mnuAcercaDe, Me.mnuFileSep5, Me.mnuSalir})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(139, 29)
        Me.mnuFile.Text = "&Configuración"
        '
        'mnuFileReleer
        '
        Me.mnuFileReleer.Name = "mnuFileReleer"
        Me.mnuFileReleer.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.mnuFileReleer.Size = New System.Drawing.Size(368, 34)
        Me.mnuFileReleer.Text = "&Releer el fichero actual"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Name = "mnuFileSep1"
        Me.mnuFileSep1.Size = New System.Drawing.Size(365, 6)
        '
        'mnuEstadísticas
        '
        Me.mnuEstadísticas.Name = "mnuEstadísticas"
        Me.mnuEstadísticas.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEstadísticas.Size = New System.Drawing.Size(368, 34)
        Me.mnuEstadísticas.Text = "&Estadísticas"
        '
        'mnuFileSep2
        '
        Me.mnuFileSep2.Name = "mnuFileSep2"
        Me.mnuFileSep2.Size = New System.Drawing.Size(365, 6)
        '
        'mnuEliza_claves
        '
        Me.mnuEliza_claves.Name = "mnuEliza_claves"
        Me.mnuEliza_claves.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuEliza_claves.Size = New System.Drawing.Size(368, 34)
        Me.mnuEliza_claves.Text = "&Formulario de consulta..."
        '
        'mnuFileSep3
        '
        Me.mnuFileSep3.Name = "mnuFileSep3"
        Me.mnuFileSep3.Size = New System.Drawing.Size(365, 6)
        '
        'mnuTextoAnalizado
        '
        Me.mnuTextoAnalizado.Name = "mnuTextoAnalizado"
        Me.mnuTextoAnalizado.Size = New System.Drawing.Size(368, 34)
        Me.mnuTextoAnalizado.Text = "Mostrar texto analizado..."
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(365, 6)
        '
        'mnuAcercaDe
        '
        Me.mnuAcercaDe.Name = "mnuAcercaDe"
        Me.mnuAcercaDe.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mnuAcercaDe.Size = New System.Drawing.Size(368, 34)
        Me.mnuAcercaDe.Text = "&Acerca de..."
        '
        'mnuFileSep5
        '
        Me.mnuFileSep5.Name = "mnuFileSep5"
        Me.mnuFileSep5.Size = New System.Drawing.Size(365, 6)
        '
        'mnuSalir
        '
        Me.mnuSalir.Name = "mnuSalir"
        Me.mnuSalir.Size = New System.Drawing.Size(368, 34)
        Me.mnuSalir.Text = "&Salir"
        '
        'txtSalida
        '
        Me.txtSalida.AcceptsReturn = True
        Me.txtSalida.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSalida.BackColor = System.Drawing.Color.Black
        Me.txtSalida.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSalida.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSalida.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtSalida.ForeColor = System.Drawing.Color.Lime
        Me.txtSalida.Location = New System.Drawing.Point(4, 36)
        Me.txtSalida.MaxLength = 0
        Me.txtSalida.Multiline = True
        Me.txtSalida.Name = "txtSalida"
        Me.txtSalida.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSalida.Size = New System.Drawing.Size(986, 480)
        Me.txtSalida.TabIndex = 4
        Me.txtSalida.Text = "txtSalida" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtEntrada
        '
        Me.txtEntrada.AcceptsReturn = True
        Me.txtEntrada.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEntrada.BackColor = System.Drawing.Color.Black
        Me.txtEntrada.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEntrada.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEntrada.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtEntrada.ForeColor = System.Drawing.Color.Lime
        Me.txtEntrada.Location = New System.Drawing.Point(4, 520)
        Me.txtEntrada.MaxLength = 0
        Me.txtEntrada.Multiline = True
        Me.txtEntrada.Name = "txtEntrada"
        Me.txtEntrada.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEntrada.Size = New System.Drawing.Size(986, 60)
        Me.txtEntrada.TabIndex = 3
        Me.txtEntrada.Text = "txtEntrada" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "línea 2"
        '
        'cmdNuevo
        '
        Me.cmdNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNuevo.Location = New System.Drawing.Point(775, 586)
        Me.cmdNuevo.Name = "cmdNuevo"
        Me.cmdNuevo.Size = New System.Drawing.Size(212, 34)
        Me.cmdNuevo.TabIndex = 0
        Me.cmdNuevo.Text = "Iniciar nueva sesión"
        '
        'Timer1
        '
        Me.Timer1.Interval = 300
        '
        'LabelInfo
        '
        Me.LabelInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelInfo.Location = New System.Drawing.Point(12, 590)
        Me.LabelInfo.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelInfo.Name = "LabelInfo"
        Me.LabelInfo.Size = New System.Drawing.Size(757, 30)
        Me.LabelInfo.TabIndex = 6
        Me.LabelInfo.Text = "Eliza para Visual Basic, ©Guillermo Som (Guille), 1998-2002, 2023"
        Me.LabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'fEliza
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1002, 632)
        Me.Controls.Add(Me.LabelInfo)
        Me.Controls.Add(Me.txtSalida)
        Me.Controls.Add(Me.txtEntrada)
        Me.Controls.Add(Me.cmdNuevo)
        Me.Controls.Add(Me.MainMenu1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 42)
        Me.Name = "fEliza"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Eliza para Visual Basic"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents Timer1 As Timer
    Private WithEvents txtEntrada As TextBox
    Private WithEvents LabelInfo As Label
    Private WithEvents txtSalida As TextBox
    Private WithEvents cmdNuevo As Button
#End Region
End Class