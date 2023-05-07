using StudyWatcherProject.Models;

namespace StudyWatcherProject.Contracts;

public interface IMonitoringRepository
{
    Task<WorkStation> AddNewWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard);

    Task<ProcessBan> AddProcessBan(
        string nameProcessBan);

    Task<ProcessBan> GetBanner(
        string nameProcessBan);

    Task<ProcessWS> AddProcess(
        string nameProcess,
        DateTime lastLaunch,
        Guid idWorkStation);
    
    Task<ProcessWS> AddProcessList(
        List<string> listProcess,
        DateTime lastLaunch,
        Guid idWorkStation);
}