using Microsoft.AspNetCore.SignalR.Client;

namespace StudyWatcherFormsAdmin;

public partial class MainForm : Form
{
    private readonly HubConnection _hubConnection;
    public MainForm()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:44300/StudyWatcherHub")
            .Build();
        InitializeComponent();
    }
}