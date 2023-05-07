/*
using Microsoft.AspNetCore.Mvc;
using StudyWatcherProject.Contracts;

namespace StudyWatcherProject.Controllers;

public class StudyWatcherController : Controller
{
    private readonly IMonitoringService _monitoringService;
    private readonly IAuthorizationUserService _authorizationUserService;
    private readonly ILogger<StudyWatcherController> _logger;

    public StudyWatcherController(
        IMonitoringService monitoringService,
        IAuthorizationUserService authorizationService,
        ILogger<StudyWatcherController> logger)
    {
        monitoringService = _monitoringService;
        authorizationService = _authorizationUserService;
        logger = _logger;
    }

    public async Task<ActionResult<string>> AddNewWorkStationRequest(
        string name,
        List<string> processList,
        List<string> processListBan)
    {
        try
        {
            var Id = await _monitoringService.AddWorkStationRequest(name, processList, processListBan);

            if (Id != Guid.Empty)
                return Created(string.Empty, Id);

            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "AddNewWorkStationRequest encountered an exception.");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    public async Task<ActionResult<string>> GetNewAuthorizationUserResponse(
        string userLogin,
        string userPassword)
    {
        try
        {
            var result = await _authorizationUserService.GetAuthorizationUserResponse(userLogin, userPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetNewAuthorizationUserResponse encountered an exception.");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
*/