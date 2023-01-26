//-----------------------------------------------------------------------------
// Eliza claves para C#                                             (26/ene/23)
//
// Convertido a C# a partir de la versión de Visual Basic .NET
// A su vez basada en Eliza para Visual Basic de 1998, 2002
//
// ©Guillermo Som (Guille), 1998-2002, 2023
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ElizaVB;

namespace ElizaNETCS
{
    public partial class Eliza_claves : Form
    {
        //public Eliza_claves()
        //{
        //    InitializeComponent();
        //}
        public Eliza_claves(cEliza eli)
        {
            InitializeComponent();
            Eliza=eli;
        }

        private cEliza Eliza;

        private cEliza.eTiposDeClaves[] clavesTag = new[] { cEliza.eTiposDeClaves.eClaves, cEliza.eTiposDeClaves.eVerbos, cEliza.eTiposDeClaves.eRS, cEliza.eTiposDeClaves.eSimp, cEliza.eTiposDeClaves.eRec, cEliza.eTiposDeClaves.eBU };

        private void Combo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal i;
            int j;
            string sClaves = "";

            j = Combo1.SelectedIndex;
            if (j == -1)
            {
                Combo1.SelectedIndex = 0;
                return;
            }
            var que = clavesTag[j];
            i = AsignarPalabras(_List1_0, _List1_1, que, ref sClaves);
            // j = (i - Fix(i)) * 1000
            // i = Fix(i)
            j = System.Convert.ToInt32((i - Math.Floor(i)) * 1000);
            i = Math.Floor(i);
            _Label1_0.Text = i + " claves principales";
            _Label1_1.Text = j + " claves extras";
            List2.Items.Clear();
            _Label1_3.Text = "";
        }

        //private bool inicializando;

        private void Eliza_claves_Load(object sender, EventArgs e)
        {
            //inicializando = true;
            _Label1_2.Text = "Cargando el formulario...";
            Timer1.Interval = 300;
            Timer1.Enabled = true;
        }

        // Private Sub Eliza_claves_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        // ' Adaptar el tamaño y posición de los controles,		(25/ene/23 08.50)
        // ' están con Anchor a los ancho
        // If inicializando Then Return
        // inicializando = True

        // Dim l2l = List2.Left
        // Dim l2w = List2.Width - 246
        // 'Dim l3w = List2.Width - List2.Left
        // Combo1.Width = l2w
        // _List1_0.Width = l2w
        // _List1_1.Width = l2w
        // _Label1_0.Width = l2w
        // _Label1_1.Width = l2w

        // 'List2.Left = Combo1.Width + 12
        // 'List2.Width -= (List2.Left - l2l)

        // 'List2.Left = Combo1.Width + 12
        // 'List2.Width -= (List2.Left - l2l)
        // '_Label1_2.Left = List2.Left
        // '_Label1_3.Left = List2.Left
        // '_Label1_2.Width = List2.Width
        // '_Label1_3.Width = List2.Width
        // inicializando = False
        // End Sub

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Inicializar();
            //inicializando = false;
        }

        private void Inicializar()
        {
            // Comprobar si hay que inicializar la variable Eliza
            _Label1_2.Text = "";
            // If Eliza IsNot Nothing Then
            if (Eliza == null)
            {
                // _Label1_2.Text = "Palabras previamente asignadas"
                // Else
                Show();
                _Label1_2.Text = "Un momento, mientras cargo la lista de claves...";
                // Crear el objeto
                Eliza = new cEliza(fEliza.AppPath());
                var sw = Stopwatch.StartNew();
                Eliza.Inicializar();
                sw.Stop();
                _Label1_2.Text = "Tiempo en inicializar (y asignar las palabras): " + sw.Elapsed.ToString(@"mm\:ss\.fff"); // .Seconds & " segundos."
            }
            {
                Combo1.Items.Add("Claves");
                Combo1.Items.Add("Verbos");
                Combo1.Items.Add("RS (reglas simplif.)");
                Combo1.Items.Add("Simp (simpl. en respuesta)");
                Combo1.Items.Add("Recordar lo que dijo el user");
                Combo1.Items.Add("Base datos user");
            }

            Combo1.SelectedIndex = 0;
            Application.DoEvents();
        }

        private void List1_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal i =0;
            int j;

            var list1 = sender as ListBox;
            if (list1 == null)
                return;

            j = Combo1.SelectedIndex;
            if (j == -1)
            {
                Combo1.SelectedIndex = 0;
                return;
            }
            _Label1_2.Text = "Asignando las palabras...";
            if (clavesTag[j] == cEliza.eTiposDeClaves.eClaves)
            {
                {
                    if (list1.SelectedIndex > -1)
                    {
                        string sClave = list1.Items[list1.SelectedIndex].ToString();
                        if (list1 == _List1_0)
                            i = AsignarPalabras(List2, _List1_1, cEliza.eTiposDeClaves.eExtras, ref sClave);
                        else
                            i = AsignarPalabras(List2, _List1_1, cEliza.eTiposDeClaves.eExtras2, ref sClave);
                        _Label1_2.Text = sClave;
                    }
                    j = System.Convert.ToInt32((i - Math.Floor(i)) * 1000);
                    i = Math.Floor(i);
                    _Label1_3.Text = i.ToString() + " Extra" + (i != 1 ? "s" : "") + ", " + j.ToString() + " Respuesta" + (j != 1 ? "s" : "");
                }
            }
            toolTip1.SetToolTip(_Label1_2, _Label1_2.Text);
        }

        private decimal AsignarPalabras(ListBox unList, ListBox unList1, cEliza.eTiposDeClaves esClave, ref string sClave)
        {
            // Asigna las palabras a los listbox del form
            // Devolverá el número de palabras asignadas al ListBox
            cRegla tRegla;
            //cRespuestas tRespuestas;
            //cContenido tContenido;
            int i=0, j=0;
            var m_col = Eliza.ColReglas;

            unList.Items.Clear();
            switch (esClave)
            {
                case cEliza.eTiposDeClaves.eClaves:
                    {
                        unList1.Items.Clear();
                        foreach (var tRegla1 in m_col.Values)
                        {
                            unList.Items.Add(tRegla1.Contenido);
                            i += 1;
                            foreach (var tRespuestas in tRegla1.Extras.Valores) // .Values
                            {
                                unList1.Items.Add(tRespuestas.Contenido);
                                j += 1;
                            }
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eExtras:
                    {
                        tRegla = m_col[sClave];
                        if (tRegla.Extras.Count > 0)
                        {
                            unList.Items.Add("---Extras---");
                            foreach (var tRespuestas in tRegla.Extras.Valores) // .Values
                            {
                                unList.Items.Add(tRespuestas.Contenido);
                                i += 1;
                            }
                        }
                        if (tRegla.Respuestas.Count > 0)
                        {
                            unList.Items.Add("---Respuestas---");
                            foreach (var tContenido in tRegla.Respuestas.Valores) // .Values
                            {
                                unList.Items.Add(tContenido.Contenido);
                                j += 1;
                            }
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eExtras2:
                    {
                        foreach (var tRegla1 in m_col.Values)
                        {
                            if (tRegla1.Extras.Count > 0)
                            {
                                foreach (var tRespuestas in tRegla1.Extras.Valores) // .Values
                                {
                                    if (tRespuestas.Contenido == sClave)
                                    {
                                        foreach (var tContenido in tRespuestas.Valores) // .Values
                                        {
                                            unList.Items.Add(tContenido.Contenido);
                                            j += 1;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eVerbos:
                    {
                        foreach (var tContenido in Eliza.ColVerbos.Valores) // .Values
                        {
                            unList.Items.Add(tContenido.ID + " -- " + tContenido.Contenido);
                            i += 1;
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eRS:
                    {
                        foreach (var tContenido in Eliza.ColRS.Valores) // .Values
                        {
                            unList.Items.Add(tContenido.ID + " -- " + tContenido.Contenido);
                            i += 1;
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eSimp:
                    {
                        foreach (var tContenido in Eliza.ColSimp.Valores) // .Values
                        {
                            unList.Items.Add(tContenido.ID + " -- " + tContenido.Contenido);
                            i += 1;
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eRec:
                    {
                        foreach (var tContenido in Eliza.ColRec.Valores) // .Values
                        {
                            unList.Items.Add(tContenido.ID + " -- " + tContenido.Contenido);
                            i += 1;
                        }

                        break;
                    }

                case cEliza.eTiposDeClaves.eBU:
                    {
                        foreach (var tContenido in Eliza.ColBaseUser.Valores) // .Values
                        {
                            unList.Items.Add(tContenido.ID + " -- " + tContenido.Contenido);
                            i += 1;
                        }

                        break;
                    }
            }
            return System.Convert.ToDecimal(i + j / (double)1000);
        }
    }
}
