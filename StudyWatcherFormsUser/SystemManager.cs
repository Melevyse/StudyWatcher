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
        List<string> result = new List<string>();
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            result.Add(process.ProcessName);
        }
        result = result.Distinct().ToList();
        this.listProcess = result;
    }

    public void systemManagmentSearchCPU()
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
        this.nameHDD = result;
    }

    public void systemManagmentSearchVidocard()
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
            result += obj["Name"] + " " + memory + " ГБ";
            iter++;
        }
        this.nameVideocard = result;
    }

    public void systemManagmentSearchMotherboard()
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
        this.nameMotherboard = result;
    }
}