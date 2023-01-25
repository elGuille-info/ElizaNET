VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cRegla"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'------------------------------------------------------------------------------
' cRegla.cls                                                        (30/May/98)
' Clase para mantener colecciones de Contenidos y KeyWords
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Explicit

Public Aleatorio As Boolean
Public Nivel As Long

Private colRespuestas As cRespuestas
Private colKeyWords As cKeyWords

Private m_Contenido As cContenido

Public Property Get Contenido() As String
Attribute Contenido.VB_UserMemId = 0
    Contenido = m_Contenido
End Property

Public Property Let Contenido(ByVal NewContenido As String)
    m_Contenido = NewContenido
End Property

Public Property Get Extras() As cKeyWords
    ' Si se usa como colección, devolverá objetos del tipo cRespuestas
    Set Extras = colKeyWords
End Property

Public Property Get Respuestas() As cRespuestas
    ' Si se usa como colección, devolverá objetos del tipo cContenido
    Set Respuestas = colRespuestas
End Property

Private Sub Class_Initialize()
    Set m_Contenido = New cContenido
    Set colRespuestas = New cRespuestas
    Set colKeyWords = New cKeyWords
End Sub

Private Sub Class_Terminate()
    Set colKeyWords = Nothing
    Set colRespuestas = Nothing
    Set m_Contenido = Nothing
End Sub

