namespace StudyWatcherProject.Contracts;

public interface IAuthorizationUserService
{
    Task<Guid> GetAuthorizationUserResponse(
        string userLogin,
        string userPassword);
}