using Microsoft.AspNetCore.SignalR.Client;

namespace StudyWatcherFormsUser;

public partial class Form1 : Form
{
    public Form1()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:44300")
            .Build();
        InitializeComponent();
        
        connection.On("CloseStartBanner", () =>
        {
            
        });
        
        connection.On("ErrorLoginPassword", () =>
        {
            
        });
        
        connection.On("AdminConnectionComplete", (
            string connectionId) =>
        {
            
        });
        
        connection.On("OpenBlackListBanner", () =>
        {
            
        });
        
        connection.On("CloseBlackListBanner", () =>
        {
            
        });
    }
}