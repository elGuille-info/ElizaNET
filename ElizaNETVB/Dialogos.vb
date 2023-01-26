'------------------------------------------------------------------------------
' Dialogos                                                    (26/ene/23 16.56)
' Para usar el TaskDialog en vez de los MessageBox
'
' (c)Guillermo Som (Guille), 2022-2023
'------------------------------------------------------------------------------
Imports UtilidadesDialog

Public Class Dialogos
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
        Return DialogTask.MessageBoxShow(text, caption, buttons, icon, defaultButton, enEspañol)
    End Function

End Class
