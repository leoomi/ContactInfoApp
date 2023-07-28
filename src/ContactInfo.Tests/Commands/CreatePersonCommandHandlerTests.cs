using ContactInfo.App.Commands;
using ContactInfo.App.Models;
using ContactInfo.App.Repositories;

namespace ContactInfo.Tests.Commands;

public class CreatePersonCommandHandlerTests
{
    private readonly CreatePersonCommandHandler _handler;
    private readonly Mock<IPersonRepository> _personRepositoryMock;

    public CreatePersonCommandHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new CreatePersonCommandHandler(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ClaimsAreInvalid_ReturnsUnauthorized()
    {
        var username = "";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);

        var command = new CreatePersonCommand
        {
            FirstName = "First",
            LastName = "Last",
            Contacts = new List<Contact>
            {
                new Contact { Info = "111", Type = ContactType.Phone },
            },
            Claims = claims,
        };

        var result = await _handler.Handle(command, default);

        Assert.False(result.IsSuccess);
        Assert.Equal(MediatorError.Unauthorized, result.Error);
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

        var command = new CreatePersonCommand
        {
            FirstName = "First",
            LastName = "Last",
            Contacts = contacts,
            Claims = claims,
        };
        var person = new Person
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Contacts = contacts,
            UserId = userId
        };

        _personRepositoryMock
            .Setup(m => m.CreatePerson(It.IsAny<Person>()))
            .Returns(person)
            .Verifiable();
        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(person, result.Value);
        _personRepositoryMock.Verify();
    }
}