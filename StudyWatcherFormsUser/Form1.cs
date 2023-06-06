using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace StudyWatcherFormsUser;

public partial class Form1 : Form
{
    private ConfigXmlRead configXmlRead = new ();
    private HubConnection connection;
    private string connectionIdAdmin;
    private List<string> BlackList;
    private SystemManager _systemManager = new();
    private Banner banner = new();
    string nameLocation;
    private DateTime lastLaunch = DateTime.UtcNow.Date;
    private string imageString;

    public async Task ConnectionHub(string str)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(str)
            .WithAutomaticReconnect()
            .Build();
        connection.ServerTimeout = TimeSpan.FromMinutes(5);
        connection.StartAsync().Wait();
    }

    public Form1()
    {
        nameLocation = configXmlRead.Location;
        var connectionHubIp = $"http://{configXmlRead.Ip}:5123/hub";
        ConnectionHub(connectionHubIp);
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
        PictureSend.Start();
    }

    private async void HubConnectionTimer_Tick(object sender, EventArgs e)
    {
        if (connectionIdAdmin != null)
        {
            var connectionId = connection.ConnectionId;
            await connection.InvokeAsync("AddWorkStationHub",
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

    private async void BlackListWatchTimer_Tick(object sender, EventArgs e)
    {
        _systemManager.systemListProcess();
        await connection.InvokeAsync("AddProcessListHub",
            nameLocation,
            _systemManager.listProcess,
            lastLaunch,
            connectionIdAdmin,
            connection.ConnectionId);
        var inFirstOnly = _systemManager.listProcess.Except(BlackList);
        if (inFirstOnly.Count() < _systemManager.listProcess.Count())
        {
            await connection.InvokeAsync("GetBannerHub",
                connection.ConnectionId, connectionIdAdmin);
            banner.labelErrorP1.Text = "Использовано запрещенное программное обеспечение";
            banner.Show();
            BlackListWatchTimer.Stop();
        }
    }

    private void PictureSend_Tick(object sender, EventArgs e)
    {
        if (connectionIdAdmin != null)
        {
            var screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height,
                PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                    Screen.PrimaryScreen.Bounds.Y, 0,
                    0, Screen.PrimaryScreen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);
            }
            var targetWidth = 480;
            var targetHeight = 250;
            byte[] imageBytes;
            using (var resizedImage = new Bitmap(targetWidth, targetHeight))
            {
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(screenshot, 0, 0, targetWidth, targetHeight);
                    using (var memoryStream = new MemoryStream())
                    {
                        resizedImage.Save(memoryStream, ImageFormat.Bmp);
                        imageBytes = memoryStream.ToArray();
                    }
                }
            }
            imageString = Convert.ToBase64String(imageBytes);
            connection.InvokeAsync("SendPictureHub", imageString, connectionIdAdmin);
        }
    }
    
    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        connection.StopAsync();
    }
}