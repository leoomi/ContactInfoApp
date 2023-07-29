using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Models;
using ContactInfo.App.Queries;
using ContactInfo.App.Repositories;
using ContactInfo.App.Services;

namespace ContactInfo.Tests.Queries;

public class LoginQueryHandlerTests
{
    private readonly LoginQueryHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly TokenService _tokenService;

    public LoginQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenService = new TokenService();
        _handler = new LoginQueryHandler(_userRepositoryMock.Object, _tokenService);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsJWTToken()
    {
        var username = "username";
        var password = "password";
        var hashedPassword = BC.HashPassword(password);
        var user = new User
        {
            Username = username,
            Password = hashedPassword,
        };
        _userRepositoryMock
            .Setup(m => m.GetUserByUsername(username))
            .Returns(user).Verifiable();

        var query = new LoginQuery
        {
            Username = username,
            Password = password,
        };

        var result = await _handler.Handle(query, default);
        Assert.NotNull(result);

        var name = _tokenService.GetClaimsName(result.Token);

        Assert.Equal(username, name);
        _userRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_NoUser_ReturnsNull()
    {
        _userRepositoryMock
            .Setup(m => m.GetUserByUsername(It.IsAny<string>()))
            .Returns((User?) null).Verifiable();

        var query = new LoginQuery
        {
            Username = "username",
            Password = "password",
        };
        var result = await _handler.Handle(query, default);
        Assert.Null(result);
        _userRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_InvalidPassword_ReturnsNull()
    {
        var username = "username";
        var password = "password";
        var hashedPassword = BC.HashPassword("anotherPassword");
        var user = new User
        {
            Username = username,
            Password = hashedPassword,
        };
        _userRepositoryMock
            .Setup(m => m.GetUserByUsername(username))
            .Returns(user).Verifiable();

        var query = new LoginQuery
        {
            Username = username,
            Password = password,
        };

        var result = await _handler.Handle(query, default);
        Assert.Null(result);
        _userRepositoryMock.Verify();
    }
}