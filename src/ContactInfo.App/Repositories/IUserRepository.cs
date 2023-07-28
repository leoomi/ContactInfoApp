using ContactInfo.App.Models;

namespace ContactInfo.App.Repositories;

public interface IUserRepository
{
    User CreateUser(User user);
    User? GetUserByUsername(string username);
}