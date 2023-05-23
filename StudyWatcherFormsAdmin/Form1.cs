using Microsoft.AspNetCore.SignalR.Client;
using System.Xml;


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
            ListViewItem foundItem = listWorkStationForm
                .FindItemWithText(connectionId, false, 0, true);
            if (foundItem != null)
            {
                foundItem.SubItems[1].Text = fio;
                foundItem.SubItems[2].Text = group;
                foundItem.SubItems[8].Text = "Online";
            }
        });

        connection.On("UserUsedBlackListProcess", (
            string processBan,
            string connectionId) =>
        {
            ListViewItem foundItem = listWorkStationForm
                .FindItemWithText(connectionId, false, 0, true);
            if (foundItem != null)
                foundItem.SubItems[8].Text = "Block";
        });

        connection.On("AddItemProcessList", (
            string nameProcess) =>
        {
            listProcessForm.Items.Add(nameProcess);
        });

        connection.On("SendPicture", (
            byte[] imageData) =>
        {
            Invoke((MethodInvoker)delegate
            {
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    Image receivedImage = Image.FromStream(stream);
                    Image resizedImage = ResizeImage(receivedImage, pictureBoxTranslator.Width, pictureBoxTranslator.Height);
                    pictureBoxTranslator.Image = resizedImage;
                }
            });
        });

        connection.On("GetProcessList", (
            List<string> processList) =>
        {
            Invoke((MethodInvoker)delegate
            {
                foreach (var element in processList)
                {
                    ListViewItem listViewItem = new ListViewItem(element);
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
            connection.InvokeAsync("RequestPictureHub", connectionIdItem);
            connection.InvokeAsync("GetProcessListHub", nameLocation, lastLaunch, connection.ConnectionId);
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
            ListViewItem selectedItem = listWorkStationForm.SelectedItems[0];
            string connectionid = selectedItem.SubItems[9].Text;
            selectedItem.SubItems[8].Text = "Online";
            connection.InvokeAsync("BannerCloseHub", connectionid);
        }
    }

    private void buttonDeleteProcessBan_Click(object sender, EventArgs e)
    {
        if (listProcessBanForm.SelectedItems.Count > 0)
        {
            ListViewItem selectedItem = listWorkStationForm.SelectedItems[0];
            string process = selectedItem.Text;
            connection.InvokeAsync("RemoveProcessListBanHub", process);
            selectedItem.Remove();
        }
    }

    private void buttonAnova_Click(object sender, EventArgs e)
    {

    }
}