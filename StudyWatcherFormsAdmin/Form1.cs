using Microsoft.AspNetCore.SignalR.Client;
using System.Xml;
using StudyWatcherProject.Models;


namespace StudyWatcherFormsAdmin;

public partial class MainForm : Form
{
    private HubConnection connection;
    private List<string> BlackList = new ();
    private List<WorkStation> WorkStations = new ();
    private List<ProcessWs> ProcessWsList = new ();
    private List<InfoWorkStation> InfoWorkStationList = new ();


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
            BeginInvoke((MethodInvoker)delegate
            {
                var workStation = WorkStations
                    .FirstOrDefault(x => x.NameLocation == nameLocation);
                if (workStation != null)
                {
                    var workStationViewItem = listWorkStationForm
                        .FindItemWithText(workStation.NameLocation, true, 0);
                    workStation.NameMotherboard = nameMotherboard;
                    workStation.NameCPU = nameCPU;
                    workStation.NameRAM = nameRAM;
                    workStation.NameHDD = nameHDD;
                    workStation.NameVideocard = nameVideocard;
                    workStation.Status = Status.Login;
                    workStation.ConnectionId = connectionId;
                    workStationViewItem.SubItems[3].Text = workStation.NameMotherboard;
                    workStationViewItem.SubItems[4].Text = workStation.NameCPU;
                    workStationViewItem.SubItems[5].Text = workStation.NameRAM;
                    workStationViewItem.SubItems[6].Text = workStation.NameHDD;
                    workStationViewItem.SubItems[7].Text = workStation.NameVideocard;
                    workStationViewItem.SubItems[8].Text = workStation.Status.ToString();
                    workStationViewItem.SubItems[9].Text = workStation.ConnectionId;
                }
            });
        });

        connection.On("InfoWorkStation", (
            List<string> infoWorkStation,
            string nameLocation) =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                if (infoWorkStation.First() == "NONE")
                {
                    var result = new InfoWorkStation();
                    result.NameLocation = nameLocation;
                    result.infoList = "Запустился в прежней конфигурации";
                    InfoWorkStationList.Add(result);
                    var resultView = new ListViewItem(result.NameLocation);
                    resultView.SubItems.Add(result.infoList);
                    listViewMessage.Items.Add(resultView);
                }
                else
                {
                    foreach (var element in infoWorkStation)
                    {
                        var resultItem = new InfoWorkStation();
                        resultItem.NameLocation = nameLocation;
                        resultItem.infoList = $"Компонент был утрачен или заменен: {element}";
                        InfoWorkStationList.Add(resultItem);
                        var resultItemView = new ListViewItem(resultItem.NameLocation);
                        resultItemView.SubItems.Add(resultItem.NameLocation);
                        listViewMessage.Items.Add(resultItemView);
                    }
                }
            });
        });
        
        connection.On("AddItemUser", (
            string fio,
            string group,
            string connectionId) =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                var workStation = WorkStations
                    .FirstOrDefault(x => x.ConnectionId == connectionId);
                workStation.Fio = fio;
                workStation.Group = group;
                workStation.Status = Status.Online;
                var listViewItem = listWorkStationForm
                    .FindItemWithText(workStation.ConnectionId, true, 0);
                listViewItem.SubItems[1].Text = workStation.Fio;
                listViewItem.SubItems[2].Text = workStation.Group;
                listViewItem.SubItems[8].Text = workStation.Status.ToString();
            });
        });
        
        connection.On("UserUsedBlackListProcess", (
            string connectionId) =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                var workStation = WorkStations
                    .FirstOrDefault(x => x.ConnectionId == connectionId);
                workStation.Status = Status.Block;
                var workStationViewItem = listWorkStationForm
                    .FindItemWithText(workStation.ConnectionId, true, 0);
                workStationViewItem.SubItems[8].Text = workStation.Status.ToString();
                
                var resultFirst = new InfoWorkStation();
                resultFirst.NameLocation = workStation.NameLocation;
                resultFirst.infoList = "Запуск запрещенной программы, экран заблокирован";
                InfoWorkStationList.Add(resultFirst);
                var itemFirst = new ListViewItem(resultFirst.NameLocation);
                itemFirst.SubItems.Add(resultFirst.infoList);
                listViewMessage.Items.Add(itemFirst);
            });
        });
        
        connection.On("GetProcessListUpdate", (
            List<string> processList,
            string connectionId) =>
        {
            var workStation = WorkStations
                .FirstOrDefault(x => x.ConnectionId == connectionId);
            workStation.ProcessList = processList;
        });

        connection.On("SendPicture", (
            string imageString) =>
        {
            byte[] imageData = Convert.FromBase64String(imageString);

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                Image receivedImage = Image.FromStream(ms);
                pictureBoxTranslator.Image = receivedImage;
            }
        });

        connection.On("ResponseBlackList", (
            List<string> listProcessBan) =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                BlackList.Clear();
                foreach (var element in listProcessBan)
                {
                    BlackList.Add(element);
                    listProcessBanForm.Items.Add(element);
                }
            });
        });

        connection.StartAsync();
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        ConnectionAdminTimer.Start();
        var objects = await connection
            .InvokeAsync<List<WorkStationSo>>("GetAllWorkStationHub");
        foreach (var elemet in objects)
        {
            var workStation = new WorkStation();
            workStation.NameLocation = elemet.NameLocation;
            workStation.Fio = "-";
            workStation.Group = "-";
            workStation.NameMotherboard = elemet.NameMotherboard;
            workStation.NameCPU = elemet.NameCPU;
            workStation.NameRAM = elemet.NameRAM;
            workStation.NameHDD = elemet.NameHDD;
            workStation.NameVideocard = elemet.NameVideocard;
            workStation.Status = Status.Offline;
            workStation.ConnectionId = "";
            WorkStations.Add(workStation);
            var result = new ListViewItem(workStation.NameLocation);
            result.SubItems.Add(workStation.Fio);
            result.SubItems.Add(workStation.Group);
            result.SubItems.Add(workStation.NameMotherboard);
            result.SubItems.Add(workStation.NameCPU);
            result.SubItems.Add(workStation.NameRAM);
            result.SubItems.Add(workStation.NameHDD);
            result.SubItems.Add(workStation.NameVideocard);
            result.SubItems.Add(workStation.Status.ToString());
            result.SubItems.Add(workStation.ConnectionId);
            listWorkStationForm.Items.Add(result);
        }
    }

    private void buttonAddProcessBan_Click(object sender, EventArgs e)
    {
        if (listProcessForm.SelectedItems.Count > 0)
        { 
            var selectedItem = listProcessForm.SelectedItems[0];
            BlackList.Add(selectedItem.Text);
            connection.InvokeAsync("AddProcessListBanHub", 
                selectedItem.Text, connection.ConnectionId);
            listProcessBanForm.Items.Add(selectedItem.Text);
        }
    }

    private void ConnectionAdminTimer_Tick(object sender, EventArgs e)
    {
        connection.InvokeAsync("GetAdminConnectionIdHub");
    }

    private async void listWorkStationForm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listWorkStationForm.SelectedItems.Count > 0)
        {
            ListViewItem selectedItem = listWorkStationForm.SelectedItems[0];
            string nameLocation = selectedItem.Text;
            DateTime lastLaunch = DateTime.UtcNow.Date;
            var result = await connection
                .InvokeAsync<List<string>>("GetProcessListHub", 
                nameLocation, lastLaunch);
            var workStation = WorkStations
                .FirstOrDefault(x => x.NameLocation == nameLocation);
            workStation.ProcessList = result;
            listProcessForm.Items.Clear();
            foreach (var element in workStation.ProcessList)
            {
                var resultItem = new ListViewItem(element);
                listProcessForm.Items.Add(resultItem);
            }
            
            connection.InvokeAsync("RequestPictureHub", workStation.ConnectionId);
        }
    }

    private void buttonUnbanUser_Click(object sender, EventArgs e)
    {
        if (listWorkStationForm.SelectedItems.Count > 0)
        {
            var selectedItem = listWorkStationForm.SelectedItems[0];
            if (selectedItem.SubItems[8].Text == Status.Block.ToString())
            {
                var workStation = WorkStations
                    .FirstOrDefault(x => x.NameLocation == selectedItem.Text);
                workStation.Status = Status.Online;
                selectedItem.SubItems[8].Text = workStation.Status.ToString();
                connection.InvokeAsync("BannerCloseHub", workStation.ConnectionId);
                var infoWorkStation = new InfoWorkStation();
                infoWorkStation.NameLocation = workStation.NameLocation;
                infoWorkStation.infoList = "Экран разблокирован";
                InfoWorkStationList.Add(infoWorkStation);
                var message = new ListViewItem(infoWorkStation.NameLocation);
                message.SubItems.Add(infoWorkStation.infoList);
                listViewMessage.Items.Add(message);
            }
        }
    }

    private void buttonDeleteProcessBan_Click(object sender, EventArgs e)
    {
        if (listProcessBanForm.SelectedItems.Count > 0)
        {
            var selectedItem = listProcessBanForm.SelectedItems[0];
            var process = selectedItem.Text;
            BlackList.Remove(process);
            connection.InvokeAsync("RemoveProcessListBanHub", process);
            selectedItem.Remove();
        }
    }

    private async void buttonAnova_Click(object sender, EventArgs e)
    {
        ProcessWsList = await connection
            .InvokeAsync<List<ProcessWs>>("GetFullProcessWsHub", connection.ConnectionId);
        var anovaAlgorithm = new AnovaAlgorithm(ProcessWsList);
        var nameProcessList = anovaAlgorithm.processArray;
        var countProcessList = anovaAlgorithm.rowSums;
        var anovaTable = anovaAlgorithm.anovaResult.Table;
        var anovaFrom = new AnovaFrom(nameProcessList, countProcessList, anovaTable);
        anovaFrom.Show();
    }
}