using Microsoft.AspNetCore.SignalR;
using StudyWatcherProject.Contracts;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.Hubs;


public class StudyWatcherHub : Hub
{
    private readonly IMonitoringService _monitoringService;
    private readonly IAuthorizationUserService _authorizationUserService;
    private readonly ILogger<StudyWatcherHub> _logger;

    public StudyWatcherHub(
        IMonitoringService monitoringService,
        IAuthorizationUserService authorizationUserService,
        ILogger<StudyWatcherHub>  logger)
    {
        _monitoringService = monitoringService;
        _authorizationUserService = authorizationUserService;
        _logger = logger;
    }

    // Метод, создание нового компьютера - не готов
    public async Task AddWorkStationHub(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        string nameLocation,
        List<string> listProcess,
        DateTime lastLaunch,
        string connectionIdAdmin,
        string connectionId)
    {
        try
        {
            var id = await _monitoringService
                .AddWorkStationRequest(nameMotherboard, nameCPU, 
                    nameRAM, nameHDD, nameVideocard, nameLocation);
            var result = await _monitoringService.AddProcessListRequest(listProcess, lastLaunch, nameLocation);
            var blackList = await _monitoringService.GetFullBlackList();
            if (id != Guid.Empty)
            {
                await Clients
                    .Client(connectionIdAdmin)
                    .SendAsync("RegisterWorkStation",
                        nameMotherboard, nameCPU, nameRAM, nameHDD,
                        nameVideocard, nameLocation, connectionId);
                await Clients
                    .Client(connectionId)
                    .SendAsync("ResponseBlackList", blackList);
                await Clients
                    .Client(connectionIdAdmin)
                    .SendAsync("ResponseBlackList", blackList);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "AddWorkStationHub encountered an exception.");
            throw;
        }
    }

    // Метод, авторизации пользователя - готов
    public async Task<bool> GetAuthorizationUserHub(
        string userLogin,
        string userPassword,
        string connectionIdAdmin)
    {
        var connectionId = Context.ConnectionId;
        try
        {
            var result = await _authorizationUserService
                .GetAuthorizationUserResponse(userLogin, userPassword);
            if (result != Guid.Empty)
            {
                var resultFio = await _authorizationUserService
                    .GetUserFioResponse(userLogin, userPassword);
                var resultGroup = await _authorizationUserService
                    .GetUserGroupResponse(userLogin, userPassword);
                // Ответ пользователю об успешной авторизации
                await Clients
                    .Client(connectionId)
                    .SendAsync("CloseStartBanner", resultFio, resultGroup);
                // Ответ администратору
                await Clients
                    .Client(connectionIdAdmin)
                    .SendAsync("AddItemUser", resultFio, resultGroup, connectionId);
                return true;
            }
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetAuthorizationUserHub encountered an exception.");
            return false;
            throw;
        }
    }

    // Метод, для сохранения идентификатора подключения администратора - готов
    public async Task GetAdminConnectionIdHub()
    {
        try
        {
            var connectionId = Context.ConnectionId;
            await Clients.All
                .SendAsync("AdminConnectionComplete", connectionId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetAdminConnectionIdHub encountered an exception.");
            throw;
        }
    }

    public async Task GetProcessListHub( 
        string nameLocation, 
        DateTime lastLaunch, 
        string connectionIdAdmin)
    {
        try
        {
            var result = await _monitoringService.GetFullProcessList(nameLocation, lastLaunch);
            await Clients.Client(connectionIdAdmin).SendAsync("GetProcessList", result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetProcessListHub encountered an exception.");
            throw;
        }
    }

    public async Task GetProcessBanListHub(
        string connectionId)
    {
        try
        {
            var result = await _monitoringService.GetFullBlackList();
            await Clients
                .Client(connectionId)
                .SendAsync("ResponseBlackList", result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetProcessBanListHub encountered an exception.");
            throw;
        }
    }

    // Метод, сообщение об использовании запрещенного ПО - готов
    public async Task GetBannerHub(
        string processBan,
        string connectionId,
        string connectionIdAdmin)
    {
        try
        {
            var result = await _monitoringService
                .GetBannerResponse(processBan);
            if (result != Guid.Empty)
            {
                // Ответ пользователю
                await Clients
                    .Client(connectionId)
                    .SendAsync("OpenBlackListBanner", processBan);
                // Ответ администатору
                await Clients.Client(connectionIdAdmin)
                    .SendAsync("UserUsedBlackListProcess", processBan, connectionId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetBannerHub encountered an exception.");
            throw;
        }
    }
    
    // Метод, который убирает баннер у пользователя - готов
    public async Task BannerCloseHub(string connectionId)
    {
        await Clients
            .Client(connectionId)
            .SendAsync("CloseBlackListBanner");
    }


    // Метод, который добаляет новый запрещенный процесс - не готов
    public async Task AddProcessListBanHub(
        string processBan,
        string connectionIdAdmin)
    {
        try
        {
            var result = await _monitoringService
                .AddProcessBanRequest(processBan);
            if (result != Guid.Empty) 
                await Clients
                    .All
                    .SendAsync("AddProcessBlackList", processBan);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "AddProcessListBanHub encountered an exception.");
            throw;
        }
    }

    public async Task RemoveProcessListBanHub(
        string processBan)
    {
        try
        {
            var result = await _monitoringService
                .RemoveProcessBanRequest(processBan);
            if (result != Guid.Empty) 
                await Clients
                    .All
                    .SendAsync("RemoveProcessBlackList", processBan);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "AddProcessListBanHub encountered an exception.");
            throw;
        }
    }

    public async Task GetFullProcessWsHub(
        string connectionId)
    {
        try
        {
            var result = await _monitoringService.GetFullProcessWs();
            await Clients.Client(connectionId).SendAsync("AnovaMethod", result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CancelSendPictureHub encountered an exception.");
            throw;
        }
    }


    public async Task AddProcessListHub(
        string nameLocation,
        List<string> listProcess,
        DateTime lastLaunch,
        string connectionIdAdmin,
        string connectionId)
    {
        try
        { 
            var result = await _monitoringService.AddProcessListRequest(listProcess, lastLaunch, nameLocation);
            await Clients.Client(connectionIdAdmin).SendAsync("GetProcessListUpdate", result, connectionId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "AddProcessListHub encountered an exception.");
            throw;
        }
    }



    public async Task RequestPictureHub(
        string connectionId)
    {
        try
        { 
            await Clients
                .Client(connectionId)
                .SendAsync("RequestPicture");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "RequestPictureHub encountered an exception.");
            throw;
        }
    }

    public async Task SendPictureHub(
        string imageData,
        string connectionIdAdmin)
    {
        try
        { 
            await Clients
                .Client(connectionIdAdmin)
                .SendAsync("SendPicture", imageData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SendPictureHub encountered an exception.");
            throw;
        }
    }

    public async Task CancelSendPictureHub(
        string connectionId)
    {
        try
        {
            await Clients
                .Client(connectionId)
                .SendAsync("CancelSendPicture");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CancelSendPictureHub encountered an exception.");
            throw;
        }
    }
}