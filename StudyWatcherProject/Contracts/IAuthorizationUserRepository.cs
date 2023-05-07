using StudyWatcherProject.Models;

namespace StudyWatcherProject.Contracts;

public interface IAuthorizationUserRepository
{
    Task<UserStudent> GetAuthorizationUser(
        string userLogin,
        string userPassword);
}