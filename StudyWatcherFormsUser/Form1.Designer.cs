namespace StudyWatcherFormsUser;

partial class Form1
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
        components = new System.ComponentModel.Container();
        HubConnectionTimer = new System.Windows.Forms.Timer(components);
        BannerTopMost = new System.Windows.Forms.Timer(components);
        loginTextBox = new TextBox();
        passwordTextBox = new TextBox();
        AcceptButton = new Button();
        SuspendLayout();
        // 
        // HubConnectionTimer
        // 
        HubConnectionTimer.Interval = 3000;
        HubConnectionTimer.Tick += HubConnectionTimer_Tick;
        // 
        // BannerTopMost
        // 
        BannerTopMost.Tick += BannerTopMost_Tick;
        // 
        // loginTextBox
        // 
        loginTextBox.Location = new Point(335, 159);
        loginTextBox.Name = "loginTextBox";
        loginTextBox.Size = new Size(212, 23);
        loginTextBox.TabIndex = 0;
        // 
        // passwordTextBox
        // 
        passwordTextBox.Location = new Point(335, 208);
        passwordTextBox.Name = "passwordTextBox";
        passwordTextBox.PasswordChar = '•';
        passwordTextBox.Size = new Size(212, 23);
        passwordTextBox.TabIndex = 1;
        // 
        // AcceptButton
        // 
        AcceptButton.Location = new Point(346, 237);
        AcceptButton.Name = "AcceptButton";
        AcceptButton.Size = new Size(75, 23);
        AcceptButton.TabIndex = 2;
        AcceptButton.Text = "Ввод";
        AcceptButton.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 361);
        Controls.Add(AcceptButton);
        Controls.Add(passwordTextBox);
        Controls.Add(loginTextBox);
        Name = "Form1";
        Text = "Form1";
        Load += Form1_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Timer HubConnectionTimer;
    private System.Windows.Forms.Timer BannerTopMost;
    private TextBox loginTextBox;
    private TextBox passwordTextBox;
    private Button AcceptButton;
}