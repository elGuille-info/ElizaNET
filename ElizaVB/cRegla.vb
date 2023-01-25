'------------------------------------------------------------------------------
' cRegla                                                            (22/ene/23)
' Clase para mantener colecciones de Contenidos y KeyWords
'
' ©Guillermo Som (Guille), 1998-2002, 2023
'------------------------------------------------------------------------------

'------------------------------------------------------------------------------
' cRegla.cls                                                        (30/May/98)
' Clase para mantener colecciones de Contenidos y KeyWords
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On

Public Class cRegla
    ''' <summary>
    ''' Si se deben tomar las respuestas de forma aleatoria.
    ''' </summary>
    Public Property Aleatorio As Boolean
    ''' <summary>
    ''' El nivel a tener en cuenta al analizar las palabras, el más bajo es 0.
    ''' </summary>
    Public Property Nivel As Integer

    Private ReadOnly colRespuestas As New cRespuestas
    Private ReadOnly colKeyWords As New cKeyWords

    ''' <summary>
    ''' La clave para esta regla.
    ''' </summary>
    Public Property Contenido As String

    Public Sub New(Optional contenido As String = "")
        Me.Contenido = contenido
    End Sub

    ''' <summary>
    ''' Las respuestas extras de esta regla.
    ''' </summary>
    Public ReadOnly Property Extras As cKeyWords
        Get
            Return colKeyWords
        End Get
    End Property

    ''' <summary>
    ''' Las respuestas principales de esta regla.
    ''' </summary>
    Public ReadOnly Property Respuestas As cRespuestas
        Get
            Return colRespuestas
        End Get
    End Property

End Class
