using Microsoft.EntityFrameworkCore;
using StudyWatcherProject.Contracts;
using StudyWatcherProject.EFC;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.Repositories;

public class MonitoringRepository : IMonitoringRepository
{
    private readonly SqlReportingContext _context;
    private readonly Logger<MonitoringRepository> _logger;

    public MonitoringRepository(
        SqlReportingContext context,
        Logger<MonitoringRepository> logger)
    {
        context = _context;
        logger = _logger;
    }

    public async Task<WorkStation> AddNewWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard)
    {
        var request = new WorkStation()
        {
            NameMotherboard = nameMotherboard,
            NameCPU = nameCPU,
            NameRAM = nameRAM,
            NameHDD = nameHDD,
            NameVideocard = nameVideocard
        };
        //
        _context.Add(request);
        _context.SaveChanges();
        return request;
    }

    public async Task<ProcessBan> AddProcessBan(string nameProcessBan)
    {
        var result = new ProcessBan()
        {
            NameProcess = nameProcessBan
        };
        _context.Add(result);
        _context.SaveChanges();
        return result;
    }

    public async Task<ProcessBan> GetBanner(string nameProcessBan)
    {
        var result = new ProcessBan()
        {
            NameProcess = nameProcessBan
        };
        var check = await _context.ProcessBan
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.NameProcess == nameProcessBan);
        if (result == check) return result;
        return check ?? throw new ArgumentException("Request is not found in the database");
    }
    
    public async Task<ProcessWS> AddProcess(
        string nameProcess,
        DateTime lastLaunch,
        Guid idWorkStation)
    {
        var result = new ProcessWS()
        {
            NameProcess = nameProcess,
            LastLaunch = lastLaunch,
            IdWorkStation = idWorkStation
        };
        _context.Add(result);
        _context.SaveChanges();
        return result;
    }
    
    public async Task<ProcessWS> UpdateProcess(
        string nameProcess,
        DateTime lastLaunch,
        Guid idWorkStation)
    {
        var result = new ProcessWS()
        {
            NameProcess = nameProcess,
            LastLaunch = lastLaunch,
            IdWorkStation = idWorkStation
        };
        var check = await _context.ProcessWS
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.NameProcess == nameProcess &&
                x.IdWorkStation == idWorkStation &&
                x.LastLaunch == lastLaunch);
        if (result != check)
        {
            check = await _context.ProcessWS
                .FirstOrDefaultAsync(x => 
                    x.NameProcess == nameProcess &&
                    x.IdWorkStation == idWorkStation);
            if (check != null)
            {
                check.LastLaunch = lastLaunch;
                return check; 
            }
            return check ?? throw new ArgumentException("No record in database");
        }
        return result;
    }

    // ДОДЕЛАТЬ!! return не правильный
    public async Task<ProcessWS> AddProcessList(
        List<string> nameProcessList,
        DateTime lastLaunch,
        Guid idWorkStation)
    {
        foreach (var element in nameProcessList)
        {
            var result = new ProcessWS()
            {
                NameProcess = element,
                LastLaunch = lastLaunch,
                IdWorkStation = idWorkStation
            };
            var check = await _context.ProcessWS
                .AsNoTracking()
                .FirstOrDefaultAsync(x => 
                    x.NameProcess == element &&
                    x.IdWorkStation == idWorkStation &&
                    x.LastLaunch == lastLaunch);
        }
        return null;
    }
}