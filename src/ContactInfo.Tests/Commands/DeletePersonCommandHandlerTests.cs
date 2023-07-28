using ContactInfo.App.Commands;
using ContactInfo.App.Models;
using ContactInfo.App.Repositories;

namespace ContactInfo.Tests.Commands;

public class DeletePersonCommandHandlerTests
{
    private readonly DeletePersonCommandHandler _handler;
    private readonly Mock<IPersonRepository> _personRepositoryMock;

    public DeletePersonCommandHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new DeletePersonCommandHandler(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ClaimsAreInvalid_ReturnsUnauthorized()
    {
        var username = "";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);

        var command = new DeletePersonCommand
        {
            Id = 1,
            Claims = claims,
        };

        var result = await _handler.Handle(command, default);

        Assert.False(result.IsSuccess);
        Assert.Equal(MediatorError.Unauthorized, result.Error);
    }

    [Fact]
    public async Task Handle_DifferentUserdId_ReturnsUnauthorized()
    {
        var username = "";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);

        var command = new DeletePersonCommand
        {
            Id = 1,
            Claims = claims,
        };

        var result = await _handler.Handle(command, default);

        Assert.False(result.IsSuccess);
        Assert.Equal(MediatorError.Unauthorized, result.Error);
        _personRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_NullPerson_ReturnsNotFound()
    {
        var username = "username";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);

        var command = new DeletePersonCommand
        {
            Id = 1,
            Claims = claims,
        };

        _personRepositoryMock
            .Setup(m => m.GetPerson(1))
            .Returns((Person) null)
            .Verifiable();

        var result = await _handler.Handle(command, default);

        Assert.False(result.IsSuccess);
        Assert.Equal(MediatorError.NotFound, result.Error);
        _personRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_ValidClaims_ReturnsSuccesfulResult()
    {
        var username = "username";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);

        var contacts = new List<Contact>
        {
            new Contact { Info = "111", Type = ContactType.Phone },
        };

        var command = new DeletePersonCommand
        {
            Id = 1,
            Claims = claims,
        };
        var person = new Person
        {
            Id = 1,
            Contacts = contacts,
            UserId = userId
        };

        _personRepositoryMock
            .Setup(m => m.GetPerson(person.Id))
            .Returns(person)
            .Verifiable();
        _personRepositoryMock
            .Setup(m => m.DeletePerson(1))
            .Returns(true)
            .Verifiable();
        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(true, result.Value);
        _personRepositoryMock.Verify();
    }
}