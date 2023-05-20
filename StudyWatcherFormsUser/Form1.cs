using Microsoft.AspNetCore.SignalR.Client;
using System.Management;
using System.Diagnostics;

namespace StudyWatcherFormsUser;

public partial class Form1 : Form
{
    private HubConnection connection;
    string connectionIdAdmin;
    
    public async Task ConnectionHub()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5123/hub")
            .WithAutomaticReconnect()
            .Build();
        connection.StartAsync().Wait();
    }
    
    public Form1()
    {
        ConnectionHub();
        InitializeComponent();

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

        connection.StartAsync();
    }

    public string systemManagmentSearchCPU()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_Processor");
        string result = "";
        int iter = 0;
        foreach (ManagementObject obj in searcher.Get())
        {
            if (iter != 0)
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
        memoryCapacity = (memoryCapacity / (1024 * 1024 * 1024));
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
            if (iter != 0)
                result += ", ";
            string memory = (Convert.ToInt64(obj["Size"]) / (1024 * 1024 * 1024)).ToString();
            result += obj["Model"] + " " + memory;
            iter++;
        }
        return result;
    }

    public string systemManagmentSearchVidocard()
    {
        string result = "";
        int iter = 0;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
        ManagementObjectCollection collection = searcher.Get();
        foreach (ManagementObject obj in collection)
        {
            if (iter != 0)
                result += ", ";
            string memory = (Convert.ToInt64(obj["AdapterRAM"]) / (1024 * 1024 * 1024)).ToString();
            result += obj["Name"] + " " + obj["AdapterRAM"];
            iter++;
        }
        return result;
    }

    public string systemManagmentSearchMotherboard()
    {
        string result = "";
        int iter = 0;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
        ManagementObjectCollection collection = searcher.Get();
        foreach (ManagementObject obj in collection)
        {
            if (iter != 0)
                result += ", ";
            result += obj["Product"] + " " + obj["SerialNumber"];
            iter++;
        }

        return result;
    }

    public List<string> systemProcess()
    {
        List<string> result = new List<string>();
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            result.Add(process.ProcessName);
        }
        return result;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        HubConnectionTimer.Start();
    }

    private void HubConnectionTimer_Tick(object sender, EventArgs e)
    {
        
        if (connectionIdAdmin != null)
        {
            string nameMotherboard = systemManagmentSearchMotherboard();
            string nameCPU = systemManagmentSearchCPU();
            string nameRAM = systemManagmentSearchRAM();
            string nameHDD = systemManagmentSearchHDD();
            string nameVideocard = systemManagmentSearchVidocard();
            string nameLocation = "StudentComputer";
            List<string> listProcess = systemProcess();
            DateTime lastLaunch = DateTime.UtcNow;
            var connectionId = connection.ConnectionId;
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
            HubConnectionTimer.Stop();
        }
    }
}