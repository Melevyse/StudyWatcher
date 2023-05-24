using StudyWatcherProject.Models;

namespace StudyWatcherProject.Contracts;

public interface IMonitoringRepository
{
    Task<List<string>> GetInfoWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation);
    Task<WorkStation> AddNewWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation);

    Task<ProcessBan> AddProcessBan(
        string nameProcessBan);

    Task<ProcessBan> RemoveProcessBan(
        string nameProcessBan);

    Task<ProcessBan> GetBanner(
        string nameProcessBan);

    Task<ProcessWs> AddProcess(
        string nameProcess,
        DateTime lastLaunch,
        string nameLocation);
    
    Task<List<string>> AddProcessList(
        List<string> listProcess,
        DateTime lastLaunch,
        string nameLocation);

    Task<List<string>> GetBlackList();
    
    Task<List<string>> GetProcessList(
        string nameLocation, 
        DateTime lastLaunch);

    Task<List<ProcessWs>> GetProcessWs();
}