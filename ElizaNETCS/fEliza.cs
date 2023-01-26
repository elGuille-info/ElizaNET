//-----------------------------------------------------------------------------
// Eliza para C#                                                    (26/ene/23)
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
    public partial class fEliza : Form
    {
        public fEliza()
        {
            InitializeComponent();

            Eliza = null;
        }

        private const string CrLf = "\r\n";

        private string sNombre = ""; // asignarle una cadena vacía, 24-ene-2023 12.24
        // private cEliza.eSexo m_Sexo;

        // Private WithEvents Eliza As cEliza
        private cEliza Eliza;
        private bool m_Terminado;
        // Private m_Idioma As eIdioma

        private bool SesionGuardada;
        private string sEntradaAnterior = "";

        private void cmdNuevo_Click(object sender, EventArgs e)
        {
            cEliza.eSexo tSexo = cEliza.eSexo.Masculino;
            string sSexo;
            int tmpSexo;
            string sMsgTmp;
            Random m_rnd = new Random();

            // Si no se ha guardado la sesión anterior, preguntar si se quiere guardar.
            if (!SesionGuardada)
            {
                // If MsgBox(sNombre & " ¿Quieres guardar el contenido de la sesión actual?",
                // MsgBoxStyle.YesNo,
                // "Guardar la sesión actual") = MsgBoxResult.Yes Then
                // GuardarSesion()
                // End If
                if (Dialogos.MessageBoxShow(sNombre + " ¿Quieres guardar el contenido de la sesión actual?", "Guardar la sesión actual", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    GuardarSesion();
            }
            SesionGuardada = true;

            txtSalida.Text = "";
            txtEntrada.Text = "> ";

            sMsgTmp = "";
            // Preguntar el nombre y el sexo
            do
            {
                // sNombre = InputBox("Por favor dime tu nombre, o la forma en que quieres que te llame, (deja la respuesta en blanco para terminar)", "Saber quién eres", sNombre)
                if (UtilidadesDialog.UtilDialog.InputBox("Por favor dime tu nombre, o la forma en que quieres que te llame, (deja la respuesta en blanco para terminar)", "Saber quién eres", ref sNombre) != DialogResult.OK)
                    sNombre = "";
                sNombre = sNombre.Trim();
                if (sNombre.Length == 0)
                {
                    m_Terminado = false;
                    sMsgTmp = "Adios, hasta la próxima sesión.";
                    break;
                }
                // Comprobar si está en la lista
                tmpSexo = -1;
                {
                    var withBlock = List2;
                    for (var i = 0; i <= withBlock.Items.Count - 1; i++)
                    {
                        // If VB6.GetItemString(List2, i) = sNombre Then
                        if (List2.Items[i].ToString() == sNombre)
                        {
                            // tmpSexo = VB6.GetItemData(List2, i)
                            tmpSexo = i;
                            break;
                        }
                    }
                }

                if (tmpSexo > -1)
                {
                    // MsgBox "Hola de nuevo " & sNombre & ", empezamos cuando quieras."
                    // ImprimirDOS "Hola de nuevo " & sNombre & ", empezamos cuando quieras."
                    tSexo = (cEliza.eSexo)tmpSexo;
                    break;
                }
                // tener una serie de nombres para no pecar de "tonto"
                sSexo = "masculino";
                tSexo = cEliza.eSexo.Masculino;
                if (sNombre.Length > 0)
                {
                    tSexo = SexoNombre();
                    sSexo = tSexo == cEliza.eSexo.Femenino ? "femenino" : "masculino";
                    if (tSexo == cEliza.eSexo.Ninguno)
                    {
                        if (sNombre.EndsWith("a"))
                        {
                            sSexo = "femenino";
                            tSexo = cEliza.eSexo.Femenino;
                        }
                        else
                        {
                            sSexo = "masculino";
                            tSexo = cEliza.eSexo.Masculino;
                        }

                        if (Dialogos.MessageBoxShow(sNombre + " por favor confirmame que tu sexo es: " + sSexo, "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            if (tSexo == cEliza.eSexo.Femenino)
                            {
                                tSexo = cEliza.eSexo.Masculino;
                                sSexo = "masculino";
                            }
                            else
                            {
                                tSexo = cEliza.eSexo.Femenino;
                                sSexo = "femenino";
                            }
                        }
                    }
                }

                if (Dialogos.MessageBoxShow("Por favor confirma que estos datos son correctos:" + CrLf + "Nombre: " + sNombre + CrLf + "Sexo: " + sSexo, "", MessageBoxButtons.YesNoCancel) != DialogResult.No)
                    break;
            }
            while (true);
            // Si no se escribe el nombre, terminar el programa
            if (sNombre.Length == 0)
            {
                if (sMsgTmp.Length == 0)
                    sMsgTmp = "Me parece que no confías lo suficiente en mí, así que no hay sesión que valga, ¡ea!";
                Dialogos.MessageBoxShow(sMsgTmp, "", MessageBoxButtons.OK);
                // MsgBox(sMsgTmp)
                Close();
                return;
            }

            // Comprobar si está en la lista
            tmpSexo = -1;
            {
                var withBlock = List2;
                for (var i = 0; i <= withBlock.Items.Count - 1; i++)
                {
                    // If VB6.GetItemString(List2, i) = sNombre Then
                    if (List2.Items[i].ToString() == sNombre)
                    {
                        tmpSexo = i;
                        break;
                    }
                }
            }
            if (tmpSexo == -1)
            {
                // añadirlo
                {
                    var withBlock = List2;
                    // .Items.Add(New VB6.ListBoxItem(sNombre, tSexo))
                    withBlock.Items.Add(sNombre);
                }
                // guardar los nombres
                GuardarNombres();
            }

            List1.Items.Clear();
            List1.Items.Add($"Sesión iniciada el: {DateTime.Now}");
            List1.Items.Add("-----------------------------------------------");

            sMsgTmp = "Hola " + sNombre + ", soy Eliza para Visual Basic";
            ImprimirDOS(sMsgTmp);
            sMsgTmp = "Por favor, intenta evitar los monosílabos y tuteame, yo así lo haré.";
            ImprimirDOS(sMsgTmp);

            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            // Inicializar los valores, mientras el usuario escribe
            {
                var withBlock = Eliza;
                if (withBlock.Iniciado)
                {
                    // Si es la primera vez... dar un poco de tiempo.
                    // i = CShort(Rnd() * 9 + 1)
                    switch (m_rnd.Next(10))
                    {
                        case object _ when m_rnd.Next(10) > 6:
                            {
                                sMsgTmp = "Espera un momento, mientras ordeno mi base de datos...";
                                break;
                            }

                        case object _ when m_rnd.Next(10) > 3:
                            {
                                sMsgTmp = "Espera un momento, mientras busco un bolígrafo...";
                                break;
                            }

                        default:
                            {
                                sMsgTmp = "Espera un momento, mientras lleno mis chips de conocimiento...";
                                break;
                            }
                    }
                    ImprimirDOS(sMsgTmp);
                }
                withBlock.Sexo = tSexo;
                withBlock.Nombre = sNombre;
                var sw = Stopwatch.StartNew();
                withBlock.Inicializar();
                sw.Stop();
                if (!withBlock.Iniciado)
                {
                    // sMsgTmp = "Tiempo en inicializar (y asignar las palabras): " & sw.Elapsed.Seconds & " segundos."
                    sMsgTmp = "Tiempo en inicializar (y asignar las palabras): " + sw.Elapsed.ToString(@"mm\:ss\.fff");
                    ImprimirDOS(sMsgTmp);
                }
                else
                {
                    // sMsgTmp = "Tiempo en re-inicializar las palabras: " & sw.Elapsed.Seconds & " segundos."
                    sMsgTmp = "Tiempo en re-inicializar las palabras: " + sw.Elapsed.ToString(@"mm\:ss\.fff");
                    ImprimirDOS(sMsgTmp);
                }
            }
            Cursor = System.Windows.Forms.Cursors.Default;

            txtEntrada.Text = "> ";
            txtEntrada.SelectionStart = 2;

            sMsgTmp = "Vamos a ello..., ¿en qué puedo ayudarte?";
            ImprimirDOS(sMsgTmp);
            if (txtEntrada.Visible)
                txtEntrada.Focus();
        }

        // Private Sub Eliza_Terminado() Handles Eliza.Terminado
        // 'El usuario ha decidido terminar
        // m_Terminado = True
        // End Sub

        private void fEliza_Load(object sender, EventArgs e)
        {
            Timer1.Interval = 300;
            Timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Inicializar();
        }

        private void Inicializar()
        {
            // Inicializar las variables
            txtSalida.Text = "";
            {
                var withBlock = txtEntrada;
                withBlock.Text = "";
                withBlock.TabIndex = 0;
                withBlock.Left = txtSalida.Left;
            }

            SesionGuardada = true;

            List2.Items.Clear();

            if (DateTime.Now.Year > 2023)
                LabelInfo.Text = "Eliza para C# ©Guillermo Som (Guille), 1998-2002, 2023-" + DateTime.Now.Year.ToString();
            else
                // LabelInfo.Text = "Eliza para Visual Basic © Guillermo 'guille' Som, 1998-2002, 2023"
                LabelInfo.Text = "Eliza para C# ©Guillermo Som (Guille), 1998-2002, 2023";

            Show();

            Eliza = new cEliza(AppPath())
            {
                Sexo = cEliza.eSexo.Ninguno
            };

            // leer la lista de nombres que han usado el programa
            LeerNombres();
            // Empezar una nueva sesión
            cmdNuevo_Click(cmdNuevo, new System.EventArgs());
        }

        public void mnuEliza_claves_Click(object sender, EventArgs e)
        {
            Hide();
            Eliza_claves fClaves = new Eliza_claves(Eliza); // With {.Eliza = Eliza}
            fClaves.ShowDialog();
            // With Eliza_claves
            // .Eliza = Eliza
            // .ShowDialog()
            // End With
            Show();
        }

        public void mnuEstadísticas_Click(object sender, EventArgs e)
        {
            cRespuestas tRespuestas;
            //cContenido tContenido;
            string sMsg;

            tRespuestas = Eliza.Estadísticas();
            sMsg = "Datos estadísticos de Eliza para Visual Basic:" + CrLf + CrLf;
            foreach (var tContenido in tRespuestas.Valores) // .Values
                sMsg = sMsg + tContenido.ID + " = " + tContenido.Contenido + CrLf;

            Dialogos.MessageBoxShow(sMsg, "Estadísticas de Eliza", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void mnuAcercaDe_Click(object sender, EventArgs e)
        {
            // Mostrar la información del programa
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            var ensamblado = typeof(fEliza).Assembly;
            var fvi = FileVersionInfo.GetVersionInfo(ensamblado.Location);

            //msg.AppendLine("Eliza para Visual Basic,");
            msg.Append("Eliza para C#,");
            msg.AppendLine($"versión {fvi.ProductVersion} ({fvi.FileVersion})");
            msg.AppendLine(fvi.ProductName);
            msg.AppendLine($"{fvi.LegalCopyright}");
            msg.AppendLine();
            msg.AppendLine($"{fvi.Comments}");
            msg.AppendLine("Versión para VB5    iniciada el Sábado, 30/May/1998 17:30");
            msg.AppendLine("Versión para VB6    iniciada el Miércoles, 18/Sep/2002 04:30");
            msg.AppendLine("Versión para VB.NET iniciada el Domingo, 22/Ene/2023 10:08");
            msg.AppendLine("Versión para C#     iniciada el Jueves, 26/Ene/2023 19:20");
            msg.AppendLine();
            msg.AppendLine("La idea del formato de las reglas y simplificación de entradas, están basadas en 'ELIZA in Prolog' de Viren Patel.");
            msg.AppendLine();
            msg.Append("Agradecimiento especial a Svetlana por toda la información aportada, ");
            msg.Append("además de ampliar la base de conocimientos de Eliza y darle ");
            msg.Append("un toque más femenino del que yo jamás le hubiese podido dar... ");
            msg.AppendLine("y, sobre todo, por motivarme a hacer este programa, sin su ayuda no hubiera sido posible...");

            Dialogos.MessageBoxShow(msg.ToString(), "Acerca de Eliza para C#", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void mnuFileReleer_Click(object sender, EventArgs e)
        {
            // Releer el fichero de palabras,
            // esto es útil por si se añaden nuevas
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Eliza.Releer();
            Cursor = System.Windows.Forms.Cursors.Default;
        }

        public void mnuSalir_Click(object sender, EventArgs e)
        {
            // Si no se ha guardado la sesión anterior, preguntar            (16/Sep/02)
            // si se quiere guardar.
            if (!SesionGuardada)
            {
                if (Dialogos.MessageBoxShow(sNombre + " ¿Quieres guardar el contenido de la sesión actual?", "Guardar la sesión actual", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    GuardarSesion();
            }
            SesionGuardada = true;

            this.Close();
        }

        // Revisar esto, que no lo analiza...						(24/ene/23 13.35)
        private void ProcesarEntrada()
        {
            // Toma lo que ha escrito el usuario y lo envía a la clase
            // para procesar lo escrito y obtener la respuesta
            // Static sEntradaAnterior As String
            string sTmp;

            if (string.IsNullOrEmpty(txtEntrada.Text))
                sTmp = "";
            else
                sTmp = txtEntrada.Text.Trim();

            // Sólo si se ha escrito algo
            if (string.IsNullOrEmpty(sTmp) == false)
            {
                txtEntrada.Text = "> ";
                txtEntrada.SelectionStart = 2;

                bool noRepitas = false;
                if (sEntradaAnterior == sTmp)
                    noRepitas = true;

                // Mostrar el aviso que no te repitas si no es no, sí, etc.

                // Las palabras que se pueden repetir están en Eliza.SiNo
                // If Eliza.SiNo.Contains(sTmp.ToLower()) Then
                // noRepitas = False
                // End If
                // Simplificando:
                // si la respuesta tiene menos de 5 caracteres no considerarlo repetición
                if (sTmp.Length < 5)
                    noRepitas = false;
                // If sAnt = sTmp Then
                if (noRepitas)
                    ImprimirDOS("Por favor, no te repitas.");
                else
                {
                    // guardar la última entrada
                    sEntradaAnterior = sTmp;
                    // mostrar en la lista lo que se ha escrito
                    ImprimirDOS(sTmp);
                    if (sTmp.StartsWith("> "))
                        sTmp = sTmp.Substring(2).TrimStart();
                    // si se escribe ?, -?, --?, -h o --help			(24/ene/23 17.58)
                    // mostrar la ayuda de comandos en principio ponerlo en modo consulta.
                    var losHelp = new string[] { "?", "-?", "--?", "-h", "--h", "--help", "-help" };
                    if (losHelp.Contains(sTmp))
                    {
                        ImprimirDOS("No hay ayuda definida, puedes usar *consulta* para buscar en las palabras clave.");
                        return;
                    }
                    // Si se escribe *consulta*, usar lo que viene después
                    // para buscar en las palabras claves de Eliza.
                    // De esa comprobación se encarga cEliza.ProcesarEntrada
                    // Una vez que se entra en el modo de consulta, lo que
                    // se escriba se buscará en las claves y sub-claves,
                    // para salir del modo consulta, hay que escribir de
                    // nuevo *consulta*
                    // 
                    // Procesar la entrada del usuario y
                    // mostrar la respuesta de Eliza
                    sTmp = Eliza.ProcesarEntrada(sTmp);
                    ImprimirDOS(sTmp);
                    // ---Esto tampoco soluciona el GPF
                    Application.DoEvents();

                    if (sTmp.StartsWith("adios", StringComparison.OrdinalIgnoreCase))
                    {
                        sEntradaAnterior = "";
                        SesionGuardada = false;
                        m_Terminado = false;
                        cmdNuevo_Click(cmdNuevo, new System.EventArgs());
                    }
                    // Por ahora para que C# no de un warning...
                    if (m_Terminado)
                    {
                        sEntradaAnterior = "";
                        SesionGuardada = false;
                        m_Terminado = false;
                        cmdNuevo_Click(cmdNuevo, new System.EventArgs());
                    }
                }
            }
            else
                ImprimirDOS("¿Te lo estás pensando?");

            SesionGuardada = false;
        }

        private void GuardarSesion()
        {
            // preguntar el nombre del fichero
            // o crearlo automáticamente

            // sFic = AppPath() & "sesiones\" & sNombre & "_" & VB6.Format(Now, "ddmmmyyyy_hhmm") & ".txt"
            var sDir = System.IO.Path.Combine(AppPath(), "sesiones");
            // sFic = AppPath() & "sesiones\" & sNombre & "_" & Date.Now.ToString("ddMMMyyyy_HHmm") & ".txt"
            var sFic = System.IO.Path.Combine(sDir, sNombre + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".txt");
            // sFic = InputBox(sNombre & " escribe el nombre del fichero:", "Guardar sesión", sFic)
            if (UtilidadesDialog.UtilDialog.InputBox(sNombre + " escribe el nombre del fichero:", "Guardar sesión", ref sFic) != DialogResult.OK)
                sFic = "";
            else
                sFic = sFic.Trim();
            if (string.IsNullOrEmpty(sFic) == false)
            {
                List1.Items.Add("-----------------------------------------------");
                List1.Items.Add($"Sesión guardada el: {DateTime.Now.ToString("dddd, dd/MMM/yyyy HH:mm")}");

                // Crear los directorios indicados en el nombre del archivo  (18/Sep/02)
                // crearDirectorios(sFic)
                if (System.IO.Directory.Exists(sDir) == false)
                    System.IO.Directory.CreateDirectory(sDir);

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sFic, false, System.Text.Encoding.UTF8))
                {
                    {
                        for (var i = 0; i <= List1.Items.Count - 1; i++)
                            sw.WriteLine(List1.Items[i].ToString());
                    }
                }
            }
        }

        private cEliza.eSexo SexoNombre()
        {
            // devolverá el sexo según el nombre introducido
            int i;
            cEliza.eSexo tSexo;

            var Mujeres = new[] { " adela", " alicia", " amalia", " amanda", " " + "ana", " anita", " asunción", " aurora", " belinda", " " + "berioska", " carmen", " carmeli", " caty", " celia", " " + "delia", " diana", " dolores", " elena", " elisa", " " + "eva", " felisa", " gabriela", " gemma", " guillermina", " " + "inma", " isa", " josef", " julia", " juana", " juanita", " " + "laura", " luisa", " maite", " manoli", " manuela", " mari", " " + "maría", " marta", " merce", " mónica", " nadia", " " + "paqui", " pepa", " rita", " rosa", " sara", " silvia", " " + "sonia", " susan", " svetlana", " tere", " vane", " " + "vero", " verónica", " vivian" };
            var Hombres = new[] { " adán", " alvaro", " andrés", " bartolo", " " + "borja", " cándido", " carlos", " dámaso", " damián", " " + "daniel", " darío", " félix", " gabriel", " guillermo", " " + "harvey", " jaime", " javier", " joaquín", " joe", " jorge", " " + "jose", " josé", " juan", " luis", " manuel", " miguel", " " + "pepe", " ramón", " santiago", " tomás" };
            // masculino si acaba con 'o', femenino si acaba con 'a'
            var Ambos = new[] { " albert", " antoni", " armand", " bernard", " " + "carmel", " dionisi", " ernest", " fernand", " " + "francisc", " gerard", " ignaci", " manol", " maurici", " " + "pac", " rosend", " venanci" };

            sNombre = " " + sNombre.Trim();
            tSexo = cEliza.eSexo.Ninguno;
            for (i = 0; i <= Mujeres.Length - 1; i++)
            {
                if (sNombre.IndexOf(Mujeres[i]) > -1)
                {
                    tSexo = cEliza.eSexo.Femenino;
                    break;
                }
            }
            if (tSexo == cEliza.eSexo.Ninguno)
            {
                for (i = 0; i <= Hombres.Length - 1; i++)
                {
                    if (sNombre.IndexOf(Hombres[i]) > -1)
                    {
                        tSexo = cEliza.eSexo.Masculino;
                        break;
                    }
                }
            }
            if (tSexo == cEliza.eSexo.Ninguno)
            {
                for (i = 0; i <= Ambos.Length - 1; i++)
                {
                    if (sNombre.IndexOf(Ambos[i]) > -1)
                    {
                        if (sNombre.IndexOf(Ambos[i] + "a") > -1)
                            tSexo = cEliza.eSexo.Femenino;
                        else if (sNombre.IndexOf(Ambos[i] + "o") > -1)
                            tSexo = cEliza.eSexo.Masculino;
                        break;
                    }
                }
            }
            sNombre = sNombre.Trim();
            return tSexo;
        }

        private void LeerNombres()
        {
            string sFic;
            string tmpNombre;

            sFic = AppPath() + "ListaDeNombres.txt";
            // If Len(Dir(sFic)) > 0 Then
            if (System.IO.File.Exists(sFic))
            {
                {
                    var withBlock = List2;
                    withBlock.Items.Clear();
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(sFic, System.Text.Encoding.UTF8, true))
                    {
                        while (!sr.EndOfStream)
                        {
                            tmpNombre = sr.ReadLine().Trim();
                            // Da error si no está inicializado sNombre, 24-ene-2023 12.24
                            if (sNombre.Length == 0)
                                sNombre = tmpNombre;
                            withBlock.Items.Add(tmpNombre);
                            tmpNombre = sr.ReadLine();
                        }
                    }
                }
            }
        }

        private void GuardarNombres()
        {
            string sFic;

            sFic = AppPath() + "ListaDeNombres.txt";
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sFic, false, System.Text.Encoding.UTF8))
            {
                for (var i = 0; i <= List2.Items.Count - 1; i++)
                {
                    // sw.WriteLine($"{BaseUser(i).ID}={BaseUser(i).Contenido}")
                    sw.WriteLine(List2.Items[i].ToString());
                    sw.WriteLine("0"); // el sexo
                }
            }
        }

        private void ImprimirDOS(string sText, bool NuevaLinea = true)
        {
            // Imprimir el texto de entrada en el TextBox de salida
            // Si se NuevaLinea tiene un valor True (valor por defecto)
            // lo siguiente que se imprima se hará en una nueva línea

            string s;

            s = txtSalida.Text + sText;
            if (NuevaLinea)
                s += "\r\n";
            txtSalida.Text = s;
            List1.Items.Add(sText);
            // Posicionar el cursor al final de la caja de texto
            txtSalida.SelectionStart = s.Length;
            txtSalida.SelectionLength = 0;
            txtSalida.ScrollToCaret();
        }


        // Comprobar si se ha pulsado la tecla "arriba",				(24/ene/23 13.16)
        // si es así poner el texto anterior.

        private void txtEntrada_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                e.Handled = true;
                txtEntrada.Text = txtEntrada.Text.Replace("\r\n", "");
                ProcesarEntrada();
            }
            else if (e.KeyData == Keys.Up)
            {
                txtEntrada.Text = sEntradaAnterior;
                txtEntrada.SelectionStart = txtEntrada.Text.Length;
                txtEntrada.SelectionLength = 0;
                e.Handled = true;
            }
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
