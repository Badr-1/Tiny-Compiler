namespace Tiny_Compiler
{
    partial class Form1
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
            this.Compile = new System.Windows.Forms.Button();
            this.regx = new System.Windows.Forms.TextBox();
            this.code = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Compile
            // 
            this.Compile.Font = new System.Drawing.Font("Tahoma", 18F);
            this.Compile.Location = new System.Drawing.Point(12, 283);
            this.Compile.Name = "Compile";
            this.Compile.Size = new System.Drawing.Size(776, 59);
            this.Compile.TabIndex = 1;
            this.Compile.Text = "Compile";
            this.Compile.UseVisualStyleBackColor = true;
            this.Compile.Click += new System.EventHandler(this.Compile_Click);
            // 
            // regx
            // 
            this.regx.Font = new System.Drawing.Font("Tahoma", 18F);
            this.regx.Location = new System.Drawing.Point(12, 348);
            this.regx.Multiline = true;
            this.regx.Name = "regx";
            this.regx.ReadOnly = true;
            this.regx.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.regx.Size = new System.Drawing.Size(776, 84);
            this.regx.TabIndex = 2;
            // 
            // code
            // 
            this.code.Font = new System.Drawing.Font("Tahoma", 18F);
            this.code.Location = new System.Drawing.Point(12, 23);
            this.code.Name = "code";
            this.code.Size = new System.Drawing.Size(776, 240);
            this.code.TabIndex = 3;
            this.code.Text = "";
            this.code.TextChanged += new System.EventHandler(this.code_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 444);
            this.Controls.Add(this.code);
            this.Controls.Add(this.regx);
            this.Controls.Add(this.Compile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Compile;
        private System.Windows.Forms.TextBox regx;
        private System.Windows.Forms.RichTextBox code;
    }
}

