using ContactInfo.App.Models;
using ContactInfo.App.Repositories;
using ContactInfo.Tests.Setup;
using Microsoft.EntityFrameworkCore;

namespace ContactInfo.Tests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly UserRepository _repository;
    private readonly ContactInfoContext _context;
    private readonly MemorySqLite _sqLite;

    public UserRepositoryTests()
    {
        _sqLite = new MemorySqLite();
        _context = _sqLite.GetContext();
        _repository = new UserRepository(_context);
    }

    public void Dispose()
    {
        _sqLite.Dispose();
    }

    [Fact]
    public void CreateUser_SingleUser_AddedAndSaved()
    {
        var user = new User
        {
            Username = "username",
            Password = "password",
        };

        var result = _repository.CreateUser(user);

        Assert.Equal(user, result);
        Assert.Equal(1, result.Id);
        Assert.Equal("username", result.Username);
        Assert.Equal("password", result.Password);
        Assert.True(_context.Users!.Any(u => u.Id == 1));
    }

    [Fact]
    public void CreateUser_RepeatedUsername_ShouldThrowException()
    {
        var user = new User
        {
            Username = "username",
            Password = "password",
        };
        _repository.CreateUser(user);
        user = new User
        {
            Username = "username",
            Password = "password2",
        };

        Assert.Throws<DbUpdateException>(() => _repository.CreateUser(user));
    }

    [Fact]
    public void CreateUser_MultipleUsers_AddedAndSaved()
    {
        var user1 = new User
        {
            Username = "username",
            Password = "password",
        };
        var user2 = new User
        {
            Username = "username2",
            Password = "password",
        };

        _repository.CreateUser(user1);
        _repository.CreateUser(user2);

        Assert.True(_context.Users!.Any(u => u.Id == user1.Id));
        Assert.True(_context.Users!.Any(u => u.Id == user2.Id));
        Assert.Equal(2, _context.Users!.Count());
    }

    [Fact]
    public void Login_ExistingUser_ReturnsUser()
    {
        var user = new User
        {
            Username = "username",
            Password = "password",
        };
        _repository.CreateUser(user);

        var result = _repository.Login(user.Username, user.Password);
        Assert.Equal(user, result);
    }

    [Fact]
    public void Login_NonExistingUser_ReturnsNull()
    {
        var user = new User
        {
            Username = "username",
            Password = "password",
        };
        _repository.CreateUser(user);

        var result = _repository.Login("username2", "password");
        Assert.Null(result);
    }
}