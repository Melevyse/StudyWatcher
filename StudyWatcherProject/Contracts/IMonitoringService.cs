using StudyWatcherProject.Models;

namespace StudyWatcherProject.Contracts;

public interface IMonitoringService
{
    Task<List<string>> GetFullInfoWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation);
    Task<Guid> AddWorkStationRequest(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation);
    
    Task<Guid> AddProcessBanRequest(
        string processBan);

    Task<Guid> RemoveProcessBanRequest(
        string processBan);
    
    Task<Guid> GetBannerResponse(
        string processBan);

    Task<List<string>> AddProcessListRequest(
        List<string> listProcess,
        DateTime lastLaunch,
        string nameLocation);

    Task<List<string>> GetFullBlackList();
    Task<List<string>> GetFullProcessList(
        string nameLocation, 
        DateTime lastLaunch);
    Task<List<ProcessWs>> GetFullProcessWs();
    Task<List<WorkStation>> GetFullWorkStation();
}