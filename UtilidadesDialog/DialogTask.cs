//-------------------------------------------------------------------------------
// DialogTask                                                   (26/ene/23 16.52)
// Para el programa de Eliza.
//
// Basada en:
// DialogTask                                                   (04/abr/22 20.22)
//
// Para usar el TaskDialog del código de esta página:
// https://github.com/dotnet/samples/tree/main/windowsforms/TaskDialogDemo
//
// (c) Guillermo Som (Guille), 2022-2023
//-------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace UtilidadesDialog;

public class DialogTask
{

    //
    // NOTA: Si se modifica este método, actualizar la versión de Visual Basic. (06/abr/22 08.50)
    //

    /// <summary>
    /// Convierte un MessageBox normal a TaskDialog. Indicando el owner (formulario).
    /// </summary>
    /// <param name="owner">El formulario que contendrá este diálogo. Un valor nulo para no usarlo.</param>
    /// <param name="text">El texto a mostrar.</param>
    /// <param name="caption">El título del diálogo, también mostrado en la cabecera. (Esto no es así en MessageBox).</param>
    /// <param name="buttons">Los botones a usar.</param>
    /// <param name="icon">El icono a usar.</param>
    /// <param name="defaultButton">El botón predeterminado.</param>
    /// <param name="enEspañol">Si se deben traducir los botones al castellano. Esto es independiente del idioma de Windows.</param>
    /// <returns>Un valor del tipo DialogResult.</returns>
    /// <remarks>Usar este método como base para otros con menos argumentos.</remarks>
    public static DialogResult MessageBoxShow(
                        IWin32Window owner, 
                        string text, 
                        string caption, 
                        MessageBoxButtons buttons, 
                        MessageBoxIcon icon = MessageBoxIcon.None, 
                        MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, 
                        bool enEspañol = false)
    {
        var tdButtons = new TaskDialogButtonCollection();
        string tdHeading = caption;

        // Si se indica cancelar (en español) indicar true.     (06/abr/22 19.59)
        bool allowCancel = false;

        switch (buttons)
        {
            case MessageBoxButtons.OK:
                {
                    if (enEspañol)
                        tdButtons.Add(DialogTaskButtons.Aceptar);
                    else
                        tdButtons.Add(TaskDialogButton.OK);
                    break;
                }
            case MessageBoxButtons.OKCancel:
                {
                    if (enEspañol)
                    {
                        tdButtons.Add(DialogTaskButtons.Aceptar);
                        tdButtons.Add(DialogTaskButtons.Cancelar);
                        allowCancel = true;
                    }
                    else
                    {
                        tdButtons.Add(TaskDialogButton.OK);
                        tdButtons.Add(TaskDialogButton.Cancel);
                    }

                    break;
                }
            case MessageBoxButtons.RetryCancel:
                {
                    if (enEspañol)
                    {
                        tdButtons.Add(DialogTaskButtons.Reintentar);
                        tdButtons.Add(DialogTaskButtons.Cancelar);
                        allowCancel = true;
                    }
                    else
                    {
                        tdButtons.Add(TaskDialogButton.Retry);
                        tdButtons.Add(TaskDialogButton.Cancel);
                    }

                    break;
                }
            case MessageBoxButtons.YesNo:
                {
                    if (enEspañol)
                        tdButtons.Add(DialogTaskButtons.Si);
                    else
                        tdButtons.Add(TaskDialogButton.Yes);
                    tdButtons.Add(TaskDialogButton.No);
                    break;
                }
            case MessageBoxButtons.YesNoCancel:
                {
                    if (enEspañol)
                    {
                        // A ver si siendo los 3 los muestra en el orden añadido.
                        // Mostraba Si, Cancelar, No
                        tdButtons.Add(DialogTaskButtons.Si);
                        tdButtons.Add(DialogTaskButtons.No);
                        // tdButtons.Add(TaskDialogButton.No)
                        tdButtons.Add(DialogTaskButtons.Cancelar);
                        allowCancel = true;
                    }
                    else
                    {
                        tdButtons.Add(TaskDialogButton.Yes);
                        tdButtons.Add(TaskDialogButton.No);
                        tdButtons.Add(TaskDialogButton.Cancel);
                    }

                    break;
                }
            case MessageBoxButtons.AbortRetryIgnore:
                if (enEspañol) 
                {
                    tdButtons.Add(DialogTaskButtons.Anular);
                    tdButtons.Add(DialogTaskButtons.Reintentar);
                    tdButtons.Add(DialogTaskButtons.Ignorar);
                }
                else 
                {
                    tdButtons.Add(TaskDialogButton.Abort);
                    tdButtons.Add(TaskDialogButton.Retry);
                    tdButtons.Add(TaskDialogButton.Ignore);
                }
                break ;
            case MessageBoxButtons.CancelTryContinue:
                if (enEspañol) {
                    tdButtons.Add(DialogTaskButtons.Cancelar);
                    tdButtons.Add(DialogTaskButtons.IntentarNuevamente);
                    tdButtons.Add(DialogTaskButtons.Continuar);
                    allowCancel = true;
                }
                else {
                    tdButtons.Add(TaskDialogButton.Cancel);
                    tdButtons.Add(TaskDialogButton.TryAgain);
                    tdButtons.Add(TaskDialogButton.Continue);
                }
                break;
            default:
                {
                    if (enEspañol)
                        tdButtons.Add(DialogTaskButtons.Aceptar);
                    else
                        tdButtons.Add(TaskDialogButton.OK);
                    break;
                }
        }

        TaskDialogIcon tdIcon = icon switch
        {
            MessageBoxIcon.Information => TaskDialogIcon.Information,
            MessageBoxIcon.Error => TaskDialogIcon.Error,
            MessageBoxIcon.Warning => TaskDialogIcon.Warning,
            MessageBoxIcon.None => TaskDialogIcon.None,
            MessageBoxIcon.Question => TaskDialogIcon.Information,
            _ => TaskDialogIcon.Information,
        };
        TaskDialogButton tddDfaultButton = defaultButton switch
        {
            MessageBoxDefaultButton.Button1 => tdButtons.FirstOrDefault(),
            MessageBoxDefaultButton.Button2 => tdButtons.Count > 1 ? tdButtons[1] : tdButtons.FirstOrDefault(),
            MessageBoxDefaultButton.Button3 => tdButtons.Count > 2 ? tdButtons[2] : tdButtons.FirstOrDefault(),
            MessageBoxDefaultButton.Button4 => tdButtons.Count > 3 ? tdButtons[3] : tdButtons.FirstOrDefault(),
            _ => tdButtons.FirstOrDefault(),
        };

        // No permitir cancelar, así se mostrará como MessageBox.Show (06/abr/22 19.52)
        // Salvo que se indique el botón Cancelar en español 
        // (en inglés es automático y aunque se indique false se mostrará la x de cerrar)
        var tdPpage = new TaskDialogPage()
        {
            Heading = tdHeading,
            Text = text,
            Caption = caption,
            Icon = tdIcon,
            AllowCancel = allowCancel, 
            Buttons = tdButtons,
            DefaultButton = tddDfaultButton
        };

        TaskDialogButton tdRes;
        if(owner == null)
        {
            tdRes = TaskDialog.ShowDialog(tdPpage);
        }
        else
        {
            tdRes = TaskDialog.ShowDialog(owner, tdPpage);
        }
        
        var res = tdRes.Text.ToLower() switch
        {
            "yes" or "sí" or "si" => DialogResult.Yes,
            "ok" or "aceptar" => DialogResult.OK,
            "no" => DialogResult.No,
            "cancel" or "cancelar" => DialogResult.Cancel,
            "retry" or "reintentar" => DialogResult.Retry,
            "ignore" or "ignorar" => DialogResult.Ignore,
            "abort" or "anular" => DialogResult.Abort,
            "tryagain" or "re-intentar" or "volver a probar" or "intentar nuevamente" or "try again" or "try" => DialogResult.TryAgain,
            "continue" or "continuar" => DialogResult.Continue,
            _ => DialogResult.None,
        };
        return res;
    }

    /// <summary>
    /// Convierte un MessageBox normal a TaskDialog.
    /// </summary>
    /// <param name="text">El texto a mostrar.</param>
    /// <param name="caption">El título del diálogo, también mostrado en la cabecera. (Esto no es así en MessageBox).</param>
    /// <param name="buttons">Los botones a usar.</param>
    /// <param name="icon">El icono a usar. Predeterminado ninguno.</param>
    /// <param name="defaultButton">El botón predeterminado. Inicialmente el botón 1.</param>
    /// <param name="enEspañol">Si se deben traducir los botones al castellano. Esto es independiente del idioma de Windows.</param>
    /// <returns></returns>
    public static DialogResult MessageBoxShow(
                        string text,
                        string caption,
                        MessageBoxButtons buttons,
                        MessageBoxIcon icon = MessageBoxIcon.None,
                        MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
                        bool enEspañol=false)
    {
        return MessageBoxShow(null, text, caption, buttons, icon, defaultButton, enEspañol);
    }

    /// <summary>
    /// Muestra un cuadro de diálogo con el botón aceptar y resaltado como warning o error.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="heading"></param>
    /// <param name="caption"></param>
    /// <param name="esError">True muestra el icono ShieldErrorRed, false el ShieldWarningYellow.</param>
    /// <returns></returns>
    public static TaskDialogButton MessageBoxShieldShow(string text, string heading, string caption,
                                                        bool esError = true)
    {
        var buttons = CrearBotones(TaskDialogButton.OK);
        TaskDialogIcon icon = esError ? TaskDialogIcon.ShieldErrorRedBar : TaskDialogIcon.ShieldWarningYellowBar;

        TaskDialogPage page = DialogPage(text, heading, caption: caption,
                                         buttons: buttons,
                                         icon: icon,
                                         allowcancel: false);
        return ShowDialog(page);
    }

    /// <summary>
    /// Muestra un cuadro de diálogo con el botón aceptar usando los botones Shield (normalmente el amarillo o el rojo).
    /// </summary>
    /// <param name="text"></param>
    /// <param name="heading"></param>
    /// <param name="caption"></param>
    /// <param name="icon">El icono a mostrar, si es nulo se muestra Shield Warning.</param>
    /// <returns></returns>
    public static TaskDialogButton MessageBoxShieldShow(string text, string heading, string caption,
                                                        TaskDialogIcon icon = null)
    {
        var buttons = CrearBotones(TaskDialogButton.OK);

        if (icon == null)
            icon = TaskDialogIcon.ShieldWarningYellowBar;

        TaskDialogPage page = DialogPage(text, heading, caption: caption, 
                                         buttons: buttons, 
                                         icon:icon, 
                                         allowcancel: false);
        return ShowDialog(page);
    }

    // Mensaje Shield con varios botones.                       (07/abr/22 21.07)

    /// <summary>
    /// Muestra un cuadro de diálogo los botones indicados usando los botones Shield (normalmente el amarillo o el rojo).
    /// </summary>
    /// <param name="text"></param>
    /// <param name="heading"></param>
    /// <param name="caption"></param>
    /// <param name="buttons">Opcional, si no se indica, se usa el botón Aceptar.</param>
    /// <param name="icon">Opcional, si no se indica se usa Shield Warning.</param>
    /// <param name="allowCancel">Si no se indica, será false, true si se ha indicado el botón TaskDialogButton.Cancel.</param>
    /// <returns></returns>
    public static TaskDialogButton MessageBoxShieldShow(string text, string heading, string caption,
                                                        TaskDialogButtonCollection buttons = null, 
                                                        TaskDialogIcon icon = null, 
                                                        bool? allowCancel = null)
    {
        if(buttons == null)
        {
            buttons = CrearBotones(TaskDialogButton.OK);
        }
        else if(allowCancel.HasValue == false)
        {
            // Si existe el botón Cancel, permitir siempre que se cancele.
            // Salvo que se haya indicado un valor a allowCancel.
            if (buttons.Contains(TaskDialogButton.Cancel))
            {
                //if(allowCancel.HasValue == false)
                    allowCancel = true;
            }
        }

        if (icon == null)
        {
            icon = TaskDialogIcon.ShieldWarningYellowBar;
        }

        TaskDialogPage page = DialogPage(text, heading, caption: caption,
                                         buttons: buttons,
                                         icon: icon,
                                         allowcancel: allowCancel == null ? false : allowCancel.Value);
        return ShowDialog(page);
    }

    // Mensaje con la zona del expander.                        (07/abr/22 22.31)

    public static TaskDialogButton ShowDialogExpander(string text, string heading, string caption,
                                                      TaskDialogButtonCollection buttons = null,
                                                      TaskDialogIcon icon = null,
                                                      bool? allowCancel = null,
                                                      TaskDialogExpander expander = null)
    {
        if (buttons == null)
        {
            buttons = CrearBotones(TaskDialogButton.OK);
        }
        else if (allowCancel.HasValue == false)
        {
            // Si existe el botón Cancel, permitir siempre que se cancele.
            // Salvo que se haya indicado un valor a allowCancel.
            if (buttons.Contains(TaskDialogButton.Cancel))
            {
                //if(allowCancel.HasValue == false)
                allowCancel = true;
            }
        }

        if (icon == null)
        {
            icon = TaskDialogIcon.ShieldWarningYellowBar;
        }

        TaskDialogPage page = DialogPage(text, heading, caption: caption,
                                         buttons: buttons,
                                         icon: icon,
                                         allowcancel: allowCancel == null ? false : allowCancel.Value, 
                                         expander: expander);
        return ShowDialog(page);
    }




    /// <summary>
    /// Mostrar un cuadro de diálogo con el tiempo de espera (en segundos) indicados.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="heading"></param>
    /// <param name="caption"></param>
    /// <param name="icon"></param>
    /// <param name="segundosEspera"></param>
    /// <returns></returns>
    public static TaskDialogButton MessageBoxTemporizadorShow(string text, string heading, string caption,
                                                              TaskDialogIcon icon = null, 
                                                              int segundosEspera = 20)
    {
        //if(botones == null)
        TaskDialogButton btnAceptar = TaskDialogButton.OK;
        // Usar un botón para detener o reanudar el temporizador. (07/abr/22 08.18)
        TaskDialogButton btnDetener = new TaskDialogButton() { Text="Detener / Reanudar", AllowCloseDialog=false};
        
        var botones = CrearBotones(btnAceptar, btnDetener);

        if (icon == null)
            icon = TaskDialogIcon.ShieldWarningYellowBar;

        var prgBar = new TaskDialogProgressBar()
        {
            State = TaskDialogProgressBarState.Paused
        };
        var page = DialogPage(text, heading, caption: caption,
                              buttons: botones,
                              icon: icon,
                              allowcancel: false, 
                              progressBar: prgBar);

        // Create a WinForms timer that raises the Tick event every tenth second.
        using var timer = new Timer()
        {
            Enabled = true,
            Interval = 100
        };

        if (segundosEspera < 5)
            segundosEspera = 5;

        int remainingTenthSeconds = segundosEspera * 10;
        var maximum = remainingTenthSeconds * 2;
        page.ProgressBar.Maximum = maximum;

        btnDetener.Click += (s, e) =>
        {
            if(timer.Enabled == true)
            {
                timer.Stop();
                // No se puede cambiar el texto.
                //btnDetener.Text = "Reanudar";
                //page.Text = text;
                page.ProgressBar.State = TaskDialogProgressBarState.Paused;
            }
            else
            {
                timer.Start();
                //btnDetener.Text = "Detener";
                page.ProgressBar.State = TaskDialogProgressBarState.Marquee;
            }
        };

        timer.Tick += (s, e) =>
        {
            remainingTenthSeconds--;
            if (remainingTenthSeconds > 0)
            {
                // Update the remaining time and progress bar.
                page.Text = string.Format("{1}\nTiempo restante {0} segundos...", (remainingTenthSeconds + 9) / 10, text);
                page.ProgressBar.Value = maximum - remainingTenthSeconds * 2;
            }
            else
            {
                // Stop the timer and click the "Reconnect" button - this will
                // close the dialog.
                timer.Enabled = false;
                btnAceptar.PerformClick();
            }
        };
        return ShowDialog(page);
    }

    //void algo2()
    //{
    //    var remainingTenthSeconds = 90;
    //    string textFormat ="{}";
    //    TaskDialogPage page;
    //    TaskDialogButton reconnectButton = null;


    //    // Set then maximum value for the ProgressBar
    //    var maximum = remainingTenthSeconds * 2;
    //    page.ProgressBar.Maximum = maximum;

    //    // Create a WinForms timer that raises the Tick event every tenth second.
    //    using (var timer = new Timer()
    //    {
    //        Enabled = true,
    //        Interval = 100
    //    })
    //    {
    //        timer.Tick += (s, e) =>
    //        {
    //            remainingTenthSeconds--;
    //            if (remainingTenthSeconds > 0)
    //            {
    //                // Update the remaining time and progress bar.
    //                page.Text = string.Format(textFormat, (remainingTenthSeconds + 9) / 10);
    //                page.ProgressBar.Value = maximum - remainingTenthSeconds * 2;
    //            }
    //            else
    //            {
    //                // Stop the timer and click the "Reconnect" button - this will
    //                // close the dialog.
    //                timer.Enabled = false;
    //                reconnectButton.PerformClick();
    //            }
    //        };

    //        TaskDialogButton result = TaskDialog.ShowDialog(this, page);
    //        if (result == reconnectButton)
    //            Console.WriteLine("Reconnecting.");
    //        else
    //            Console.WriteLine("Not reconnecting.");
    //    }

    //    // Create a WinForms timer that raises the Tick event every tenth second.
    //    using (var timer2 = new System.Windows.Forms.Timer()
    //    {
    //        Enabled = true,
    //        Interval = 100
    //    })
    //    {
    //        timer.Tick += (s, e) =>
    //        {
    //            remainingTenthSeconds--;
    //            if (remainingTenthSeconds > 0)
    //            {
    //                // Update the remaining time and progress bar.
    //                page.Text = string.Format(textFormat, (remainingTenthSeconds + 9) / 10);
    //                page.ProgressBar.Value = 100 - remainingTenthSeconds * 2;
    //            }
    //            else
    //            {
    //                // Stop the timer and click the "Reconnect" button - this will
    //                // close the dialog.
    //                timer.Enabled = false;
    //                reconnectButton.PerformClick();
    //            }
    //        };
    //    }
    //}

    /// <summary>
    /// Crear una colección de botones a partir de los botones indicados.
    /// </summary>
    /// <param name="botones">Los botones a asignar.</param>
    /// <returns>Una colección del tipo TaskDialogButtonCollection con los botones creados a partir de los indicados.</returns>
    public static TaskDialogButtonCollection CrearBotones(params TaskDialogButton[] botones)
    {
        var col = new TaskDialogButtonCollection();
        foreach (var button in botones)
            col.Add(button);
        return col;
    }

    /// <summary>
    /// Crear una colección de botones a partir de los textos indicados.
    /// </summary>
    /// <param name="textos">Los textos a asignar a los botones.</param>
    /// <returns>Una colección del tipo TaskDialogButtonCollection con los botones creados a partir de los textos.</returns>
    public static TaskDialogButtonCollection CrearBotones(params string[] textos)
    {
        var col = new TaskDialogButtonCollection();
        foreach (string texto in textos)
            col.Add(new TaskDialogButton(texto));
        return col;
    }

    /// <summary>
    /// Crear un botón con el texto indicado.
    /// </summary>
    /// <param name="texto">El texto del botón</param>
    /// <param name="allowCloseDialog">Si al pulsar en el botón se acepta lo pulsado.</param>
    /// <param name="showShieldIcon">Si se muestra el icono de que se requiere permisos (UAC) para esa acción.</param>
    /// <param name="enabled">Si estará habilitado.</param>
    /// <param name="visible">Si es visible.</param>
    /// <returns>Un objeto del tipo TaskDialogButton con el botón creado.</returns>
    public static TaskDialogButton CrearBoton(string texto, 
                                              bool allowCloseDialog = true, 
                                              bool showShieldIcon = false, 
                                              bool enabled = true, 
                                              bool visible = true)
    {
        return new TaskDialogButton() { 
            Text = texto, 
            AllowCloseDialog = allowCloseDialog, 
            ShowShieldIcon = showShieldIcon, 
            Enabled = enabled, 
            Visible = visible
        };
    }

    // Así no funciona
    //// Probar de esta forma, para que se pueda usar en la comprobación. (04/abr/22 22.31)
    //private static TaskDialogButton _ButtonSi = new TaskDialogButton() { Text = "Sí" };
    ////public static TaskDialogButton ButtonSi { get { return _ButtonSi; } }
    //public static TaskDialogButton ButtonSi
    //{
    //    get { return _ButtonSi; }
    //}

    // Los defino en una clase separada: DialogTaskButtons

    // Hay que devolver siempre un nuevo objeto, si no... dice que ya existe.
    // Aún así para poder comprobar si es el botón hay que asignarlo antes.
    //public static TaskDialogButton Si { get { return new TaskDialogButton() { Text = "Sí" }; } }
    //public static TaskDialogButton Cancelar { get { return new() { Text = "Cancelar" }; } }
    //public static TaskDialogButton Aceptar { get { return new() { Text = "Aceptar" }; } }
    //public static TaskDialogButton Reintentar { get { return new() { Text = "Reintentar" }; } }


    /// <summary>
    /// Crea una página para usar con los cuadros de diálogo de esta clase.
    /// </summary>
    /// <param name="text">El texto a mostrar.</param>
    /// <param name="heading">El texto de la cabecera.</param>
    /// <param name="caption">El texto del cuadro de diálogo, si es nulo se usa heading.</param>
    /// <param name="buttons">Los botones a mostrar, si es nulo se usa OK.</param>
    /// <param name="icon">El icono a mostrar, si es nulo no se muestra ninguno.</param>
    /// <param name="defaultButton">El botón predeterminado, si es nulo se usa el primero.</param>
    /// <param name="allowcancel">Si permite cancelar, predeterminado sí.</param>
    /// <param name="verification">CheckBox de verificación o nulo para no mostrarlo.</param>
    /// <param name="expander">El expander a mostrar o nulo para no mostrar.</param>
    /// <param name="footnote">El área de las notas al pie del diálogo.</param>
    /// <param name="progressBar">Un objeto de tipo TaskDialogProgressBar.</param>
    /// <param name="radioButtons">Una colección con los RadioButtons a mostrar.</param>
    /// <param name="sizeToContent">Si el ancho del diálogo se adapta al contenedor (como hace MessageBox).</param>
    /// <returns>Un objeto TaskDialogPage con las opciones indicadas.</returns>
    public static TaskDialogPage DialogPage(string text, string heading, 
                                            string caption = null, 
                                            TaskDialogButtonCollection buttons = null,
                                            TaskDialogIcon icon = null, 
                                            TaskDialogButton defaultButton = null,
                                            bool allowcancel = true,
                                            TaskDialogVerificationCheckBox verification = null,
                                            TaskDialogExpander expander = null, 
                                            TaskDialogFootnote footnote = null,
                                            TaskDialogProgressBar progressBar = null, 
                                            TaskDialogRadioButtonCollection radioButtons = null, 
                                            bool allowMinimize = false,
                                            bool sizeToContent = true)
    {
        var page = new TaskDialogPage()
        {
            Heading = heading,
            Text = text,
            Caption = caption ?? heading,
            Icon = icon ?? TaskDialogIcon.None,
            AllowCancel = allowcancel,
            Verification = verification,
            Buttons = buttons ?? new TaskDialogButtonCollection() { TaskDialogButton.OK },
            DefaultButton = defaultButton ?? buttons.FirstOrDefault(), 
            AllowMinimize = allowMinimize, 
            Expander = expander ?? default, 
            Footnote = footnote ?? default, 
            ProgressBar = progressBar ?? default,
            //RadioButtons = radioButtons ?? default, 
            SizeToContent = sizeToContent
        };
        if(radioButtons != null)
            page.RadioButtons = radioButtons;

        return page;
    }

    /// <summary>
    /// Muestra el diálogo según la página indicada.
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static TaskDialogButton ShowDialog(TaskDialogPage page)
    {
        return TaskDialog.ShowDialog(page);
    }

    /// <summary>
    /// Muestra un diálogo con los parámetros indicados.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="caption"></param>
    /// <param name="heading"></param>
    /// <param name="button"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static TaskDialogButton ShowDialog(string text, string caption,
                                              string heading = null,
                                              TaskDialogButton button = null,
                                              TaskDialogIcon icon = null)
    {
        // Show a task dialog (simple).
        var botones = new TaskDialogButtonCollection
        {
            button
        };
        return ShowDialog(text, caption, heading,  botones, icon);
    }

    /// <summary>
    /// Muestra un diálogo según los parámetros indicados.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="caption"></param>
    /// <param name="heading"></param>
    /// <param name="buttons"></param>
    /// <param name="icon"></param>
    /// <param name="defaultButton"></param>
    /// <param name="allowcancel"></param>
    /// <param name="verification"></param>
    /// <returns></returns>
    public static TaskDialogButton ShowDialog(string text, string caption, 
                                              string heading = null,
                                              TaskDialogButtonCollection buttons = null,
                                              TaskDialogIcon icon = null, 
                                              TaskDialogButton defaultButton = null, 
                                              bool allowcancel = true, 
                                              TaskDialogVerificationCheckBox verification = null)
    {
        // Show a task dialog (simple).
        TaskDialogButton result = TaskDialog.ShowDialog(new TaskDialogPage()
        {
            Text = text,
            Caption = caption,
            Heading = heading ?? caption,
            Buttons = buttons ?? new TaskDialogButtonCollection() { TaskDialogButton.OK },
            Icon = icon ?? TaskDialogIcon.None,
            DefaultButton = defaultButton ?? buttons.FirstOrDefault(),
            AllowCancel = allowcancel,
            Verification = verification
        });

        return result;

        //// Show a task dialog (enhanced).
        //var page = new TaskDialogPage()
        //{
        //    Heading = heading,
        //    Text = text,
        //    Caption = caption,
        //    Icon = icon,
        //    AllowCancel = allowcancel,
        //    Verification = verification,
        //    Buttons = buttons,
        //    DefaultButton = TaskDialogButton.No
        //};

        //var resultButton = TaskDialog.ShowDialog(page);

        //if (resultButton == TaskDialogButton.Yes)
        //{
        //    if (page.Verification.Checked)
        //        Console.WriteLine("Do not show this confirmation again.");

        //    Console.WriteLine("User confirmed to stop the operation.");
        //}
    }

    // Convertido a C# desde el de Visual Basic.                (06/abr/22 19.31)

    /// <summary>
    /// Muestra un mensaje de aviso para recordar o no lo indicado.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="caption"></param>
    /// <param name="verificarText"></param>
    /// <param name="botones"></param>
    /// <param name="icon"></param>
    /// <param name="allowCancel"></param>
    /// <returns></returns>
    public static (TaskDialogButton Boton, bool Recordar) 
        MessageBoxVerificarShow(string text, string caption, 
                                string verificarText = "No volver a preguntar", 
                                TaskDialogButtonCollection botones = null, 
                                TaskDialogIcon icon = null, 
                                bool allowCancel = false)
    {
        bool recordar;
        TaskDialogVerificationCheckBox chkRecordar = new() { Checked = false, Text = verificarText };
        if (botones == null)
            botones = CrearBotones(TaskDialogButton.Yes, TaskDialogButton.No);
        
        var page = DialogPage(text, caption, buttons: botones, verification: chkRecordar, icon: icon, 
                              allowcancel:allowCancel);
        
        var res = ShowDialog(page);
        // Si se permite cancelar y se ha cancelado, ignorar el valor de recordar
        if(allowCancel && res == TaskDialogButton.Cancel)
        {
            recordar = false;
        }
        else
        {
            recordar = page.Verification.Checked;
        }
        
        return (res, recordar);
    }

    //public static TaskDialogItem ShowTaskDialog(string title, string caption, params TaskDialogItem[] items)
    //{
    //    var taskDialog = new TaskDialog { Owner = this, Items = items, Icon = this.Icon, Title = title, Caption = caption };
    //    if (taskDialog.ShowDialog() == true)
    //        return taskDialog.SelectedItem;
    //    else
    //        return null;
    //}

}

public static class DialogTaskButtons
{
    /// <summary>
    /// Yes en inglés.
    /// </summary>
    public static TaskDialogButton Si { get { return new TaskDialogButton() { Text = "Sí" }; } }
    /// <summary>
    /// Cancel en inglés.
    /// </summary>
    public static TaskDialogButton Cancelar { get { return new() { Text = "Cancelar" }; } }
    /// <summary>
    /// OK en inglés.
    /// </summary>
    public static TaskDialogButton Aceptar { get { return new() { Text = "Aceptar" }; } }
    /// <summary>
    /// Retry o TryAgain en inglés.
    /// </summary>
    public static TaskDialogButton Reintentar { get { return new() { Text = "Reintentar" }; } }
    /// <summary>
    /// No en inglés.
    /// </summary>
    public static TaskDialogButton No { get { return new TaskDialogButton() { Text = "No" }; } }
    /// <summary>
    /// Abort en inglés.
    /// </summary>
    public static TaskDialogButton Anular { get { return new TaskDialogButton() { Text = "Anular" }; } }
    /// <summary>
    /// Ignore en inglés.
    /// </summary>
    public static TaskDialogButton Ignorar { get { return new TaskDialogButton() { Text = "Ignorar" }; } }
    /// <summary>
    /// Continue en inglés.
    /// </summary>
    public static TaskDialogButton Continuar { get { return new TaskDialogButton() { Text = "Continuar" }; } }
    /// <summary>
    /// TryAgain en inglés.
    /// </summary>
    /// <remarks>Muestra Try Again en inglés pero en Windows en español muestra Reintentar.</remarks>
    //[Obsolete("Usar Reintentar en vez de este. Salvo si quieres que devuelva el mismo resultado que TryAgain.")]
    public static TaskDialogButton IntentarNuevamente { get { return new TaskDialogButton() { Text = "Re-Intentar" }; } }
    //public static TaskDialogButton IntentarNuevamente { get { return new TaskDialogButton() { Text = "Volver a probar" }; } }
}
