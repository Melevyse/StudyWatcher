namespace StudyWatcherProject.Contracts;

public interface IMonitoringService
{
    Task<Guid> AddWorkStationRequest(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard);
    
    Task<Guid> AddProcessBanRequest(
        string processBan);
    
    Task<Guid> GetBannerResponse(
        string processBan);
    
    Task<Guid> AddProcessRequest(
        string nameProcess,
        DateTime lastLaunch,
        Guid idWorkStation);
    
    Task<Guid> AddProcessListRequest(
        List<string> listProcess,
        DateTime lastLaunch,
        Guid idWorkStation);
}