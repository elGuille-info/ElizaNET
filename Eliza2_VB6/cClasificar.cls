VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cClasificar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'------------------------------------------------------------------
'Clase para clasificar arrays                           (25/May/98)
'
'©Guillermo 'guille' Som, 1998
'------------------------------------------------------------------
Option Explicit

Public Sub Classif2(n() As String, Optional MenorAMayor As Boolean = True)
    '------------------------------------------------------------
    'Esta rutina está adaptada de una que "copié" de un libro
    'para el Amstrad CPC de Data-Becker
    '
    'Los parámetros:
    'n$() es el array a clasificar
    '
    '------------------------------------------------------------
    Dim i As Long, s As Long, L As Long, j As Long, r As Long
    Dim t As String
    Dim FI As Long
    
    FI = UBound(n)
    If FI < 2 Then Exit Sub             'Nada que clasificar
    
    ReDim O(30) As Integer, U(30) As Integer
    s = 1
    O(1) = 1
    U(1) = FI
    
    Do
        L = O(s): r = U(s): s = s - 1
        Do
            i = L: j = r:
            t$ = n$((L + r) / 2)
            If MenorAMayor Then
                If LTrim$(t$) = "" Then t$ = Chr$(255)
            Else
                If LTrim$(t$) = "" Then t$ = Chr$(1)
            End If
            Do
                If MenorAMayor Then
                    While (n$(i) < t$ And i < r): i = i + 1: Wend
                    While (n$(j) > t$ And j > L): j = j - 1: Wend
                Else
                    While (n$(i) > t$ And i < r): i = i + 1: Wend
                    While (n$(j) < t$ And j > L): j = j - 1: Wend
                End If
                If i <= j Then
                    Swap n$(i), n$(j)
                    i = i + 1
                    j = j - 1
                End If
            Loop While i <= j
            If r - i <= j - L Then
                If i < r Then
                    s = s + 1
                    O(s) = i
                    U(s) = r
                End If
                r = j
            Else
                If L < j Then
                    s = s + 1
                    O(s) = L
                    U(s) = j
                End If
                L = i
            End If
        Loop While r > L
    Loop While s > 0
    Erase O, U
    '
'Para usarlo:
'
'    nItem = List1.ListCount
'    ReDim Preserve Nombre(nItem)
'
'    For i = 1 To nItem
'        Nombre(i) = List1.List(i - 1)
'    Next
'
'    List2.Clear
'    t1 = Timer
'    Classif2 Nombre(), nItem
'    t2 = Timer
'    Label1 = "Para " & CStr(nItem) & " elementos= " & Format$(t2 - t1, "###,##0.00") & " seg"
'    DoEvents
'    For i = 1 To nItem
'        List2.AddItem Nombre(i)
'    Next
End Sub
Public Sub Classif(n() As String, CL() As Long, Optional MenorAMayor As Boolean = True)
    '------------------------------------------------------------
    'Esta rutina está adaptada de una que "copié" de un libro
    'para el Amstrad CPC de Data-Becker
    '
    'Los parámetros:
    'n$() es el array a clasificar
    'CL() aquí se guardan "punteros" a los elementos clasificados
    'FI   es el número de elementos
    '
    'La variable n$() permanece sin clasificar, para acceder a los
    'elementos clasificados habría que hacer esto:
    'For i = 0 to FI
    '   Print i, n$(CL(i))
    'Next
    '
    'Puede que sea muy rebuscado, pero lo hice porque mis arrays
    'deberían permanecer en el mismo orden que estaban originalmente
    '------------------------------------------------------------
    Dim i As Long, s As Long, L As Long, j As Long, r As Long
    Dim t As String
    Dim FI As Long
    
    FI = UBound(n)
    For i = 1 To FI: CL(i) = i: Next
    If FI < 2 Then Exit Sub             'Nada que clasificar
    
    ReDim O(30) As Integer, U(30) As Integer
    s = 1
    O(1) = 1
    U(1) = FI
    
    Do
        L = O(s): r = U(s): s = s - 1
        Do
            i = L: j = r:
            t$ = n$(CL((L + r) / 2))
            'si se clasifica de menor a mayor
            If MenorAMayor Then
                If LTrim$(t$) = "" Then t$ = Chr$(255)
            Else
                If LTrim$(t$) = "" Then t$ = Chr$(1)
            End If
            Do
                If MenorAMayor Then
                    While (n$(CL(i)) < t$ And i < r): i = i + 1: Wend
                    While (n$(CL(j)) > t$ And j > L): j = j - 1: Wend
                Else
                    While (n$(CL(i)) > t$ And i < r): i = i + 1: Wend
                    While (n$(CL(j)) < t$ And j > L): j = j - 1: Wend
                End If
                If i <= j Then
                    Swap CL(i), CL(j)
                    i = i + 1
                    j = j - 1
                End If
            Loop While i <= j
            If r - i <= j - L Then
                If i < r Then
                    s = s + 1
                    O(s) = i
                    U(s) = r
                End If
                r = j
            Else
                If L < j Then
                    s = s + 1
                    O(s) = L
                    U(s) = j
                End If
                L = i
            End If
        Loop While r > L
    Loop While s > 0
    Erase O, U
    '
'Para usarlo:
'
'    nItem = List1.ListCount
'    ReDim Preserve Nombre(nItem)
'    ReDim Orden(0 To nItem) As Integer
'
'    For i = 1 To nItem
'        Nombre(i) = List1.List(i - 1)
'    Next
'
'    List2.Clear
'    t1 = Timer
'    Classif Nombre(), Orden(), nItem
'    t2 = Timer
'    Label1 = "Para " & CStr(nItem) & " elementos= " & Format$(t2 - t1, "###,##0.00") & " seg"
'    DoEvents
'    For i = 1 To nItem
'        List2.AddItem Nombre(Orden(i))
'    Next
End Sub

Private Sub Swap(uno As Variant, dos As Variant)
    'Intercambia dos valores
    Dim tres As Variant
    
    tres = uno
    uno = dos
    dos = tres
End Sub

