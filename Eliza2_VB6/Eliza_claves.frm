VERSION 5.00
Begin VB.Form Eliza_claves 
   AutoRedraw      =   -1  'True
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Mantenimiento de claves y respuestas de Eliza"
   ClientHeight    =   6000
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   8205
   LinkTopic       =   "Form2"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   ScaleHeight     =   6000
   ScaleWidth      =   8205
   StartUpPosition =   2  'CenterScreen
   Begin VB.ListBox List1 
      Height          =   2205
      Index           =   1
      ItemData        =   "Eliza_claves.frx":0000
      Left            =   210
      List            =   "Eliza_claves.frx":0007
      TabIndex        =   6
      Top             =   3210
      Width           =   3075
   End
   Begin VB.Timer Timer1 
      Interval        =   100
      Left            =   7140
      Top             =   5490
   End
   Begin VB.ComboBox Combo1 
      Height          =   315
      Left            =   210
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   150
      Width           =   3075
   End
   Begin VB.ListBox List2 
      Height          =   4935
      Left            =   3480
      TabIndex        =   1
      Top             =   510
      Width           =   4515
   End
   Begin VB.ListBox List1 
      Height          =   2205
      Index           =   0
      ItemData        =   "Eliza_claves.frx":0015
      Left            =   210
      List            =   "Eliza_claves.frx":001C
      TabIndex        =   0
      Top             =   540
      Width           =   3075
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
      Height          =   255
      Index           =   0
      Left            =   210
      TabIndex        =   7
      Top             =   2820
      Width           =   3015
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
      Height          =   255
      Index           =   2
      Left            =   3480
      TabIndex        =   5
      Top             =   180
      Width           =   4515
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
      Height          =   255
      Index           =   3
      Left            =   3480
      TabIndex        =   4
      Top             =   5490
      Width           =   3015
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
      Height          =   255
      Index           =   1
      Left            =   210
      TabIndex        =   3
      Top             =   5490
      Width           =   3015
   End
End
Attribute VB_Name = "Eliza_claves"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' Eliza_claves                                                      (22/Jun/98)
' Form para ver las claves y demás palabras y respuestas de Eliza
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Explicit

Public Eliza As cEliza

Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
    (ByVal hWnd As Long, ByVal wMsg As Long, _
    ByVal wParam As Long, lParam As Long) As Long

Private Sub Combo1_Click()
    Dim i As Currency, j As Long
    '
    j = Combo1.ListIndex
    i = Eliza.AsignarPalabras(List1(0), List1(1), Combo1.ItemData(j))
    j = (i - Fix(i)) * 1000
    i = Fix(i)
    Label1(0) = i & " claves principales"
    Label1(1) = j & " claves extras"
    List2.Clear
    Label1(3) = ""
End Sub

Private Sub Form_Load()
    ' Poner scroll horizontal en los ListBoxes
    Const LB_SETHORIZONTALEXTENT As Long = &H194
    Dim ScaleTmp As Long  'Valor anterior de ScaleMode
    '
    ScaleTmp = ScaleMode
    ScaleMode = vbPixels            'wParam is in PIXELs(3)
    Call SendMessage(List1(0).hWnd, LB_SETHORIZONTALEXTENT, 1024&, 0&)
    Call SendMessage(List1(1).hWnd, LB_SETHORIZONTALEXTENT, 1024&, 0&)
    Call SendMessage(List2.hWnd, LB_SETHORIZONTALEXTENT, 1024&, 0&)
    ScaleMode = ScaleTmp     'Restablecer el valor anterior de ScaleMode
    '
    Timer1.Enabled = True
    'Inicializar
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Set Eliza = Nothing
    Set Eliza_claves = Nothing
End Sub

Private Sub Inicializar()
    ' Comprobar si hay que inicializar la variable Eliza
'    Dim tTimer As cGetTimer
    Dim i As Currency
    
    If Not Eliza Is Nothing Then
        Label1(2) = ""
    Else
        Show
        Label1(2) = "Un momento, mientras cargo la lista de claves..."
        ' Crear el objeto
        Set Eliza = New cEliza
'        Set tTimer = New cGetTimer
'        tTimer.StartTimer
        Eliza.Inicializar
'        tTimer.StopTimer
'        Label1(2) = "Tiempo en inicializar (y asignar las palabras): " & tTimer.ElapsedTimer & " segundos."
    End If
    '
    With Combo1
        .AddItem "Claves"
        .ItemData(.NewIndex) = eClaves
        .AddItem "Verbos"
        .ItemData(.NewIndex) = eVerbos
        .AddItem "RS (reglas simplif.)"
        .ItemData(.NewIndex) = eRS
        .AddItem "Simp (simpl. en respuesta)"
        .ItemData(.NewIndex) = eSimp
        .AddItem "Recordar lo que dijo el user"
        .ItemData(.NewIndex) = eRec
        .AddItem "Base datos user"
        .ItemData(.NewIndex) = eBU
    End With
    '
    DoEvents
    Combo1.ListIndex = 0
End Sub

Private Sub List1_Click(Index As Integer)
    Dim sClave As String
    Dim i As Currency
    Dim j As Long
    '
    j = Combo1.ListIndex
    If Combo1.ItemData(j) = eClaves Then
        With List1(Index)
            If .ListIndex > -1 Then
                sClave = .List(.ListIndex)
                If Index = 0 Then
                    i = Eliza.AsignarPalabras(List2, , eExtras, sClave)
                Else
                    i = Eliza.AsignarPalabras(List2, , eExtras2, sClave)
                End If
            End If
            j = (i - Fix(i)) * 1000
            i = Fix(i)
            Label1(3) = i & " Extra" & IIf(i <> 1, "s", "") & ", " & j & " Respuesta" & IIf(j <> 1, "s", "")
        End With
    End If
End Sub

Private Sub Timer1_Timer()
    Timer1.Enabled = False
    Inicializar
End Sub


