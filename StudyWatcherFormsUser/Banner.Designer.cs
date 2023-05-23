namespace StudyWatcherFormsUser
{
    partial class Banner
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
            components = new System.ComponentModel.Container();
            TopMostTimer = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            labelErrorP1 = new Label();
            labelErrorP2 = new Label();
            SuspendLayout();
            // 
            // TopMostTimer
            // 
            TopMostTimer.Tick += TopMostTimer_Tick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // labelErrorP1
            // 
            labelErrorP1.AutoSize = true;
            labelErrorP1.BackColor = SystemColors.ControlText;
            labelErrorP1.Font = new Font("Times New Roman", 20F, FontStyle.Regular, GraphicsUnit.Point);
            labelErrorP1.ForeColor = SystemColors.Control;
            labelErrorP1.Location = new Point(141, 226);
            labelErrorP1.Name = "labelErrorP1";
            labelErrorP1.Size = new Size(650, 31);
            labelErrorP1.TabIndex = 1;
            labelErrorP1.Text = "Использовано запрещенное программное обеспечение:";
            // 
            // labelErrorP2
            // 
            labelErrorP2.AutoSize = true;
            labelErrorP2.BackColor = SystemColors.ControlText;
            labelErrorP2.Font = new Font("Times New Roman", 20F, FontStyle.Regular, GraphicsUnit.Point);
            labelErrorP2.ForeColor = SystemColors.Control;
            labelErrorP2.Location = new Point(95, 275);
            labelErrorP2.Name = "labelErrorP2";
            labelErrorP2.Size = new Size(735, 31);
            labelErrorP2.TabIndex = 2;
            labelErrorP2.Text = "Дождитесь пока администратор одобрит продолжение работы";
            // 
            // Banner
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(951, 548);
            Controls.Add(labelErrorP2);
            Controls.Add(labelErrorP1);
            Controls.Add(label1);
            Name = "Banner";
            Text = "Banner";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer TopMostTimer;
        private Label label1;
        private Label labelErrorP2;
        public Label labelErrorP1;
    }
}