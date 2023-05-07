using StudyWatcherProject.Contracts;

namespace StudyWatcherProject.Services;

public class AuthorizationUserService : IAuthorizationUserService
{
    private readonly IAuthorizationUserRepository _repositories;

    public async Task<Guid> GetAuthorizationUserResponse(
        string userLogin,
        string userPassword)
    {
        var result = await _repositories.GetAuthorizationUser(userLogin, userPassword);
        return result.Id;
    }
}