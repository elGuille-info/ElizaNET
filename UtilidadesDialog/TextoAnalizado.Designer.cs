namespace UtilidadesDialog
{
    partial class TextoAnalizado
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
            this.TxtResultado = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button0 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.ComboTextos = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtResultado
            // 
            this.TxtResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtResultado.BackColor = System.Drawing.Color.Black;
            this.TxtResultado.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtResultado.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TxtResultado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TxtResultado.ForeColor = System.Drawing.Color.Lime;
            this.TxtResultado.Location = new System.Drawing.Point(12, 51);
            this.TxtResultado.MaxLength = 0;
            this.TxtResultado.Multiline = true;
            this.TxtResultado.Name = "TxtResultado";
            this.TxtResultado.ReadOnly = true;
            this.TxtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtResultado.Size = new System.Drawing.Size(1183, 593);
            this.TxtResultado.TabIndex = 1;
            this.TxtResultado.Text = "Resultado del análisis (solo lectura)";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.button0);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.button6);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 653);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1177, 47);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // button0
            // 
            this.button0.AutoSize = true;
            this.button0.Location = new System.Drawing.Point(7, 7);
            this.button0.Name = "button0";
            this.button0.Size = new System.Drawing.Size(199, 35);
            this.button0.TabIndex = 0;
            this.button0.Text = "Resumen seleccionada";
            this.button0.UseVisualStyleBackColor = true;
            this.button0.Click += new System.EventHandler(this.button0_Click);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(212, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Todo con tokens";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Location = new System.Drawing.Point(372, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 35);
            this.button2.TabIndex = 2;
            this.button2.Text = "Todo sin tokens";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.Location = new System.Drawing.Point(525, 7);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 35);
            this.button3.TabIndex = 3;
            this.button3.Text = "Solo tokens";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.Location = new System.Drawing.Point(647, 7);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(119, 35);
            this.button4.TabIndex = 4;
            this.button4.Text = "Solo entities";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.Location = new System.Drawing.Point(772, 7);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(199, 35);
            this.button5.TabIndex = 5;
            this.button5.Text = "Resumen última";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.AutoSize = true;
            this.button6.Location = new System.Drawing.Point(977, 7);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(144, 35);
            this.button6.TabIndex = 6;
            this.button6.Text = "Resumen todas";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // ComboTextos
            // 
            this.ComboTextos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboTextos.DisplayMember = "Texto";
            this.ComboTextos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboTextos.FormattingEnabled = true;
            this.ComboTextos.Location = new System.Drawing.Point(12, 12);
            this.ComboTextos.Name = "ComboTextos";
            this.ComboTextos.Size = new System.Drawing.Size(1183, 33);
            this.ComboTextos.TabIndex = 0;
            this.ComboTextos.SelectedIndexChanged += new System.EventHandler(this.ComboTextos_SelectedIndexChanged);
            // 
            // TextoAnalizado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 715);
            this.Controls.Add(this.ComboTextos);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.TxtResultado);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "TextoAnalizado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Texto Analizado";
            this.Load += new System.EventHandler(this.TextoAnalizado_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtResultado;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ComboBox ComboTextos;
        private System.Windows.Forms.Button button0;
    }
}