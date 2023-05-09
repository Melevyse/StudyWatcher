using Microsoft.AspNetCore.SignalR.Client;

namespace StudyWatcherFormsAdmin;

public partial class MainForm : Form
{
    private readonly HubConnection _hubConnection;
    public MainForm()
    {
        // Создать конфигурационный файл, который будет определять физ. позицию компьютера
        // и адрес подключения к серверу
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:44300")
            .Build();
        InitializeComponent();

        // Создать колекцию процессов подключаенных компьютеров
        connection.On("RegisterWorkStation", (
            string nameMotherboard,
            string nameCPU,
            string nameRAM,
            string nameHDD,
            string nameVideocard,
            string nameLocation,
            string connectionId) =>
        {
            ListViewItem listViewItem = new ListViewItem(nameLocation);
            listViewItem.SubItems.Add("-");
            listViewItem.SubItems.Add("-");
            listViewItem.SubItems.Add(nameMotherboard);
            listViewItem.SubItems.Add(nameCPU);
            listViewItem.SubItems.Add(nameRAM);
            listViewItem.SubItems.Add(nameHDD);
            listViewItem.SubItems.Add(nameVideocard);
            listViewItem.SubItems.Add("Offline");
            listViewItem.SubItems.Add(connectionId);
            listWorkStationForm.Items.Add(listViewItem);   
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
                foundItem.SubItems[0].Text = fio;
                foundItem.SubItems[0].Text = group;
                foundItem.SubItems[7].Text = "Online";
                foundItem.SubItems[8].Text = connectionId;
            }
        });

        connection.On("UserUsedBlackListProcess", (
            string processBan,
            string connectionId) =>
        {
            ListViewItem foundItem = listWorkStationForm
                .FindItemWithText(connectionId, false, 0, true);
            if (foundItem != null)
                foundItem.SubItems[7].Text = "Block";
        });

        connection.On("UpdateProcessBlackList", (
            string processBan) =>
        {
            listProcessBanForm.Items.Add(processBan);
        });
        
        // Доделать
        connection.On("AddItemProcessList", (string nameProcess
        ) =>
        {
            listProcessForm.Items.Add(nameProcess);
        });

        connection.On("AdminConnectionComplete", (
            string connectionId) =>
        {
            // Не требует обработки на данном этапе
        });

        connection.StartAsync().Wait();
    }
}