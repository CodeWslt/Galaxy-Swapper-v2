
namespace Galaxy_Swapper_v2.Workspace.Forms
{
    partial class ID_Converter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ID_Converter));
            this.Dragbar = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseBox = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CosmeticNameLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RevertButton = new System.Windows.Forms.Label();
            this.ConvertButton = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Dragbar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Dragbar
            // 
            this.Dragbar.Controls.Add(this.panel1);
            this.Dragbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Dragbar.Location = new System.Drawing.Point(0, 0);
            this.Dragbar.Name = "Dragbar";
            this.Dragbar.Size = new System.Drawing.Size(520, 26);
            this.Dragbar.TabIndex = 37;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CloseBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(493, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(27, 26);
            this.panel1.TabIndex = 0;
            // 
            // CloseBox
            // 
            this.CloseBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseBox.Image = ((System.Drawing.Image)(resources.GetObject("CloseBox.Image")));
            this.CloseBox.Location = new System.Drawing.Point(5, 4);
            this.CloseBox.Name = "CloseBox";
            this.CloseBox.Size = new System.Drawing.Size(17, 17);
            this.CloseBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseBox.TabIndex = 18;
            this.CloseBox.TabStop = false;
            this.CloseBox.Click += new System.EventHandler(this.CloseBox_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(12, 54);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(483, 25);
            this.textBox1.TabIndex = 38;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox2.Location = new System.Drawing.Point(12, 102);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(483, 25);
            this.textBox2.TabIndex = 39;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(497, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 25);
            this.label1.TabIndex = 40;
            this.label1.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(497, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 25);
            this.label2.TabIndex = 41;
            this.label2.Text = "0";
            // 
            // CosmeticNameLabel
            // 
            this.CosmeticNameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.CosmeticNameLabel.ForeColor = System.Drawing.Color.White;
            this.CosmeticNameLabel.Location = new System.Drawing.Point(12, 33);
            this.CosmeticNameLabel.Name = "CosmeticNameLabel";
            this.CosmeticNameLabel.Size = new System.Drawing.Size(522, 18);
            this.CosmeticNameLabel.TabIndex = 42;
            this.CosmeticNameLabel.Text = "ID That You Own:";
            this.CosmeticNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(522, 18);
            this.label3.TabIndex = 43;
            this.label3.Text = "ID That You Want:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // RevertButton
            // 
            this.RevertButton.BackColor = System.Drawing.Color.White;
            this.RevertButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RevertButton.ForeColor = System.Drawing.Color.White;
            this.RevertButton.Location = new System.Drawing.Point(261, 144);
            this.RevertButton.Name = "RevertButton";
            this.RevertButton.Size = new System.Drawing.Size(234, 39);
            this.RevertButton.TabIndex = 45;
            this.RevertButton.Text = "Revert";
            this.RevertButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // ConvertButton
            // 
            this.ConvertButton.BackColor = System.Drawing.Color.White;
            this.ConvertButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConvertButton.ForeColor = System.Drawing.Color.White;
            this.ConvertButton.Location = new System.Drawing.Point(12, 144);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(234, 39);
            this.ConvertButton.TabIndex = 44;
            this.ConvertButton.Text = "Convert";
            this.ConvertButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(234, 39);
            this.label4.TabIndex = 47;
            this.label4.Text = "Tutorial";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(261, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(234, 39);
            this.label5.TabIndex = 46;
            this.label5.Text = "Cosmetic Id List";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // ID_Converter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 244);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RevertButton);
            this.Controls.Add(this.ConvertButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CosmeticNameLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Dragbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ID_Converter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ID_Converter";
            this.Load += new System.EventHandler(this.ID_Converter_Load);
            this.Dragbar.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CloseBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Dragbar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox CloseBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CosmeticNameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label RevertButton;
        private System.Windows.Forms.Label ConvertButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}