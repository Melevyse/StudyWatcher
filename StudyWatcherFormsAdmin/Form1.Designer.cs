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
        ListViewItem listViewItem2 = new ListViewItem("данные");
        listView1 = new ListView();
        columnHeader1 = new ColumnHeader();
        columnHeader2 = new ColumnHeader();
        columnHeader3 = new ColumnHeader();
        button1 = new Button();
        button2 = new Button();
        listView2 = new ListView();
        label1 = new Label();
        label2 = new Label();
        SuspendLayout();
        // 
        // listView1
        // 
        listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
        listView1.Items.AddRange(new ListViewItem[] { listViewItem2 });
        listView1.Location = new Point(12, 86);
        listView1.Name = "listView1";
        listView1.Size = new Size(880, 352);
        listView1.TabIndex = 0;
        listView1.UseCompatibleStateImageBehavior = false;
        // 
        // columnHeader1
        // 
        columnHeader1.Text = "ID Устройства";
        // 
        // columnHeader2
        // 
        columnHeader2.Text = "Состояние";
        // 
        // columnHeader3
        // 
        columnHeader3.Text = "Пользователь";
        // 
        // button1
        // 
        button1.Location = new Point(12, 22);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 1;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        button2.Location = new Point(93, 22);
        button2.Name = "button2";
        button2.Size = new Size(75, 23);
        button2.TabIndex = 2;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
        // 
        // listView2
        // 
        listView2.Location = new Point(898, 86);
        listView2.Name = "listView2";
        listView2.Size = new Size(274, 352);
        listView2.TabIndex = 3;
        listView2.UseCompatibleStateImageBehavior = false;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 67);
        label1.Name = "label1";
        label1.Size = new Size(106, 15);
        label1.TabIndex = 4;
        label1.Text = "Список устройств";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(898, 67);
        label2.Name = "label2";
        label2.Size = new Size(110, 15);
        label2.TabIndex = 5;
        label2.Text = "Список процессов";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1184, 450);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(listView2);
        Controls.Add(button2);
        Controls.Add(button1);
        Controls.Add(listView1);
        Name = "MainForm";
        Text = "StudyWatcher";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ListView listView1;
    private Button button1;
    private Button button2;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ListView listView2;
    private Label label1;
    private Label label2;
}