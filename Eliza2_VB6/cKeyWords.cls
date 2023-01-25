VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cKeyWords"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' cKeyWords.cls                                                     (30/May/98)
' Colección de objetos del tipo cRespuestas
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Explicit

'Public Event NewItem(ByVal sContenido As String)

Private m_col As Collection

Public Function NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
    ' Debe ser un miembro oculto y el id del procedimiento debe ser -4
    '
    Set NewEnum = m_col.[_NewEnum]
End Function

Public Function Item(ByVal newContenido As Variant) As cRespuestas
Attribute Item.VB_UserMemId = 0
    'Dim tRespuestas As New cRespuestas
    Dim tRespuestas As cRespuestas
    '
    On Local Error Resume Next
    '
    Set Item = m_col.Item(newContenido)
    If Err Then
        Set tRespuestas = New cRespuestas
        tRespuestas.Contenido = newContenido
        m_col.Add tRespuestas, newContenido
        'por si se quiere usar un evento cada vez que se añada
        'un nuevo elemento a la colección
        'RaiseEvent NewItem(NewContenido)
        Set Item = tRespuestas
    End If
    Set tRespuestas = Nothing
    Err = 0
End Function

Public Function Count() As Long
    ' Método Count de las colección
    Count = m_col.Count
End Function

Private Sub Class_Initialize()
    Set m_col = New Collection
End Sub

Private Sub Class_Terminate()
    Set m_col = Nothing
End Sub
