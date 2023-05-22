namespace StudyWatcherFormsAdmin;

partial class MainForm
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
        listWorkStationForm = new ListView();
        columnHeader1 = new ColumnHeader();
        columnHeader2 = new ColumnHeader();
        columnHeader3 = new ColumnHeader();
        columnHeader4 = new ColumnHeader();
        columnHeader5 = new ColumnHeader();
        columnHeader6 = new ColumnHeader();
        columnHeader7 = new ColumnHeader();
        columnHeader8 = new ColumnHeader();
        columnHeader9 = new ColumnHeader();
        columnHeader10 = new ColumnHeader();
        buttonAddProcessBan = new Button();
        buttonDeleteProcessBan = new Button();
        listProcessForm = new ListView();
        label1 = new Label();
        label2 = new Label();
        listProcessBanForm = new ListView();
        label3 = new Label();
        pictureBoxTranslator = new PictureBox();
        ConnectionAdminTimer = new System.Windows.Forms.Timer(components);
        buttonAnova = new Button();
        ((System.ComponentModel.ISupportInitialize)pictureBoxTranslator).BeginInit();
        SuspendLayout();
        // 
        // listWorkStationForm
        // 
        listWorkStationForm.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8, columnHeader9, columnHeader10 });
        listWorkStationForm.Location = new Point(12, 297);
        listWorkStationForm.MultiSelect = false;
        listWorkStationForm.Name = "listWorkStationForm";
        listWorkStationForm.Size = new Size(930, 300);
        listWorkStationForm.TabIndex = 0;
        listWorkStationForm.UseCompatibleStateImageBehavior = false;
        listWorkStationForm.View = View.Details;
        listWorkStationForm.SelectedIndexChanged += listWorkStationForm_SelectedIndexChanged;
        // 
        // columnHeader1
        // 
        columnHeader1.Text = "ID Устройства";
        columnHeader1.Width = 120;
        // 
        // columnHeader2
        // 
        columnHeader2.Text = "Пользователь";
        columnHeader2.Width = 100;
        // 
        // columnHeader3
        // 
        columnHeader3.Text = "Группа";
        columnHeader3.Width = 100;
        // 
        // columnHeader4
        // 
        columnHeader4.Text = "MAC";
        columnHeader4.Width = 100;
        // 
        // columnHeader5
        // 
        columnHeader5.Text = "CPU";
        columnHeader5.Width = 100;
        // 
        // columnHeader6
        // 
        columnHeader6.Text = "RAM";
        columnHeader6.Width = 100;
        // 
        // columnHeader7
        // 
        columnHeader7.Text = "HDD";
        columnHeader7.Width = 100;
        // 
        // columnHeader8
        // 
        columnHeader8.Text = "Videocard";
        columnHeader8.Width = 100;
        // 
        // columnHeader9
        // 
        columnHeader9.Text = "Статус";
        columnHeader9.Width = 100;
        // 
        // columnHeader10
        // 
        columnHeader10.Text = "ConnectionId";
        columnHeader10.Width = 0;
        // 
        // buttonAddProcessBan
        // 
        buttonAddProcessBan.Location = new Point(1172, 603);
        buttonAddProcessBan.Name = "buttonAddProcessBan";
        buttonAddProcessBan.Size = new Size(75, 23);
        buttonAddProcessBan.TabIndex = 1;
        buttonAddProcessBan.Text = "Добавить ";
        buttonAddProcessBan.UseVisualStyleBackColor = true;
        buttonAddProcessBan.Click += buttonAddProcessBan_Click;
        // 
        // buttonDeleteProcessBan
        // 
        buttonDeleteProcessBan.Location = new Point(1297, 603);
        buttonDeleteProcessBan.Name = "buttonDeleteProcessBan";
        buttonDeleteProcessBan.Size = new Size(75, 23);
        buttonDeleteProcessBan.TabIndex = 2;
        buttonDeleteProcessBan.Text = "Удалить";
        buttonDeleteProcessBan.UseVisualStyleBackColor = true;
        // 
        // listProcessForm
        // 
        listProcessForm.Location = new Point(958, 37);
        listProcessForm.MultiSelect = false;
        listProcessForm.Name = "listProcessForm";
        listProcessForm.Size = new Size(200, 560);
        listProcessForm.TabIndex = 3;
        listProcessForm.UseCompatibleStateImageBehavior = false;
        listProcessForm.View = View.List;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = SystemColors.Control;
        label1.Location = new Point(12, 279);
        label1.Name = "label1";
        label1.Size = new Size(106, 15);
        label1.TabIndex = 4;
        label1.Text = "Список устройств";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.ForeColor = SystemColors.Control;
        label2.Location = new Point(958, 19);
        label2.Name = "label2";
        label2.Size = new Size(110, 15);
        label2.TabIndex = 5;
        label2.Text = "Список процессов";
        // 
        // listProcessBanForm
        // 
        listProcessBanForm.Location = new Point(1172, 37);
        listProcessBanForm.MultiSelect = false;
        listProcessBanForm.Name = "listProcessBanForm";
        listProcessBanForm.Size = new Size(200, 560);
        listProcessBanForm.TabIndex = 6;
        listProcessBanForm.UseCompatibleStateImageBehavior = false;
        listProcessBanForm.View = View.List;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.ForeColor = SystemColors.Control;
        label3.Location = new Point(1172, 19);
        label3.Name = "label3";
        label3.Size = new Size(190, 15);
        label3.TabIndex = 8;
        label3.Text = "Список запрещенных процессов";
        // 
        // pictureBoxTranslator
        // 
        pictureBoxTranslator.BackColor = SystemColors.ControlDark;
        pictureBoxTranslator.Location = new Point(12, 22);
        pictureBoxTranslator.Name = "pictureBoxTranslator";
        pictureBoxTranslator.Size = new Size(480, 250);
        pictureBoxTranslator.TabIndex = 9;
        pictureBoxTranslator.TabStop = false;
        // 
        // ConnectionAdminTimer
        // 
        ConnectionAdminTimer.Interval = 5000;
        ConnectionAdminTimer.Tick += ConnectionAdminTimer_Tick;
        // 
        // buttonAnova
        // 
        buttonAnova.Location = new Point(958, 603);
        buttonAnova.Name = "buttonAnova";
        buttonAnova.Size = new Size(75, 23);
        buttonAnova.TabIndex = 10;
        buttonAnova.Text = "ANOVA";
        buttonAnova.UseVisualStyleBackColor = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.GrayText;
        ClientSize = new Size(1384, 661);
        Controls.Add(buttonAnova);
        Controls.Add(pictureBoxTranslator);
        Controls.Add(label3);
        Controls.Add(listProcessBanForm);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(listProcessForm);
        Controls.Add(buttonDeleteProcessBan);
        Controls.Add(buttonAddProcessBan);
        Controls.Add(listWorkStationForm);
        Name = "MainForm";
        Text = "StudyWatcher";
        Load += MainForm_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBoxTranslator).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ListView listWorkStationForm;
    private Button buttonAddProcessBan;
    private Button buttonDeleteProcessBan;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ListView listProcessForm;
    private Label label1;
    private Label label2;
    private ListView listProcessBanForm;
    private Label label3;
    private PictureBox pictureBoxTranslator;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private ColumnHeader columnHeader9;
    private ColumnHeader columnHeader10;
    private System.Windows.Forms.Timer ConnectionAdminTimer;
    private Button buttonAnova;
}