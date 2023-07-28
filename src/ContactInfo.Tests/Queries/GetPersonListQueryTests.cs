using ContactInfo.App.Models;
using ContactInfo.App.Queries;
using ContactInfo.App.Repositories;

namespace ContactInfo.Tests.Queries;

public class GetPersonListQueryHandlerTests
{
    private readonly GetPersonListQueryHandler _handler;
    private readonly Mock<IPersonRepository> _personRepositoryMock;

    public GetPersonListQueryHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new GetPersonListQueryHandler(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList()
    {
        var username = "username";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);
        var query = new GetPersonListQuery
        {
            Claims = claims
        };
        _personRepositoryMock
            .Setup(m => m.GetPersonList(userId))
            .Returns(new List<Person>())
            .Verifiable();

        var result = await _handler.Handle(query, default);

        Assert.Empty(result.Value!);
        _personRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_ReturnsList()
    {
        var username = "username";
        var userId = 1;
        var claims = SetupClaims.CreateClaims(username, userId);
        var query = new GetPersonListQuery
        {
            Claims = claims
        };
        var personList = new List<Person>
        {
            new Person{ FirstName = "Name 1" },
            new Person{ FirstName = "Name 2" },
        };

        _personRepositoryMock
            .Setup(m => m.GetPersonList(userId))
            .Returns(personList)
            .Verifiable();

        var result = await _handler.Handle(query, default);

        Assert.Equal(personList, result.Value);
        _personRepositoryMock.Verify();
    }
}