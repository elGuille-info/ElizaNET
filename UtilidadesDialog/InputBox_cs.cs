//-----------------------------------------------------------------------------
// InputBox para C#                                                 (25/ene/23)
//
// Basado en el ejemplo de:
// InputBox [C#]
// By Jan Slama, 17-Sep-2008
// https://www.csharp-examples.net/inputbox/
//
// ©Guillermo Som (Guille), 2023
//-----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Drawing;


namespace UtilidadesDialog;

public class UtilDialog
{
    public static DialogResult InputBox(string promptText, string title, ref string value)
    {
        Form form = new Form();
        Label label = new Label();
        TextBox textBox = new TextBox();
        Button buttonOk = new Button();
        Button buttonCancel = new Button();

        int h = 250;

        form.Text = title;
        label.Text = promptText;
        textBox.Text = value;

        buttonOk.Text = "OK";
        buttonCancel.Text = "Cancel";
        buttonOk.DialogResult = DialogResult.OK;
        buttonCancel.DialogResult = DialogResult.Cancel;

        //label.SetBounds(9, 20, 372, 13);
        //textBox.SetBounds(12, 36, 372, 20);
        //buttonOk.SetBounds(228, 72, 75, 23);
        //buttonCancel.SetBounds(309, 72, 75, 23);
        label.SetBounds(12, 12, 550, h - 150);
        textBox.SetBounds(12, h - 112, 570, 60);
        buttonOk.SetBounds(500, h - 55, 100, 35);
        buttonCancel.SetBounds(500 - 12, h - 55, 100, 35);
        
        //buttonCancel.Left = 600 - 75;
        buttonOk.Left = buttonCancel.Left - buttonOk.Width -12;
        //buttonCancel.Top = 400;
        //buttonOk.Top = buttonCancel.Top;

        label.AutoSize = false; // true;
        //label.Anchor = label.Anchor | AnchorStyles.Right;
        //textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
        buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

        //form.ClientSize = new Size(396, 107);
        form.BackColor = Color.White;
        form.ClientSize = new Size(600, h);
        form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
        //form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MinimizeBox = false;
        form.MaximizeBox = false;
        form.AcceptButton = buttonOk;
        form.CancelButton = buttonCancel;

        DialogResult dialogResult = form.ShowDialog();
        value = textBox.Text;
        return dialogResult;
    }
}

