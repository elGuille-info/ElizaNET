// ------------------------------------------------------------------------------
// Dialogos                                                    (26/ene/23 16.56)
// Para usar el TaskDialog en vez de los MessageBox
//
// Convertida a C# a partir de la versión para Visual Basic .NET
// 
// (c)Guillermo Som (Guille), 2022-2023
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UtilidadesDialog;

namespace UtilidadesDialog;
public class Dialogos
{
    /// <summary>
    /// Convierte un MessageBox normal a TaskDialog.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="caption"></param>
    /// <param name="buttons"></param>
    /// <param name="icon"></param>
    /// <param name="defaultButton"></param>
    /// <param name="enEspañol">Si se deben traducir los mensajes al castellano.</param>
    /// <returns></returns>
    /// <remarks>Utiliza la definición de MessageBoxShow en DialogTask. Si se cierra con la x devuelve cancel.</remarks>
    public static DialogResult MessageBoxShow(string text, string caption, MessageBoxButtons buttons,
                                              MessageBoxIcon icon = MessageBoxIcon.None,
                                              MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
                                              bool enEspañol = false)
    {
        return DialogTask.MessageBoxShow(text, caption, buttons, icon, defaultButton, enEspañol);
    }
}
