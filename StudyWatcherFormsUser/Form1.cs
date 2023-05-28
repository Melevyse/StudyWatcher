using Microsoft.AspNetCore.SignalR.Client;
using System.Drawing;

namespace StudyWatcherFormsUser;

public partial class Form1 : Form
{
    private HubConnection connection;
    private string connectionIdAdmin;
    private List<string> BlackList;
    private SystemManager _systemManager = new ();
    Banner banner = new();
    string nameLocation = "Г301 #3";
    DateTime lastLaunch = DateTime.UtcNow.Date;

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

        connection.On("CloseStartBanner", 
            (string resultFio, 
                string resultGroup) =>
        {
            Invoke((MethodInvoker)delegate
            {
                BannerTopMost.Stop();
                MessageBox.Show($"Здравствуйте, {resultFio}!\nГруппа:{resultGroup}",
                    "Авторизация успешно завершена");
                this.Hide();
                BlackListWatchTimer.Start();
            });
        });

        connection.On("AdminConnectionComplete", (
            string connectionId) =>
        {
            connectionIdAdmin = connectionId;
        });

        connection.On("CloseBlackListBanner", () =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                banner.Hide();
                BlackListWatchTimer.Start();
            });
        });

        connection.On("RequestPicture", () =>
        {
            PictureSend.Start();
        });

        connection.On("CancelSendPicture", () =>
        {
            PictureSend.Stop();
        });

        connection.On("AddProcessBlackList", (
            string processBan) =>
        {
            if (BlackList != null)
                BlackList.Add(processBan);
        });

        connection.On("RemoveProcessBlackList", (
            string processBan) =>
        {
            if (BlackList != null && BlackList.Count != 0)
            {
                BlackList.Remove(processBan);
            }
        });

        connection.On("ResponseBlackList", (
            List<string> processBanList) =>
        {
            if (BlackList == null)
                BlackList = new List<string>();
            if (BlackList.Count != 0)
                BlackList.Clear();
            BlackList.AddRange(processBanList);
            BlackList = BlackList.Distinct().ToList();
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
            var result = connection
                .InvokeAsync<bool>("GetAuthorizationUserHub",
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
        _systemManager.systemListProcess();
        bool elementFound = false;
        connection.InvokeAsync("AddProcessListHub", 
            nameLocation,
            _systemManager.listProcess,
            lastLaunch,
            connectionIdAdmin,
            connection.ConnectionId);
        IEnumerable<string> inFirstOnly = _systemManager.listProcess.Except(BlackList);
        if (inFirstOnly.Count() < _systemManager.listProcess.Count())
        {
            connection.InvokeAsync("GetBannerHub",
                connection.ConnectionId, connectionIdAdmin);
            banner.labelErrorP1.Text = "Использовано запрещенное программное обеспечение";
            banner.Show();
            BlackListWatchTimer.Stop();
        }
    }

    private void PictureSend_Tick(object sender, EventArgs e)
    {
        using (Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
        {
            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
            }
            ImageConverter converter = new ImageConverter();
            byte[] imageBytes = (byte[])converter.ConvertTo(screenshot, typeof(byte[]));
            
            string base64String = Convert.ToBase64String(imageBytes);

            connection.InvokeAsync("SendPictureHub", base64String, connectionIdAdmin);
        }
    }
}