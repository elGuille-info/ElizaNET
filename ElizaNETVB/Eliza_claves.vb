'------------------------------------------------------------------------------
' Eliza_claves														(23/ene/23)
' Convertido a .NET usando Visual Studio Express 2008
' Y arreglado manualmente.
'
' ©Guillermo Som (Guille), 1998-2002, 2023
'------------------------------------------------------------------------------
'
' Eliza_claves                                                      (22/Jun/98)
' Form para ver las claves y demás palabras y respuestas de Eliza
'
' ©Guillermo 'guille' Som, 1998-2002, 2023
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On

Imports ElizaVB

Friend Class Eliza_claves
	Inherits System.Windows.Forms.Form

	Public Sub New(eli As cEliza)
		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.

		Eliza = eli
	End Sub

	Private Eliza As cEliza

	Private ReadOnly clavesTag As cEliza.eTiposDeClaves() = {cEliza.eTiposDeClaves.eClaves, cEliza.eTiposDeClaves.eVerbos, cEliza.eTiposDeClaves.eRS, cEliza.eTiposDeClaves.eSimp, cEliza.eTiposDeClaves.eRec, cEliza.eTiposDeClaves.eBU}

	Private Sub Combo1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Combo1.SelectedIndexChanged
		Dim i As Decimal
		Dim j As Integer

		j = Combo1.SelectedIndex
		If j = -1 Then
			Combo1.SelectedIndex = 0
			Return
		End If
		Dim que = clavesTag(j)
		i = AsignarPalabras(_List1_0, _List1_1, que)
		'j = (i - Fix(i)) * 1000
		'i = Fix(i)
		j = CInt((i - Math.Floor(i)) * 1000)
		i = Math.Floor(i)
		_Label1_0.Text = i & " claves principales"
		_Label1_1.Text = j & " claves extras"
		List2.Items.Clear()
		_Label1_3.Text = ""
	End Sub

	Private inicializando As Boolean

	Private Sub Eliza_claves_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		inicializando = True
		_Label1_2.Text = "Cargando el formulario..."
		Timer1.Interval = 300
		Timer1.Enabled = True
	End Sub

	'Private Sub Eliza_claves_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
	'	' Adaptar el tamaño y posición de los controles,		(25/ene/23 08.50)
	'	' están con Anchor a los ancho
	'	If inicializando Then Return
	'	inicializando = True

	'	Dim l2l = List2.Left
	'	Dim l2w = List2.Width - 246
	'	'Dim l3w = List2.Width - List2.Left
	'	Combo1.Width = l2w
	'	_List1_0.Width = l2w
	'	_List1_1.Width = l2w
	'	_Label1_0.Width = l2w
	'	_Label1_1.Width = l2w

	'	'List2.Left = Combo1.Width + 12
	'	'List2.Width -= (List2.Left - l2l)

	'	'List2.Left = Combo1.Width + 12
	'	'List2.Width -= (List2.Left - l2l)
	'	'_Label1_2.Left = List2.Left
	'	'_Label1_3.Left = List2.Left
	'	'_Label1_2.Width = List2.Width
	'	'_Label1_3.Width = List2.Width
	'	inicializando = False
	'End Sub

	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
		Timer1.Enabled = False
		Inicializar()
		inicializando = False
	End Sub

	Private Sub Inicializar()
		' Comprobar si hay que inicializar la variable Eliza
		_Label1_2.Text = ""
		'If Eliza IsNot Nothing Then
		If Eliza Is Nothing Then
			'	_Label1_2.Text = "Palabras previamente asignadas"
			'Else
			Show()
			_Label1_2.Text = "Un momento, mientras cargo la lista de claves..."
			' Crear el objeto
			Eliza = New cEliza(fEliza.ElizaLocalPath())
			Dim sw = Stopwatch.StartNew()
			Eliza.Inicializar()
			sw.Stop()
			_Label1_2.Text = "Tiempo en inicializar (y asignar las palabras): " & sw.Elapsed.ToString("mm\:ss\.fff") '.Seconds & " segundos."
		End If
		With Combo1
			.Items.Add("Claves")
			.Items.Add("Verbos")
			.Items.Add("RS (reglas simplif.)")
			.Items.Add("Simp (simpl. en respuesta)")
			.Items.Add("Recordar lo que dijo el user")
			.Items.Add("Base datos user")
		End With

		Combo1.SelectedIndex = 0
		Application.DoEvents()
	End Sub

	Private Sub List1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _List1_0.SelectedIndexChanged, _List1_1.SelectedIndexChanged
		Dim sClave As String '= ""
		Dim i As Decimal
		Dim j As Integer

		Dim list1 = TryCast(sender, ListBox)
		If list1 Is Nothing Then Return

		j = Combo1.SelectedIndex
		If j = -1 Then
			Combo1.SelectedIndex = 0
			Return
		End If
		_Label1_2.Text = "Asignando las palabras..."
		'Dim sw = Stopwatch.StartNew()
		If clavesTag(j) = cEliza.eTiposDeClaves.eClaves Then
			With list1
				If .SelectedIndex > -1 Then
					sClave = list1.Items(.SelectedIndex).ToString()
					If list1 Is _List1_0 Then
						i = AsignarPalabras(List2, Nothing, cEliza.eTiposDeClaves.eExtras, sClave)
					Else
						i = AsignarPalabras(List2, Nothing, cEliza.eTiposDeClaves.eExtras2, sClave)
					End If
					_Label1_2.Text = sClave
				End If
				j = CInt((i - Math.Floor(i)) * 1000)
				i = Math.Floor(i)
				_Label1_3.Text = i & " Extra" & If(i <> 1, "s", "") & ", " & j & " Respuesta" & If(j <> 1, "s", "")
			End With
		End If
		'sw.Stop()
		'_Label1_2.Text = "Tiempo en asignar las palabras: " & sw.Elapsed.ToString("mm\:ss\.fff") '.Seconds & " segundos."
		toolTip1.SetToolTip(_Label1_2, _Label1_2.Text)

	End Sub

	Private Function AsignarPalabras(
			unList As ListBox,
			Optional unList1 As ListBox = Nothing,
			Optional esClave As cEliza.eTiposDeClaves = cEliza.eTiposDeClaves.eClaves,
			Optional ByRef sClave As String = "") As Decimal
		' Asigna las palabras a los listbox del form
		' Devolverá el número de palabras asignadas al ListBox
		Dim tRegla As cRegla
		Dim tRespuestas As cRespuestas
		Dim tContenido As cContenido
		Dim i, j As Integer
		Dim m_col = Eliza.ColReglas

		unList.Items.Clear()
		Select Case esClave
			Case cEliza.eTiposDeClaves.eClaves
				unList1.Items.Clear()
				For Each tRegla In m_col.Values
					unList.Items.Add(tRegla.Contenido)
					i += 1
					For Each tRespuestas In tRegla.Extras.Valores '.Values
						unList1.Items.Add(tRespuestas.Contenido)
						j += 1
					Next
				Next
			Case cEliza.eTiposDeClaves.eExtras
				tRegla = m_col(sClave)
				If tRegla.Extras.Count() > 0 Then
					unList.Items.Add("---Extras---")
					For Each tRespuestas In tRegla.Extras.Valores '.Values
						unList.Items.Add(tRespuestas.Contenido)
						i += 1
					Next
				End If
				If tRegla.Respuestas.Count > 0 Then
					unList.Items.Add("---Respuestas---")
					For Each tContenido In tRegla.Respuestas.Valores '.Values
						unList.Items.Add(tContenido.Contenido)
						j += 1
					Next
				End If
			Case cEliza.eTiposDeClaves.eExtras2
				For Each tRegla In m_col.Values
					If tRegla.Extras.Count > 0 Then
						For Each tRespuestas In tRegla.Extras.Valores '.Values
							If tRespuestas.Contenido = sClave Then
								For Each tContenido In tRespuestas.Valores '.Values
									unList.Items.Add(tContenido.Contenido)
									j += 1
								Next
							End If
						Next
					End If
				Next
			Case cEliza.eTiposDeClaves.eVerbos
				For Each tContenido In Eliza.ColVerbos.Valores '.Values
					unList.Items.Add(tContenido.ID & " -- " & tContenido.Contenido)
					i += 1
				Next
			Case cEliza.eTiposDeClaves.eRS
				For Each tContenido In Eliza.ColRS.Valores '.Values
					unList.Items.Add(tContenido.ID & " -- " & tContenido.Contenido)
					i += 1
				Next
			Case cEliza.eTiposDeClaves.eSimp
				For Each tContenido In Eliza.ColSimp.Valores '.Values
					unList.Items.Add(tContenido.ID & " -- " & tContenido.Contenido)
					i += 1
				Next
			Case cEliza.eTiposDeClaves.eRec
				For Each tContenido In Eliza.ColRec.Valores '.Values
					unList.Items.Add(tContenido.ID & " -- " & tContenido.Contenido)
					i += 1
				Next
			Case cEliza.eTiposDeClaves.eBU
				For Each tContenido In Eliza.ColBaseUser.Valores '.Values
					unList.Items.Add(tContenido.ID & " -- " & tContenido.Contenido)
					i += 1
				Next
		End Select
		Return CDec(i + j / 1000)
	End Function

End Class