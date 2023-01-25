VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cContenido"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' cContenido.cls                                                    (30/May/98)
' Clase básica para almacenar un nombre
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Explicit

Private m_ID As String
Private m_Contenido As String
Attribute m_Contenido.VB_VarUserMemId = 0

Public Property Get Contenido() As String
Attribute Contenido.VB_UserMemId = 0
    Contenido = m_Contenido
End Property

Public Property Let Contenido(ByVal newContenido As String)
    m_Contenido = newContenido
End Property

Public Property Get ID() As String
    ID = m_ID
End Property

Public Property Let ID(ByVal newID As String)
    ' Una vez asignado el ID, no se puede cambiar
    Static beenHere As Boolean
    '
    If Not beenHere Then
        beenHere = True
        m_ID = newID
    End If
End Property
