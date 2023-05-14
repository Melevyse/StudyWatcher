using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StudyWatcherProject.Contracts;
using StudyWatcherProject.EFC;
using StudyWatcherProject.Models;
using StudyWatcherProject.Repositories;
using StudyWatcherProject.Services;
using StudyWatcherProject.Validators;

namespace StudyWatcherProject;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection service)
    {
        service.AddDbContext<SqlReportingContext>(options =>
            options.UseNpgsql(_configuration.GetConnectionString("SqlReportingContext")));
        service.AddScoped<IAuthorizationUserService, AuthorizationUserService>();
        service.AddScoped<IAuthorizationUserRepository, AuthorizationUserRepository>();
        service.AddScoped<IMonitoringService, MonitoringService>();
        service.AddScoped<IMonitoringRepository, MonitoringRepository>();
        service.AddSignalR();

        service.AddTransient<IValidator<UserStudent>,UserAuthorizationValidator>();
    }
    
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        SqlReportingContext sqlReportingContext,
        ILogger<Startup> logger)
    {
        
    }
}