namespace StudyWatcherFormsUser;
using System.Management;
using System.Diagnostics;

public class SystemManager
{
    public string nameMotherboard;
    public string nameCPU;
    public string nameRAM;
    public string nameHDD;
    public string nameVideocard;
    public List<string> listProcess;

    public SystemManager()
    {
        systemManagmentSearchCPU();
        systemManagmentSearchRAM();
        systemManagmentSearchHDD();
        systemManagmentSearchVidocard();
        systemManagmentSearchMotherboard();
        systemListProcess();
    }

    public void systemListProcess()
    {
        var result = new List<string>();
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            result.Add(process.ProcessName);
        }
        result = result.Distinct().ToList();
        this.listProcess = result;
    }

    public void systemManagmentSearchCPU()
    {
        var searcher = new ManagementObjectSearcher("select Name from Win32_Processor");
        var result = "";
        var iter = 0;
        foreach (ManagementObject obj in searcher.Get())
        {
            if (iter != 0)
                result += ", ";
            result += obj["Name"];
            iter++;
        }
        this.nameCPU = result;
    }

    public void systemManagmentSearchRAM()
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
        this.nameRAM = result;
    }

    public void systemManagmentSearchHDD()
    {
        var result = "";
        var iter = 0;
        var searcher = new ManagementObjectSearcher("select Model, Size from Win32_DiskDrive");
        foreach (var obj in searcher.Get())
        {
            if (iter != 0)
                result += ", ";
            string memory = (Convert.ToInt64(obj["Size"]) / (1024 * 1024 * 1024)).ToString();
            result += obj["Model"] + " " + memory;
            iter++;
        }
        this.nameHDD = result;
    }

    public void systemManagmentSearchVidocard()
    {
        var result = "";
        var iter = 0;
        var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
        var collection = searcher.Get();
        foreach (var obj in collection)
        {
            if (iter != 0)
                result += ", ";
            string memory = (Convert.ToInt64(obj["AdapterRAM"]) / (1024 * 1024 * 1024)).ToString();
            result += obj["Name"] + " " + memory + " ГБ";
            iter++;
        }
        this.nameVideocard = result;
    }

    public void systemManagmentSearchMotherboard()
    {
        var result = "";
        var iter = 0;
        var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
        var collection = searcher.Get();
        foreach (var obj in collection)
        {
            if (iter != 0)
                result += ", ";
            result += obj["Product"] + " " + obj["SerialNumber"];
            iter++;
        }
        this.nameMotherboard = result;
    }
}