using Microsoft.AspNetCore.SignalR.Client;
using System.Xml;
using StudyWatcherProject.Models;


namespace StudyWatcherFormsAdmin;

public partial class MainForm : Form
{
    private HubConnection connection;
    private List<string> BlackList;

    public async Task ConnectionHub()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5123/hub")
            .WithAutomaticReconnect()
            .Build();
        connection.StartAsync().Wait();
    }

    public MainForm()
    {
        ConnectionHub();
        InitializeComponent();

        connection.On("RegisterWorkStation", (
            string nameMotherboard,
            string nameCPU,
            string nameRAM,
            string nameHDD,
            string nameVideocard,
            string nameLocation,
            string connectionId) =>
        {
            listWorkStationForm.Invoke((MethodInvoker)delegate
            {
                ListViewItem listViewItem = new ListViewItem(nameLocation);
                listViewItem.SubItems.Add("-");
                listViewItem.SubItems.Add("-");
                listViewItem.SubItems.Add(nameMotherboard);
                listViewItem.SubItems.Add(nameCPU);
                listViewItem.SubItems.Add(nameRAM);
                listViewItem.SubItems.Add(nameHDD);
                listViewItem.SubItems.Add(nameVideocard);
                listViewItem.SubItems.Add("Login");
                listViewItem.SubItems.Add(connectionId);
                listWorkStationForm.Items.Add(listViewItem);
            });
        });

        connection.On("AddItemUser", (
            string fio,
            string group,
            string connectionId) =>
        {
            Invoke((MethodInvoker)delegate
            {
                ListViewItem foundItem = listWorkStationForm
                    .FindItemWithText(connectionId, true, 0, true);
                if (foundItem != null)
                {
                    foundItem.SubItems[1].Text = fio;
                    foundItem.SubItems[2].Text = group;
                    foundItem.SubItems[8].Text = "Online";
                }
            });
        });

        connection.On("UserUsedBlackListProcess", (
            string processBan,
            string connectionId) =>
        {
            Invoke((MethodInvoker)delegate
            {
                ListViewItem foundItem = listWorkStationForm
                    .FindItemWithText(connectionId, true, 0, true);
                if (foundItem != null)
                    foundItem.SubItems[8].Text = "Block";
                var nameItem = foundItem.Text;
                var messageFirst = new ListViewItem(nameItem);
                messageFirst.SubItems.Add($"Запуск запрещенной программы {processBan}");
                var messageSecond = new ListViewItem(nameItem);
                messageSecond.SubItems.Add("Экран заблокирован");
                listViewMessage.Items.Add(messageFirst);
                listViewMessage.Items.Add(messageSecond);
            });
        });
        
        connection.On("GetProcessListUpdate", (
            List<string> processList,
            string connectionId) =>
        {
            Invoke((MethodInvoker)delegate
            {
                if (listWorkStationForm.SelectedItems.Count > 0)
                {
                    var selectedItem = listWorkStationForm.SelectedItems[0];
                    if (selectedItem.SubItems[9].Text == connectionId)
                    {
                        listProcessForm.Clear();
                        foreach (var element in processList)
                        {
                            var listViewItem = new ListViewItem(element);
                            listProcessForm.Items.Add(listViewItem);
                        }
                    }
                }
            });
        });

        connection.On("SendPicture", (
            string imageString) =>
        {
            byte[] imageData = Convert.FromBase64CharArray(imageString.ToCharArray(), 0, imageString.Length);
            ImageConverter converter = new ImageConverter();
            Image receivedImage = (Image)converter.ConvertFrom(imageData);
            Image resizedImage = ResizeImage(receivedImage, pictureBoxTranslator.Width, pictureBoxTranslator.Height);
            pictureBoxTranslator.Image = resizedImage;
        });

        connection.On("GetProcessList", (
            List<string> processList) =>
        {
            Invoke((MethodInvoker)delegate
            {
                listProcessForm.Clear();
                foreach (var element in processList)
                {
                    var listViewItem = new ListViewItem(element);
                    listProcessForm.Items.Add(listViewItem);
                }
            });
        });

        connection.On("ResponseBlackList", (
            List<string> listProcessBan) =>
        {
            Invoke((MethodInvoker)delegate
            {
                if (BlackList == null)
                    BlackList = new List<string>();
                if (BlackList.Count != 0)
                    BlackList.Clear();
                BlackList.AddRange(listProcessBan);
                BlackList = BlackList.Distinct().ToList();
                listProcessBanForm.Items.Clear();
                foreach (var element in BlackList)
                {
                    ListViewItem listViewItem = new ListViewItem(element);
                    listProcessBanForm.Items.Add(listViewItem);
                }
            });
        });

        connection.On("AnovaMethod", (
            List<ProcessWs> listProcessWs) =>
        {
            Invoke((MethodInvoker)delegate
            {
                var anovaAlgorithm = new AnovaAlgorithm(listProcessWs);
                var nameProcessList = anovaAlgorithm.processArray;
                var countProcessList = anovaAlgorithm.rowSums;
                var anovaTable = anovaAlgorithm.anovaResult.Table;
                var anovaFrom = new AnovaFrom(nameProcessList, countProcessList, anovaTable);
                anovaFrom.Show();
            });
        });

        connection.StartAsync();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        ConnectionAdminTimer.Start();
    }

    private void buttonAddProcessBan_Click(object sender, EventArgs e)
    {
        if (listProcessForm.SelectedItems.Count > 0)
        {
            if (listProcessForm.SelectedItems.Count == 1)
            {
                ListViewItem selectedItem = listProcessForm.SelectedItems[0];
                connection.InvokeAsync("AddProcessListBanHub",
                    selectedItem.Text, connection.ConnectionId);
                listProcessBanForm.Items.Add(selectedItem.Text);
            }
            else
            {
                foreach (ListViewItem selectedItem in listProcessForm.SelectedItems)
                {
                    connection.InvokeAsync("AddProcessListBanHub",
                        selectedItem.Text, connection.ConnectionId);
                    listProcessBanForm.Items.Add(selectedItem.Text);
                }
            }
        }
    }

    private void ConnectionAdminTimer_Tick(object sender, EventArgs e)
    {
        connection.InvokeAsync("GetAdminConnectionIdHub");
    }

    private void listWorkStationForm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listWorkStationForm.SelectedItems.Count > 0)
        {
            ListViewItem selectedItem = listWorkStationForm.SelectedItems[0];
            string nameLocation = selectedItem.Text;
            DateTime lastLaunch = DateTime.UtcNow.Date;
            string connectionIdItem = selectedItem.SubItems[9].Text;
            connection.InvokeAsync("GetProcessListHub", nameLocation, lastLaunch, connection.ConnectionId);
            connection.InvokeAsync("RequestPictureHub", connectionIdItem);
        }
    }

    private Image ResizeImage(Image image, int width, int height)
    {
        Bitmap resizedImage = new Bitmap(width, height);
        using (Graphics graphics = Graphics.FromImage(resizedImage))
        {
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, 0, 0, width, height);
        }
        return resizedImage;
    }

    private void buttonUnbanUser_Click(object sender, EventArgs e)
    {
        if (listWorkStationForm.SelectedItems.Count > 0)
        {
            var selectedItem = listWorkStationForm.SelectedItems[0];
            var connectionid = selectedItem.SubItems[9].Text;
            var nameItem = selectedItem.Text;
            selectedItem.SubItems[8].Text = "Online";
            connection.InvokeAsync("BannerCloseHub", connectionid);
            var message = new ListViewItem(nameItem);
            message.SubItems.Add("Экран разблокирован");
            listViewMessage.Items.Add(message);
        }
    }

    private void buttonDeleteProcessBan_Click(object sender, EventArgs e)
    {
        if (listProcessBanForm.SelectedItems.Count > 0)
        {
            ListViewItem selectedItem = listProcessBanForm.SelectedItems[0];
            string process = selectedItem.Text;
            connection.InvokeAsync("RemoveProcessListBanHub", process);
            selectedItem.Remove();
        }
    }

    private void buttonAnova_Click(object sender, EventArgs e)
    {
        connection.InvokeAsync("GetFullProcessWsHub", connection.ConnectionId);
    }
}