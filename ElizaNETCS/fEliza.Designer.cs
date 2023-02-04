using System.Windows.Forms;

namespace ElizaNETCS
{
    partial class fEliza
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fEliza));
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.MainMenu1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.LabelInfo = new System.Windows.Forms.Label();
            this.txtSalida = new System.Windows.Forms.TextBox();
            this.txtEntrada = new System.Windows.Forms.TextBox();
            this.cmdNuevo = new System.Windows.Forms.Button();
            this.List1 = new System.Windows.Forms.ListBox();
            this.MainMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // MainMenu1
            // 
            this.MainMenu1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.MainMenu1.Location = new System.Drawing.Point(0, 0);
            this.MainMenu1.Name = "MainMenu1";
            this.MainMenu1.Size = new System.Drawing.Size(1002, 33);
            this.MainMenu1.TabIndex = 0;
            // 
            // mnuFile
            // 
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(139, 29);
            this.mnuFile.Text = "&Configuración";
            // 
            // LabelInfo
            // 
            this.LabelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabelInfo.Location = new System.Drawing.Point(15, 590);
            this.LabelInfo.Margin = new System.Windows.Forms.Padding(3);
            this.LabelInfo.Name = "LabelInfo";
            this.LabelInfo.Size = new System.Drawing.Size(757, 30);
            this.LabelInfo.TabIndex = 10;
            this.LabelInfo.Text = "Eliza para Visual Basic, ©Guillermo Som (Guille), 1998-2002, 2023";
            this.LabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSalida
            // 
            this.txtSalida.AcceptsReturn = true;
            this.txtSalida.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSalida.BackColor = System.Drawing.Color.Black;
            this.txtSalida.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSalida.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSalida.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSalida.ForeColor = System.Drawing.Color.Lime;
            this.txtSalida.Location = new System.Drawing.Point(7, 36);
            this.txtSalida.MaxLength = 0;
            this.txtSalida.Multiline = true;
            this.txtSalida.Name = "txtSalida";
            this.txtSalida.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSalida.Size = new System.Drawing.Size(986, 480);
            this.txtSalida.TabIndex = 9;
            this.txtSalida.Text = "txtSalida\r\n";
            // 
            // txtEntrada
            // 
            this.txtEntrada.AcceptsReturn = true;
            this.txtEntrada.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEntrada.BackColor = System.Drawing.Color.Black;
            this.txtEntrada.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEntrada.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEntrada.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEntrada.ForeColor = System.Drawing.Color.Lime;
            this.txtEntrada.Location = new System.Drawing.Point(7, 520);
            this.txtEntrada.MaxLength = 0;
            this.txtEntrada.Multiline = true;
            this.txtEntrada.Name = "txtEntrada";
            this.txtEntrada.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEntrada.Size = new System.Drawing.Size(986, 60);
            this.txtEntrada.TabIndex = 8;
            this.txtEntrada.Text = "txtEntrada\r\nlínea 2";
            this.txtEntrada.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtEntrada_KeyUp);
            // 
            // cmdNuevo
            // 
            this.cmdNuevo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdNuevo.Location = new System.Drawing.Point(778, 586);
            this.cmdNuevo.Name = "cmdNuevo";
            this.cmdNuevo.Size = new System.Drawing.Size(212, 34);
            this.cmdNuevo.TabIndex = 7;
            this.cmdNuevo.Text = "Iniciar nueva sesión";
            this.cmdNuevo.Click += new System.EventHandler(this.cmdNuevo_Click);
            // 
            // List1
            // 
            this.List1.BackColor = System.Drawing.SystemColors.Window;
            this.List1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.List1.ItemHeight = 25;
            this.List1.Items.AddRange(new object[] {
            "List1-No visible"});
            this.List1.Location = new System.Drawing.Point(864, 58);
            this.List1.Name = "List1";
            this.List1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.List1.Size = new System.Drawing.Size(113, 29);
            this.List1.TabIndex = 12;
            this.List1.Visible = false;
            // 
            // fEliza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 632);
            this.Controls.Add(this.List1);
            this.Controls.Add(this.LabelInfo);
            this.Controls.Add(this.txtSalida);
            this.Controls.Add(this.txtEntrada);
            this.Controls.Add(this.cmdNuevo);
            this.Controls.Add(this.MainMenu1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu1;
            this.Name = "fEliza";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eliza para C#";
            this.Load += new System.EventHandler(this.fEliza_Load);
            this.MainMenu1.ResumeLayout(false);
            this.MainMenu1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolTip ToolTip1;
        private System.Windows.Forms.Timer Timer1;
        private MenuStrip MainMenu1;
        private ToolStripMenuItem mnuFile;
        private Label LabelInfo;
        private TextBox txtSalida;
        private TextBox txtEntrada;
        private Button cmdNuevo;
        public ListBox List1;
        public System.Windows.Forms.ToolStripMenuItem mnuFileReleer = new();
        public System.Windows.Forms.ToolStripSeparator mnuFileSep1 = new();
        public System.Windows.Forms.ToolStripMenuItem mnuEstadísticas = new();
        public System.Windows.Forms.ToolStripSeparator mnuFileSep2 = new();
        public System.Windows.Forms.ToolStripMenuItem mnuEliza_claves = new();
        public System.Windows.Forms.ToolStripSeparator mnuFileSep3 = new();
        public System.Windows.Forms.ToolStripMenuItem mnuAcercaDe = new();
        public System.Windows.Forms.ToolStripSeparator mnuFileSep5 = new();
        public System.Windows.Forms.ToolStripMenuItem mnuSalir = new();
    }
}