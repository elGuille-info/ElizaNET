VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cRespuestas"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' cRespuestas.cls                                                   (30/May/98)
' Clase colección para almacenar objetos del tipo cContenido
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Explicit

Public UltimoItem As Long

'Public Event NewItem(ByVal sContenido As String)

Private m_col As Collection
'Public Sub Clear()
'    'Borrar la colección
'    Set m_col = Nothing
'    Set m_col = New Collection
'End Sub
Private m_Contenido As cContenido

Public Function Count() As Long
    ' Número de elementos en la colección
    Count = m_col.Count
End Function

Public Function Item(ByVal newContenido As Variant) As cContenido
Attribute Item.VB_UserMemId = 0
    'Dim tContenido As New cContenido
    Dim tContenido As cContenido
    '
    On Local Error Resume Next
    '
    Set Item = m_col.Item(newContenido)
    If Err Then
        'Si no existe añadirlo
        Set tContenido = New cContenido
        tContenido.ID = newContenido
        m_col.Add tContenido, newContenido
        'Por si se usan los eventos, avisar de que hay un nuevo
        'contenido
        'RaiseEvent NewItem(NewContenido)
        Set Item = tContenido
    End If
    Set tContenido = Nothing
    Err = 0
End Function

Public Function NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
    ' Debe ser un miembro oculto y el id del procedimiento debe ser -4
    '
    Set NewEnum = m_col.[_NewEnum]
End Function

Private Sub Class_Initialize()
    Set m_Contenido = New cContenido
    Set m_col = New Collection
End Sub

Private Sub Class_Terminate()
    Set m_col = Nothing
    Set m_Contenido = Nothing
End Sub

Public Property Get Contenido() As String
    Contenido = m_Contenido
End Property

Public Property Let Contenido(ByVal newContenido As String)
    m_Contenido = newContenido
End Property

Public Sub Add(ByVal newContenido As String)
    ' Añadirlo a la colección
    newContenido = Trim$(newContenido)
    If Len(newContenido) Then
        Me.Item(newContenido) = newContenido
    End If
End Sub

Public Function ExisteItem(ByVal sContenido As String) As Boolean
    '
    Dim tContenido As cContenido
    '
    On Local Error Resume Next
    '
    Set tContenido = m_col.Item(sContenido)
    If Err Then
        ExisteItem = False
    Else
        ExisteItem = True
    End If
    Err = 0
End Function
