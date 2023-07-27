using Microsoft.EntityFrameworkCore;
using ContactInfo.App.Models;

namespace ContactInfo.App.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ContactInfoContext context;

    public UserRepository(ContactInfoContext context)
    {
        this.context = context;
    }

    public User CreateUser(User user)
    {
        context.Add(user);
        context.SaveChanges();
        return user;
    }

    public User? Login(string username, string password)
    {
        return context.Users!
            .FirstOrDefault(u => u.Username == username &&
                u.Password == password);
    }
}