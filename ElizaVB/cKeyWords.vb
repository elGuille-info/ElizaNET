'------------------------------------------------------------------------------
' cKeyWords                                                         (22/ene/23)
' Colección de objetos del tipo cRespuestas
'
' ©Guillermo 'guille' Som, 1998-2002, 2023
'------------------------------------------------------------------------------

'------------------------------------------------------------------------------
' cKeyWords.cls                                                     (30/May/98)
' Colección de objetos del tipo cRespuestas
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On

Public Class cKeyWords
    Private m_col As New Dictionary(Of String, cRespuestas)

    'Default Public ReadOnly Property Item(ByVal newContenido As String) As cRespuestas
    Public ReadOnly Property Item(newContenido As String) As cRespuestas
        Get
            Dim tRespuestas As cRespuestas

            If m_col.ContainsKey(newContenido) Then
                tRespuestas = m_col.Item(newContenido)
            Else
                'Si no existe añadirlo
                tRespuestas = New cRespuestas(newContenido)
                m_col.Add(newContenido, tRespuestas)
            End If
            Return tRespuestas
        End Get
    End Property

    Public ReadOnly Property Count As Integer
        Get
            ' Método Count de la colección
            Return m_col.Count
        End Get
    End Property

    Public ReadOnly Property Valores As Dictionary(Of String, cRespuestas).ValueCollection
        Get
            Return m_col.Values
        End Get
    End Property

End Class
