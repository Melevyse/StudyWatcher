using Microsoft.EntityFrameworkCore;
using StudyWatcherProject.Contracts;
using StudyWatcherProject.EFC;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.Repositories;

public class MonitoringRepository : IMonitoringRepository
{
    private readonly SqlReportingContext _context;
    private readonly ILogger<MonitoringRepository> _logger;

    public MonitoringRepository(
        SqlReportingContext context,
        ILogger<MonitoringRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<WorkStation> AddNewWorkStation(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation)
    {
        var existingWorkStation = await _context.WorkStation
            .FirstOrDefaultAsync(x => x.NameLocation == nameLocation);
        if (existingWorkStation != null)
        {
            existingWorkStation.NameMotherboard = nameMotherboard;
            existingWorkStation.NameCPU = nameCPU;
            existingWorkStation.NameRAM = nameRAM;
            existingWorkStation.NameHDD = nameHDD;
            existingWorkStation.NameVideocard = nameVideocard;
        }
        else
        {
            var newWorkStation = new WorkStation()
            {
                NameMotherboard = nameMotherboard,
                NameCPU = nameCPU,
                NameRAM = nameRAM,
                NameHDD = nameHDD,
                NameVideocard = nameVideocard,
                NameLocation = nameLocation
            };

            _context.Add(newWorkStation);
            existingWorkStation = newWorkStation;
        }
        await _context.SaveChangesAsync();
        return existingWorkStation;
    }

    public async Task<ProcessBan> AddProcessBan(string nameProcessBan)
    {
        var result = new ProcessBan()
        {
            NameProcess = nameProcessBan
        };
        _context.Add(result);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<ProcessBan> RemoveProcessBan(string nameProcessBan)
    {
        var processBan = await _context.ProcessBan.FindAsync(nameProcessBan);
        if (processBan != null)
        {
            _context.ProcessBan.Remove(processBan);
            await _context.SaveChangesAsync();
        }
        return processBan ?? throw new ArgumentException("Request is not found in the database");;
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
    
    public async Task<ProcessWs> AddProcess(
        string nameProcess,
        DateTime lastLaunch,
        string nameLocation)
    {
        var result = new ProcessWs()
        {
            NameProcess = nameProcess,
            LastLaunch = lastLaunch,
            NameLocation = nameLocation
        };
        _context.Add(result);
        await _context.SaveChangesAsync();
        return result;
    }
    
    public async Task<ProcessWs> UpdateProcess(
        string nameProcess,
        DateTime lastLaunch,
        string nameLocation)
    {
        var result = new ProcessWs()
        {
            NameProcess = nameProcess,
            LastLaunch = lastLaunch,
            NameLocation = nameLocation
        };
        var check = await _context.ProcessWs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.NameProcess == nameProcess &&
                x.NameLocation == nameLocation &&
                x.LastLaunch == lastLaunch);
        if (result != check)
        {
            check = await _context.ProcessWs
                .FirstOrDefaultAsync(x => 
                    x.NameProcess == nameProcess &&
                    x.NameLocation == nameLocation);
            if (check != null)
            {
                check.LastLaunch = lastLaunch;
                await _context.SaveChangesAsync();
                return check; 
            }
            return check ?? throw new ArgumentException("No record in database");
        }
        _context.Add(result);
        await _context.SaveChangesAsync();
        return check ?? throw new ArgumentException("No record in database");
    }
    
    public async Task<List<string>> AddProcessList(
        List<string> nameProcessList,
        DateTime lastLaunch,
        string nameLocation)
    {
        List<string> result = new List<string>();
        foreach (var element in nameProcessList)
        {
            var iter = new ProcessWs()
            {
                NameProcess = element,
                LastLaunch = lastLaunch,
                NameLocation = nameLocation
            };
            var check = await _context.ProcessWs
                .FirstOrDefaultAsync(x => 
                    x.NameProcess == element &&
                    x.LastLaunch == lastLaunch &&
                    x.NameLocation == nameLocation);
            if (check == null)
            {
                result.Add(iter.NameProcess);
                _context.Add(iter);
                await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<List<string>> GetBlackList()
    {
        var result = new List<string>();
        var check = await _context.ProcessBan.ToListAsync();
        foreach (var element in check)
        {
            result.Add(element.NameProcess);
        }
        return result;
    }

    public async Task<List<string>> GetProcessList(string nameLocation, DateTime lastLaunch)
    {
        var result = new List<string>();
        var check = await _context.ProcessWs.ToListAsync();
        foreach (var element in check)
        {
            if (element.NameLocation == nameLocation && element.LastLaunch == lastLaunch)
                result.Add(element.NameProcess);
        }
        return result ?? throw new ArgumentException("No record in database");
    }
}