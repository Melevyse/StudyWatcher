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
        ListViewItem listViewItem1 = new ListViewItem("данные");
        ListViewItem listViewItem2 = new ListViewItem("pltcm");
        ListViewItem listViewItem3 = new ListViewItem("3123");
        ListViewItem listViewItem4 = new ListViewItem("2312312");
        ListViewItem listViewItem5 = new ListViewItem("");
        listWorkStationForm = new ListView();
        columnHeader1 = new ColumnHeader();
        columnHeader2 = new ColumnHeader();
        columnHeader3 = new ColumnHeader();
        buttonAddProcessBan = new Button();
        buttonDeleteProcessBan = new Button();
        listProcessForm = new ListView();
        label1 = new Label();
        label2 = new Label();
        listProcessBanForm = new ListView();
        label3 = new Label();
        SuspendLayout();
        // 
        // listWorkStationForm
        // 
        listWorkStationForm.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
        listWorkStationForm.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5 });
        listWorkStationForm.Location = new Point(12, 297);
        listWorkStationForm.Name = "listWorkStationForm";
        listWorkStationForm.Size = new Size(748, 303);
        listWorkStationForm.TabIndex = 0;
        listWorkStationForm.UseCompatibleStateImageBehavior = false;
        listWorkStationForm.View = View.Details;
        // 
        // columnHeader1
        // 
        columnHeader1.Text = "ID Устройства";
        columnHeader1.Width = 120;
        // 
        // columnHeader2
        // 
        columnHeader2.Text = "Состояние";
        columnHeader2.Width = 100;
        // 
        // columnHeader3
        // 
        columnHeader3.Text = "Пользователь";
        columnHeader3.Width = 100;
        // 
        // buttonAddProcessBan
        // 
        buttonAddProcessBan.Location = new Point(766, 606);
        buttonAddProcessBan.Name = "buttonAddProcessBan";
        buttonAddProcessBan.Size = new Size(75, 23);
        buttonAddProcessBan.TabIndex = 1;
        buttonAddProcessBan.Text = "Добавить ";
        buttonAddProcessBan.UseVisualStyleBackColor = true;
        // 
        // buttonDeleteProcessBan
        // 
        buttonDeleteProcessBan.Location = new Point(972, 606);
        buttonDeleteProcessBan.Name = "buttonDeleteProcessBan";
        buttonDeleteProcessBan.Size = new Size(75, 23);
        buttonDeleteProcessBan.TabIndex = 2;
        buttonDeleteProcessBan.Text = "button2";
        buttonDeleteProcessBan.UseVisualStyleBackColor = true;
        // 
        // listProcessForm
        // 
        listProcessForm.Location = new Point(766, 40);
        listProcessForm.Name = "listProcessForm";
        listProcessForm.Size = new Size(200, 560);
        listProcessForm.TabIndex = 3;
        listProcessForm.UseCompatibleStateImageBehavior = false;
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
        label2.Location = new Point(766, 22);
        label2.Name = "label2";
        label2.Size = new Size(110, 15);
        label2.TabIndex = 5;
        label2.Text = "Список процессов";
        // 
        // listProcessBanForm
        // 
        listProcessBanForm.Location = new Point(972, 40);
        listProcessBanForm.Name = "listProcessBanForm";
        listProcessBanForm.Size = new Size(200, 560);
        listProcessBanForm.TabIndex = 6;
        listProcessBanForm.UseCompatibleStateImageBehavior = false;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.ForeColor = SystemColors.Control;
        label3.Location = new Point(972, 22);
        label3.Name = "label3";
        label3.Size = new Size(190, 15);
        label3.TabIndex = 8;
        label3.Text = "Список запрещенных процессов";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.GrayText;
        ClientSize = new Size(1184, 661);
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
}