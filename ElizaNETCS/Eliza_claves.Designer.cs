using System.Windows.Forms;

namespace ElizaNETCS
{
    partial class Eliza_claves
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Eliza_claves));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Combo1 = new System.Windows.Forms.ComboBox();
            this._List1_1 = new System.Windows.Forms.ListBox();
            this._List1_0 = new System.Windows.Forms.ListBox();
            this._Label1_0 = new System.Windows.Forms.Label();
            this._Label1_1 = new System.Windows.Forms.Label();
            this.List2 = new System.Windows.Forms.ListBox();
            this._Label1_2 = new System.Windows.Forms.Label();
            this._Label1_3 = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Combo1
            // 
            this.Combo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo1.Location = new System.Drawing.Point(12, 12);
            this.Combo1.Name = "Combo1";
            this.Combo1.Size = new System.Drawing.Size(358, 33);
            this.Combo1.TabIndex = 1;
            this.Combo1.SelectedIndexChanged += new System.EventHandler(this.Combo1_SelectedIndexChanged);
            // 
            // _List1_1
            // 
            this._List1_1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._List1_1.HorizontalScrollbar = true;
            this._List1_1.ItemHeight = 25;
            this._List1_1.Location = new System.Drawing.Point(12, 260);
            this._List1_1.Name = "_List1_1";
            this._List1_1.Size = new System.Drawing.Size(358, 179);
            this._List1_1.TabIndex = 7;
            this._List1_1.SelectedIndexChanged += new System.EventHandler(this.List1_SelectedIndexChanged);
            // 
            // _List1_0
            // 
            this._List1_0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._List1_0.HorizontalScrollbar = true;
            this._List1_0.ItemHeight = 25;
            this._List1_0.Location = new System.Drawing.Point(12, 51);
            this._List1_0.Name = "_List1_0";
            this._List1_0.Size = new System.Drawing.Size(358, 154);
            this._List1_0.Sorted = true;
            this._List1_0.TabIndex = 5;
            this._List1_0.SelectedIndexChanged += new System.EventHandler(this.List1_SelectedIndexChanged);
            // 
            // _Label1_0
            // 
            this._Label1_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._Label1_0.Location = new System.Drawing.Point(12, 229);
            this._Label1_0.Margin = new System.Windows.Forms.Padding(3);
            this._Label1_0.Name = "_Label1_0";
            this._Label1_0.Size = new System.Drawing.Size(358, 25);
            this._Label1_0.TabIndex = 6;
            this._Label1_0.Text = "Label1";
            this._Label1_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _Label1_1
            // 
            this._Label1_1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._Label1_1.Location = new System.Drawing.Point(12, 461);
            this._Label1_1.Margin = new System.Windows.Forms.Padding(3);
            this._Label1_1.Name = "_Label1_1";
            this._Label1_1.Size = new System.Drawing.Size(358, 25);
            this._Label1_1.TabIndex = 8;
            this._Label1_1.Text = "Label1";
            this._Label1_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // List2
            // 
            this.List2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.List2.HorizontalScrollbar = true;
            this.List2.ItemHeight = 25;
            this.List2.Location = new System.Drawing.Point(385, 51);
            this.List2.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.List2.Name = "List2";
            this.List2.Size = new System.Drawing.Size(498, 379);
            this.List2.TabIndex = 10;
            // 
            // _Label1_2
            // 
            this._Label1_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._Label1_2.Location = new System.Drawing.Point(385, 13);
            this._Label1_2.Margin = new System.Windows.Forms.Padding(3);
            this._Label1_2.Name = "_Label1_2";
            this._Label1_2.Size = new System.Drawing.Size(498, 25);
            this._Label1_2.TabIndex = 9;
            this._Label1_2.Text = "Label1";
            this._Label1_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _Label1_3
            // 
            this._Label1_3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._Label1_3.Location = new System.Drawing.Point(385, 461);
            this._Label1_3.Margin = new System.Windows.Forms.Padding(3);
            this._Label1_3.Name = "_Label1_3";
            this._Label1_3.Size = new System.Drawing.Size(498, 25);
            this._Label1_3.TabIndex = 11;
            this._Label1_3.Text = "Label1";
            this._Label1_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Eliza_claves
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 498);
            this.Controls.Add(this.List2);
            this.Controls.Add(this._Label1_2);
            this.Controls.Add(this._Label1_3);
            this.Controls.Add(this._List1_1);
            this.Controls.Add(this._List1_0);
            this.Controls.Add(this._Label1_0);
            this.Controls.Add(this._Label1_1);
            this.Controls.Add(this.Combo1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Eliza_claves";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mantenimiento de claves y respuestas de Eliza";
            this.Load += new System.EventHandler(this.Eliza_claves_Load);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.Timer Timer1 = new();
        private ToolTip toolTip1;
        private ComboBox Combo1;
        private ListBox _List1_1;
        private ListBox _List1_0;
        private Label _Label1_0;
        private Label _Label1_1;
        private ListBox List2;
        private Label _Label1_2;
        private Label _Label1_3;
        private System.Windows.Forms.Timer Timer1;
    }
}