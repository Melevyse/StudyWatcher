﻿using System.Runtime.InteropServices.ComTypes;
using StudyWatcherProject.Contracts;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.Services;

public class MonitoringService : IMonitoringService
{
    private readonly IMonitoringRepository _repositories;

    public MonitoringService(
        IMonitoringRepository repositories)
    {
        _repositories = repositories;
    }

    public async Task<List<string>> GetFullInfoWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation)
    {
        var result = await _repositories
            .GetInfoWorkStation(nameMotherboard, nameCPU, nameRAM, nameHDD , nameVideocard, nameLocation);
        return result;
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
    
    public async Task<Guid> RemoveProcessBanRequest(string processBan)
    {
        var result = await _repositories
            .RemoveProcessBan(processBan);
        return result.Id;
    }

    public async  Task<Guid> GetBannerResponse(string nameProcessBan)
    {
        var result = await _repositories
            .GetBanner(nameProcessBan);
        return result.Id;
    }

    public async Task<List<string>> AddProcessListRequest(
        List<string> nameProcess,
        DateTime lastLaunch,
        string nameLocation)
    {
        var result = await _repositories
            .AddProcessList(nameProcess, lastLaunch, nameLocation);
        return result;
    }

    public async Task<List<string>> GetFullBlackList()
    {
        var result = await _repositories.GetBlackList();
        return result;
    }

    public async Task<List<string>> GetFullProcessList(string nameLocation, DateTime lastLaunch)
    {
        var result = await _repositories
            .GetProcessList(nameLocation, lastLaunch);
        return result;
    }
    
    public async Task<List<ProcessWs>> GetFullProcessWs()
    {
        var result = await _repositories
            .GetProcessWs();
        return result;
    }

    public async Task<List<WorkStation>> GetFullWorkStation()
    {
        var result = await _repositories
            .GetWorkStation();
        return result;
    }
}