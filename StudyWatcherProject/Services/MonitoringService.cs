using StudyWatcherProject.Contracts;

namespace StudyWatcherProject.Services;

public class MonitoringService : IMonitoringService
{
    private readonly IMonitoringRepository _repositories;

    public MonitoringService(
        IMonitoringRepository repositories)
    {
        repositories = _repositories;
    }
    
    public async  Task<Guid> AddWorkStationRequest(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation)
    {
        var result = await _repositories
            .AddNewWorkStation(nameMotherboard, nameCPU, nameRAM, nameHDD, nameVideocard, nameLocation);
        return result.Id;
    }

    public async  Task<Guid> AddProcessBanRequest(string processBan)
    {
        var result = await _repositories
            .AddProcessBan(processBan);
        return result.Id;
    }

    public async  Task<Guid> GetBannerResponse(string nameProcessBan)
    {
        var result = await _repositories
            .GetBanner(nameProcessBan);
        return result.Id;
    }
    
    // Перед добавлением в бд, добаваить проверку на существование в бд.
    public async Task<Guid> AddProcessRequest(
        string nameProcess,
        DateTime lastLaunch,
        Guid idWorkStation)
    {
        var result = await _repositories
            .AddProcess(nameProcess, lastLaunch, idWorkStation);
        return result.Id;
    }

    public async Task<List<string>> AddProcessListRequest(
        List<string> nameProcess,
        DateTime lastLaunch,
        Guid idWorkStation)
    {
        var result = await _repositories
            .AddProcessList(nameProcess, lastLaunch, idWorkStation);
        return result;
    }
}