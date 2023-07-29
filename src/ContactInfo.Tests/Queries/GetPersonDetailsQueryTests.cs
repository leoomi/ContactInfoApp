using ContactInfo.App.Models;
using ContactInfo.App.Queries;
using ContactInfo.App.Repositories;

namespace ContactInfo.Tests.Commands;

public class GetPersonDetailsQueryHandlerTests
{
    private readonly GetPersonDetailsQueryHandler _handler;
    private readonly Mock<IPersonRepository> _personRepositoryMock;

    public GetPersonDetailsQueryHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new GetPersonDetailsQueryHandler(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ClaimsAreInvalid_ReturnsUnauthorized()
    {
        var username = "";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);

        var command = new GetPersonDetailsQuery
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

        var command = new GetPersonDetailsQuery
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

        var command = new GetPersonDetailsQuery
        {
            Id = 1,
            Claims = claims,
        };

        _personRepositoryMock
            .Setup(m => m.GetPersonWithContacts(1))
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

        var command = new GetPersonDetailsQuery
        {
            Id = 1,
            Claims = claims,
        };
        var person = new Person
        {
            Id = 1,
            FirstName = "First",
            LastName = "Last",
            Contacts = contacts,
            UserId = userId
        };

        _personRepositoryMock
            .Setup(m => m.GetPersonWithContacts(person.Id))
            .Returns(person)
            .Verifiable();
        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(person, result.Value);
        _personRepositoryMock.Verify();
    }
}