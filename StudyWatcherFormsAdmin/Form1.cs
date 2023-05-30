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
                var workStationItem = WorkStations
                    .FirstOrDefault(x => x.NameLocation == nameLocation);
                if (workStationItem != null)
                {
                    workStationItem.WorkStationUpdate(nameMotherboard, nameCPU, nameRAM, 
                            nameHDD, nameVideocard, Status.Login, 
                            connectionId, listWorkStationForm);
                }
                else
                { 
                    var workStation = new WorkStation("-", "-", nameMotherboard, 
                        nameCPU, nameRAM, nameHDD, 
                        nameVideocard, Status.Login, nameLocation, connectionId, listWorkStationForm);
                    WorkStations.Add(workStation);
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
                    InfoWorkStation result;
                    var itemFound = false;
                    foreach (var element in WorkStations)
                    {
                        if (element.NameLocation == nameLocation)
                        {
                            itemFound = true;
                            break;
                        }
                    }
                    if (itemFound)
                    {
                        result = new InfoWorkStation(
                            nameLocation, "Запустился в прежней конфигурации",listViewMessage);
                        InfoWorkStationList.Add(result);
                    }
                    else
                    {
                        result = new InfoWorkStation(
                            nameLocation, "Новое устройство",listViewMessage);
                        InfoWorkStationList.Add(result);
                    }
                }
                else
                {
                    foreach (var element in infoWorkStation)
                    {
                        var resultItem = new InfoWorkStation(
                            nameLocation, $"Компонент был утрачен или заменен: {element}", listViewMessage);
                        InfoWorkStationList.Add(resultItem);
                    }
                }
            });
        });
        
        connection.On("ClientOffline" , (
            string connectionId) =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                var workStation = WorkStations
                    .FirstOrDefault(x => x.ConnectionId == connectionId);
                if (workStation != null)
                {
                    var workStationViewItem = listWorkStationForm
                        .FindItemWithText(workStation.ConnectionId, true, 0);
                    workStation.WorkStationUpdate("-", "-", 
                        Status.Offline, listWorkStationForm);
                    WorkStations.Remove(workStation);
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
                workStation.WorkStationUpdate(fio, group, 
                    Status.Online, listWorkStationForm);
            });
        });
        
        connection.On("UserUsedBlackListProcess", (
            string connectionId) =>
        {
            BeginInvoke((MethodInvoker)delegate
            {
                var workStation = WorkStations
                    .FirstOrDefault(x => x.ConnectionId == connectionId);
                workStation.WorkStationUpdate(Status.Block, listWorkStationForm);
                var resultFirst = new InfoWorkStation(
                    workStation.NameLocation, "Запуск запрещенной программы, экран заблокирован", listViewMessage);
                InfoWorkStationList.Add(resultFirst);
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
            var workStation = new WorkStation(
                "-", "-", elemet.NameMotherboard,
                elemet.NameCPU, elemet.NameRAM, elemet.NameHDD,
                elemet.NameVideocard, Status.Offline,
                elemet.NameLocation, "", listWorkStationForm);
            WorkStations.Add(workStation);
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
            listProcessForm.Items.Clear();
            var workStation = WorkStations
                .FirstOrDefault(x => x.NameLocation == nameLocation);
            if (workStation.Status != Status.Offline)
            {
                workStation.ProcessList = await connection
                    .InvokeAsync<List<string>>("GetProcessListHub", 
                        nameLocation, lastLaunch);;
                foreach (var element in workStation.ProcessList)
                {
                    var resultItem = new ListViewItem(element);
                    listProcessForm.Items.Add(resultItem);
                }
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
                workStation.WorkStationUpdate(Status.Online, listWorkStationForm);
                connection.InvokeAsync("BannerCloseHub", workStation.ConnectionId);
                var infoWorkStation = new InfoWorkStation(
                    workStation.NameLocation, "Экран разблокирован", listViewMessage);
                InfoWorkStationList.Add(infoWorkStation);
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