using Microsoft.AspNetCore.SignalR.Client;
using System.Management;
using System.Diagnostics;
using System;

namespace StudyWatcherFormsUser;

public partial class Form1 : Form
{
    private HubConnection connection;
    string connectionIdAdmin;
    private List<string> BlackList;
    private SystemManager _systemManager;

    public async Task ConnectionHub()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5123/hub")
            .WithAutomaticReconnect()
            .Build();
        connection.StartAsync().Wait();
    }

    public Form1()
    {
        ConnectionHub();
        _systemManager = new SystemManager();
        InitializeComponent();
        BannerTopMost.Start();
        this.WindowState = FormWindowState.Maximized;
        this.FormBorderStyle = FormBorderStyle.None;
        loginTextBox.Size = new Size((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - loginTextBox.Size.Width / 2,
            System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - 50);
        loginTextBox.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - loginTextBox.Size.Width / 2,
            (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2) - 100);
        passwordTextBox.Size = new Size((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - passwordTextBox.Size.Width / 2,
            System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - 50);
        passwordTextBox.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - passwordTextBox.Size.Width / 2,
            (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2) - 50);
        AcceptButton.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 50,
            (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2));

        connection.On("CloseStartBanner", (string resultFio, string resultGroup) =>
        {
            MessageBox.Show($"Здравствуйте, {resultFio}!/nГруппа:{resultGroup}", "/nАвторизация успешно завершена");
            this.Hide();
        });

        connection.On("AdminConnectionComplete", (
            string connectionId) =>
        {
            connectionIdAdmin = connectionId;
        });

        connection.On("OpenBlackListBanner", () =>
        {
            //На данный момент форма не создана
        });

        connection.On("CloseBlackListBanner", () =>
        {
            //На данный момент форма не создана
        });
        
        connection.On("CloseBlackListBanner", () =>
        {
            //На данный момент форма не создана
        });
        
        connection.On("RemoveProcessBlackList", (
            string processBan) =>
        {
            
        });
        
        connection.On("ResponseBlackList", (
            List<string> processBanList) =>
        {
            if (BlackList == null)
            {
                BlackList = new List<string>();
                if (BlackList.Count != 0) BlackList.Clear();
            }
            BlackList.AddRange(processBanList);
        });

        connection.StartAsync();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        HubConnectionTimer.Start();
    }

    private void HubConnectionTimer_Tick(object sender, EventArgs e)
    {

        if (connectionIdAdmin != null)
        {
            string nameLocation = "Г301 #1";
            DateTime lastLaunch = DateTime.UtcNow;
            var connectionId = connection.ConnectionId;
            connection.InvokeAsync("AddWorkStationHub",
                _systemManager.nameMotherboard,
                _systemManager.nameCPU,
                _systemManager.nameRAM,
                _systemManager.nameHDD,
                _systemManager.nameVideocard,
                nameLocation,
                _systemManager.listProcess,
                lastLaunch,
                connectionIdAdmin,
                connectionId);
            HubConnectionTimer.Stop();
        }
    }

    private void BannerTopMost_Tick(object sender, EventArgs e)
    {
        this.TopMost = true;
    }

    private void AcceptButton_Click(object sender, EventArgs e)
    {
        if (loginTextBox.Text != "" && passwordTextBox.Text != "")
        {
            var result = connection.InvokeAsync<bool>("GetAuthorizationUserHub",
                loginTextBox.Text, passwordTextBox.Text, connectionIdAdmin);
            if (result.GetAwaiter().GetResult() != true)
                MessageBox.Show("Неправильный логин или пароль");
        }
        else
        {
            MessageBox.Show("Все поля должны быть заполнены");
        }
    }

    private void BlackListWatchTimer_Tick(object sender, EventArgs e)
    {

    }
}