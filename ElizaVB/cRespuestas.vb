'------------------------------------------------------------------------------
' cRespuestas                                                 (22/ene/23 10.26)
' Clase colección para almacenar objetos del tipo cContenido
'
' ©Guillermo Som (Guille), 1998-2002, 2023
'------------------------------------------------------------------------------

'------------------------------------------------------------------------------
' cRespuestas.cls                                                   (30/May/98)
' Clase colección para almacenar objetos del tipo cContenido
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On

Public Class cRespuestas

    ' Lo inicio con -1 para que al incrementarlo sea 0          (24/ene/23 12.37)
    ' ya que los índices empiezan por cero
    Public Property UltimoItem As Integer = -1
    Public Property Contenido As String

    Public Sub New(Optional contenido As String = "")
        Me.Contenido = contenido
    End Sub

    Private ReadOnly m_col As New Dictionary(Of String, cContenido)

    ' Número de elementos en la colección
    Public ReadOnly Property Count As Integer
        Get
            Return m_col.Count
        End Get
    End Property

    Public Sub Add(newContenido As String)
        ' Añadirlo a la colección
        newContenido = newContenido.Trim()
        If String.IsNullOrEmpty(newContenido) = False Then
            Item(newContenido).Contenido = newContenido
        End If
    End Sub

    ''' <summary>
    ''' Limpiar el contenido de la colección.
    ''' </summary>
    Public Sub Clear()
        m_col.Clear()
    End Sub

    ''' <summary>
    ''' El elemento con la clave indicada. Si no existe, lo crea y añade a la colección.
    ''' </summary>
    ''' <param name="newContenido"></param>
    'Default Public ReadOnly Property Item(newContenido As String) As cContenido
    Public ReadOnly Property Item(newContenido As String) As cContenido
        Get
            Dim tContenido As cContenido '= Nothing

            'If m_col.TryGetValue(newContenido, tContenido) = False Then
            '    'Si no existe añadirlo
            '    tContenido = New cContenido(newContenido)
            '    m_col.Add(newContenido, tContenido)
            'End If

            If m_col.ContainsKey(newContenido) Then
                tContenido = m_col.Item(newContenido)
            Else
                'Si no existe añadirlo
                tContenido = New cContenido(newContenido)
                m_col.Add(newContenido, tContenido)
            End If

            Return tContenido
        End Get
    End Property

    ''' <summary>
    ''' El elemento con el índice indicado. Si no existe, se devuelve el último.
    ''' </summary>
    ''' <param name="newContenido">El índice en base 0: de 0 a col.count-1</param>
    'Default Public ReadOnly Property Item(newContenido As Integer) As cContenido
    Public ReadOnly Property Item(newContenido As Integer) As cContenido
        Get
            'If m_col.Count < newContenido Then
            If newContenido < m_col.Count Then
                Return m_col.ElementAt(newContenido).Value
            Else
                Return m_col.ElementAt(m_col.Count - 1).Value
            End If
        End Get
    End Property

    Public Function ExisteItem(sContenido As String) As Boolean
        Return m_col.ContainsKey(sContenido)
    End Function

    Public ReadOnly Property Valores As Dictionary(Of String, cContenido).ValueCollection
        Get
            Return m_col.Values
        End Get
    End Property

End Class
