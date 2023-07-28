using ContactInfo.App.Commands;
using ContactInfo.App.Models;
using ContactInfo.App.Repositories;

namespace ContactInfo.Tests.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateUserCommandHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CallRepositoryCreateUserAndReturnsUser()
    {
        var command = new CreateUserCommand
        {
            Username = "username",
            Password = "password",
            PasswordConfirmation = "password",
        };
        var user = new User
        {
            Id = 1,
            Username = command.Username,
            Password = command.Password,
        };
        _userRepositoryMock.Setup(m => m
            .CreateUser(It.Is<User>(u =>
                u.Username == command.Username
            )))
            .Returns(user)
            .Verifiable();
        var result = await _handler.Handle(command, default);

        Assert.Equal(user, result);
        _userRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_SamePassword_ReturnsDifferentHash()
    {
        var hashedPasswords = new List<string>();
        var command = new CreateUserCommand
        {
            Username = "username",
            Password = "password",
            PasswordConfirmation = "password",
        };
        _userRepositoryMock.Setup(m => m
            .CreateUser(It.IsAny<User>()))
            .Callback<User>(u => hashedPasswords.Add(u.Password!));
        await _handler.Handle(command, default);
        await _handler.Handle(command, default);
        await _handler.Handle(command, default);
        await _handler.Handle(command, default);
        await _handler.Handle(command, default);

        Assert.Distinct(hashedPasswords);
    }
}