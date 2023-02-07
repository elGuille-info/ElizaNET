//-------------------------------------------------------------------------------
// Mostrar el resumen del texto analizado                       (06/feb/23 21.40)
//
// (c)Guillermo Som (Guille), 2023
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Eliza_gcnl;

namespace UtilidadesDialog
{
    public partial class TextoAnalizado : Form
    {
        private Frases frase = null;
        //private string text, ultimaOriginal;

        public TextoAnalizado(Frases laFrase)
        {
            InitializeComponent();

            frase = laFrase;

            // Asignar las frases y seleccionar la indicada
            int index = -1;
            ComboTextos.Items.Clear();
            for (int i = 0; i < Frases.LasFrases.Count; i++)
            {
                var f = Frases.LasFrases[i];
                ComboTextos.Items.Add(f);
                if (f.Texto == frase.Texto) { index = i; }
            }
            if (index > -1) { ComboTextos.SelectedIndex = index; }
        }

        private void TextoAnalizado_Load(object sender, EventArgs e)
        {
            button5_Click(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TxtResultado.Text = Frases.MostrarResumen(true);
            TxtResultado.SelectionStart = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TxtResultado.Text = frase.Analizar(conTokens: true, soloEntities: false);
            // Mostrar el texto desde el principio
            TxtResultado.SelectionStart = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TxtResultado.Text = frase.Analizar(conTokens: false, soloEntities: false);
            TxtResultado.SelectionStart = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TxtResultado.Text = frase.MostrarTokens();
            TxtResultado.SelectionStart = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TxtResultado.Text = frase.Analizar(conTokens: false, soloEntities: true);
            TxtResultado.SelectionStart = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TxtResultado.Text = Frases.MostrarResumen(false);
            TxtResultado.SelectionStart = 0;
        }

        private void ComboTextos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboTextos.SelectedIndex != -1)
            {
                frase = ComboTextos.SelectedItem as Frases;
                button2_Click(null, null);
            }
        }
    }
}
