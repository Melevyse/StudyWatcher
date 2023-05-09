namespace StudyWatcherProject.Contracts;

public interface IAuthorizationUserService
{
    Task<Guid> GetAuthorizationUserResponse(
        string userLogin,
        string userPassword);

    Task<string> GetUserFioResponse(
        string userLogin,
        string userPassword);
    
    Task<string> GetUserGroupResponse(
        string userLogin,
        string userPassword);
}