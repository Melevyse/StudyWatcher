using Microsoft.AspNetCore.SignalR.Client;
using System.Management;

namespace StudyWatcherFormsUser;

public partial class Form1 : Form
{
    public Form1()
    {
        string connectionIdAdmin = "";
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:44300")
            .Build();
        InitializeComponent();
        var connectionId = connection.ConnectionId;
        
        connection.On("CloseStartBanner", () =>
        {
            //На данный момент форма не создана
        });
        
        connection.On("ErrorLoginPassword", () =>
        {
            //На данный момент форма не создана
        });
        
        connection.On("AdminConnectionComplete", (
            string connectionId) =>
        {
            connectionIdAdmin = connectionId;
        });
        
        connection.On("OpenBlackListBanner", () =>
        {
            //На данный момент форма не создана
        });
        
        connection.On("CloseBlackListBanner", () =>
        {
            //На данный момент форма не создана
        });
        
        connection.StartAsync().Wait();

        string nameMotherboard = ;
        string nameCPU = systemManagmentSearchCPU();
        string nameRAM = systemManagmentSearchRAM();
        string nameHDD = systemManagmentSearchHDD();
        string nameVideocard = ;

        if (connectionIdAdmin != "")
            connection.InvokeAsync("AddWorkStationHub",
                nameMotherboard,
                nameCPU,
                nameRAM,
                nameHDD,
                nameVideocard,
                nameLocation,
                listProcess,
                lastLaunch,
                connectionIdAdmin,
                connectionId);
    }

    public string systemManagmentSearchCPU()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_Processor");
        string result = "";
        int iter = 0;
        foreach (ManagementObject obj in searcher.Get())
        {
            if (iter !=0)
                result += ", ";
            result += obj["Name"];
            iter++;
        }
        return result;
    }

    public string systemManagmentSearchRAM()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory");
        long memoryCapacity = 0;
        string result;
        foreach (ManagementObject obj in searcher.Get())
        {
            memoryCapacity += Convert.ToInt64(obj["Capacity"]);
        }
        memoryCapacity = memoryCapacity / (1024 * 1024 * 1024));
        result = memoryCapacity + " ГБ";
        return result;
    }

    public string systemManagmentSearchHDD()
    {
        string result = "";
        int iter = 0;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Model, Size from Win32_DiskDrive");
        foreach (ManagementObject obj in searcher.Get())
        {
            if (iter !=0)
                result += ", ";
            string memory = (Convert.ToInt64(obj["Size"]) / (1024 * 1024 * 1024)).ToString();
            result += obj["Model"] + " " +memory;
            iter++;
        }
        return result;
    }

    public string systemManagmentSearchVidocard()
    {
        string result = "";
        return result;
    }

    public string systemManagmentSearchMotherboard()
    {
        string result = "";
        return result;
    }
}