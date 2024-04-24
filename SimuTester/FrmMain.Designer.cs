namespace SimuTester
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Ans_Box = new TextBox();
            Matrix_Button = new Button();
            FreqResp_Button = new Button();
            MathCheck_Button = new Button();
            Polynomial_Button = new Button();
            SuspendLayout();
            // 
            // Ans_Box
            // 
            Ans_Box.Location = new Point(15, 100);
            Ans_Box.Multiline = true;
            Ans_Box.Name = "Ans_Box";
            Ans_Box.ScrollBars = ScrollBars.Both;
            Ans_Box.Size = new Size(750, 600);
            Ans_Box.TabIndex = 0;
            // 
            // Matrix_Button
            // 
            Matrix_Button.Location = new Point(15, 15);
            Matrix_Button.Name = "Matrix_Button";
            Matrix_Button.Size = new Size(120, 40);
            Matrix_Button.TabIndex = 1;
            Matrix_Button.Text = "Matrix";
            Matrix_Button.UseVisualStyleBackColor = true;
            Matrix_Button.Click += Matrix_Button_Click;
            // 
            // FreqResp_Button
            // 
            FreqResp_Button.Location = new Point(150, 15);
            FreqResp_Button.Name = "FreqResp_Button";
            FreqResp_Button.Size = new Size(120, 40);
            FreqResp_Button.TabIndex = 1;
            FreqResp_Button.Text = "FreqResp";
            FreqResp_Button.UseVisualStyleBackColor = true;
            FreqResp_Button.Click += FreqResp_Button_Click;
            // 
            // MathCheck_Button
            // 
            MathCheck_Button.Location = new Point(285, 15);
            MathCheck_Button.Name = "MathCheck_Button";
            MathCheck_Button.Size = new Size(120, 40);
            MathCheck_Button.TabIndex = 1;
            MathCheck_Button.Text = "MathCheck";
            MathCheck_Button.UseVisualStyleBackColor = true;
            MathCheck_Button.Click += MathCheck_Button_Click;
            // 
            // Polynomial_Button
            // 
            Polynomial_Button.Location = new Point(420, 15);
            Polynomial_Button.Name = "Polynomial_Button";
            Polynomial_Button.Size = new Size(120, 40);
            Polynomial_Button.TabIndex = 2;
            Polynomial_Button.Text = "Polynomial";
            Polynomial_Button.UseVisualStyleBackColor = true;
            Polynomial_Button.Click += Polynomial_Button_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(778, 744);
            Controls.Add(Polynomial_Button);
            Controls.Add(MathCheck_Button);
            Controls.Add(FreqResp_Button);
            Controls.Add(Matrix_Button);
            Controls.Add(Ans_Box);
            Font = new Font("Meiryo UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "FrmMain";
            Text = "Form1";
            Load += FrmMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Ans_Box;
        private Button Matrix_Button;
        private Button FreqResp_Button;
        private Button MathCheck_Button;
        private Button Polynomial_Button;
    }
}
