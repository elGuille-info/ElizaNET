'------------------------------------------------------------------------------
' cContenido                                                        (22/ene/23)
' Clase básica para almacenar un nombre
'
' ©Guillermo Som (Guille), 1998-2002, 2023
'------------------------------------------------------------------------------

'------------------------------------------------------------------------------
' cContenido.cls                                                    (30/May/98)
' Clase básica para almacenar un nombre
'
' ©Guillermo 'guille' Som, 1998-2002
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On

Public Class cContenido

    ''' <summary>
    ''' Crear las instancias indicando siempre la clave o id de este contenido.
    ''' </summary>
    ''' <param name="newID"></param>
    Public Sub New(newID As String)
        ID = newID
        Contenido = ""
    End Sub

    ''' <summary>
    ''' El contenido asociado con la clave de este contenido.
    ''' </summary>
    Public Property Contenido As String

    ''' <summary>
    ''' El ID o clave de este contenido.
    ''' </summary>
    Public ReadOnly Property ID As String

End Class
