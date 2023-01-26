'------------------------------------------------------------------------------
' Dialogos                                                    (26/ene/23 16.56)
' Para usar el TaskDialog en vez de los MessageBox
'
' (c) Guillermo Som (Guille), 2023
'------------------------------------------------------------------------------
Imports UtilidadesDialog

Public Class Dialogos

    ' Para simular el MsgBox de Visual Basic
    'Public Shared Function MsgBox(prompt As String, msgBoxButtons As MsgBoxStyle, title As String) As MsgBoxResult
    '    Dim buttons As MessageBoxButtons
    '    Dim res = MessageBoxShow(prompt, title, buttons)
    'End Function

    ''' <summary>
    ''' Convierte un MessageBox normal a TaskDialog.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="caption"></param>
    ''' <param name="buttons"></param>
    ''' <param name="icon"></param>
    ''' <param name="defaultButton"></param>
    ''' <param name="enEspañol">Si se deben traducir los mensajes al castellano.</param>
    ''' <returns></returns>
    ''' <remarks>Utiliza la definición de MessageBoxShow en DialogTask. Si se cierra con la x devuelve cancel.</remarks>
    Public Shared Function MessageBoxShow(
                        text As String,
                        caption As String,
                        buttons As MessageBoxButtons,
                        Optional icon As MessageBoxIcon = MessageBoxIcon.None,
                        Optional defaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1,
                        Optional enEspañol As Boolean = False
                        ) As DialogResult
        ' Usando la versión de Visual Basic.
        'Return MessageBoxShow(Nothing, text, caption, buttons, icon, defaultButton, enEspañol)
        ' Usando la definida en C# (que es la que, seguramente, ampliaré primero)
        Return DialogTask.MessageBoxShow(text, caption, buttons, icon, defaultButton, enEspañol)
    End Function

End Class
