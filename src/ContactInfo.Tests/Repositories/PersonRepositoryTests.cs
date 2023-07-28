using ContactInfo.App.Models;
using ContactInfo.App.Repositories;
using ContactInfo.Tests.Setup;
using Microsoft.EntityFrameworkCore;

namespace ContactInfo.Tests.Repositories;

public class PersonRepositoryTests : IDisposable
{
    private readonly PersonRepository _repository;
    private readonly UserRepository _userRepository;
    private readonly ContactInfoContext _context;
    private readonly MemorySqLite _sqLite;

    public PersonRepositoryTests()
    {
        _sqLite = new MemorySqLite();
        _context = _sqLite.GetContext();
        _userRepository = new UserRepository(_context);
        _repository = new PersonRepository(_context);
    }

    public void Dispose()
    {
        _sqLite.Dispose();
    }

    [Fact]
    public void CreatePerson_SinglePerson_AddedAndSaved()
    {
        var user = CreateUser();
        var person = new Person
        {
            FirstName = "First",
            LastName = "Last",
            UserId = user.Id
        };

        var result = _repository.CreatePerson(person);

        Assert.Equal(person, result);
        Assert.Equal(1, result.Id);
        Assert.Equal("First", result.FirstName);
        Assert.Equal("Last", result.LastName);
        Assert.True(_context.People!.Any(u => u.Id == 1));
    }

    [Fact]
    public void CreatePerson_MultiplePeople_AddedAndSaved()
    {
        var user = CreateUser();
        var person1 = new Person
        {
            FirstName = "First1",
            LastName = "Last1",
            UserId = user.Id
        };
        var person2 = new Person
        {
            FirstName = "First2",
            LastName = "Last2",
            UserId = user.Id
        };

        var result1 = _repository.CreatePerson(person1);
        var result2 = _repository.CreatePerson(person2);

        Assert.Equal(person1, result1);
        Assert.Equal(1, result1.Id);
        Assert.Equal("First1", result1.FirstName);
        Assert.Equal("Last1", result1.LastName);

        Assert.Equal(person2, result2);
        Assert.Equal(2, result2.Id);
        Assert.Equal("First2", result2.FirstName);
        Assert.Equal("Last2", result2.LastName);
 
        Assert.Equal(2, _context.People!.Count());
    }

    [Fact]
    public void SavePerson_EditNames()
    {
        var user = CreateUser();
        var person = new Person
        {
            FirstName = "First",
            LastName = "Last",
            UserId = user.Id
        };
        _repository.CreatePerson(person);

        person.FirstName = "First2";
        person.LastName = "Last2";
        var result = _repository.SavePerson(person);

        Assert.Equal(person, result);
        Assert.Equal(1, result.Id);
        Assert.Equal("First2", result.FirstName);
        Assert.Equal("Last2", result.LastName);
        Assert.True(_context.People!.Any(u => u.Id == 1));
    }

    private User CreateUser()
    {
        var user = new User
        {
            Username = "username",
            Password = "password"
        };

        _userRepository.CreateUser(user);
        Assert.NotEqual(0, user.Id);
        Assert.Equal("username", user.Username);
        Assert.Equal("password", user.Password);

        return user;
    }
}