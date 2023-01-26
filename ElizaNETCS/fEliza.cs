using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElizaNETCS
{
    public partial class fEliza : Form
    {
        public fEliza()
        {
            InitializeComponent();
        }

        public static string AppPath()
        {
            // Devuelve el path del ejecutable con la barra final            (15/Sep/02)
            // Return App.Path & IIf(Right$(App.Path, 1) = "\", "", "\")
            var ensamblado = typeof(fEliza).Assembly;
            var elPath = System.IO.Path.GetDirectoryName(ensamblado.Location);
            return elPath + (elPath.EndsWith(@"\") ? "" : @"\");
        }
    }
}
