using Microsoft.EntityFrameworkCore;
using StudyWatcherProject.Contracts;
using StudyWatcherProject.EFC;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.Repositories;

public class AuthorizationUserRepository : IAuthorizationUserRepository
{
    private readonly SqlReportingContext _context;
    private readonly Logger<AuthorizationUserRepository> _logger;
    public async Task<UserStudent> GetAuthorizationUser(
        string userLogin,
        string userPassword)
    {
        var result = await _context.UserStudent
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.UserLogin == userLogin &&
                x.UserPassword == userPassword);
        return result ?? throw new ArgumentException("Request is not found in the database");
    }
}