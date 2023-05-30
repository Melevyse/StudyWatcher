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
                    workStationItem.workStationUpdate(nameMotherboard, nameCPU, nameRAM, 
                            nameHDD, nameVideocard, Status.Login, 
                            nameLocation, connectionId, listWorkStationForm);
                }
                else
                { 
                    var workStation = new WorkStation();
                    
                    var result = workStation
                        .workStationAdd(
                            "-", "-", nameMotherboard, 
                            nameCPU, nameRAM, nameHDD, 
                            nameVideocard, Status.Login, nameLocation, connectionId);
                    WorkStations.Add(workStation);
                    listWorkStationForm.Items.Add(result);
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
                    var itemFound = false;
                    result.NameLocation = nameLocation;

                    foreach (var element in WorkStations)
                    {
                        if (element.NameLocation == nameLocation)
                        {
                            itemFound = true;
                            break;
                        }
                    }
                    if (itemFound)
                        result.infoList = "Запустился в прежней конфигурации";
                    else
                        result.infoList = "Новое устройство";
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
                    workStation.workStationUpdate("-", "-", 
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
                workStation.workStationUpdate(fio, group, 
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
            var result = workStation
                .workStationAdd(
                "-", "-", elemet.NameMotherboard, 
                elemet.NameCPU, elemet.NameRAM, elemet.NameHDD, 
                elemet.NameVideocard, Status.Offline, 
                elemet.NameLocation, "");
            WorkStations.Add(workStation);
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