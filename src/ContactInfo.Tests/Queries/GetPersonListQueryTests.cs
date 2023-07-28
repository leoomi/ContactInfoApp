using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Models;
using ContactInfo.App.Queries;
using ContactInfo.App.Repositories;
using ContactInfo.App.Services;
using System.IdentityModel.Tokens.Jwt;

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
        var query = new GetPersonListQuery
        {
            Username = "username",
        };
        _personRepositoryMock
            .Setup(m => m.GetPersonList(query.Username))
            .Returns(new List<Person>())
            .Verifiable();

        var result = await _handler.Handle(query, default);

        Assert.Empty(result);
        _personRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_ReturnsList()
    {
        var query = new GetPersonListQuery
        {
            Username = "username",
        };
        var personList = new List<Person>
        {
            new Person{ FirstName = "Name 1" },
            new Person{ FirstName = "Name 2" },
        };
        _personRepositoryMock
            .Setup(m => m.GetPersonList(query.Username))
            .Returns(personList)
            .Verifiable();

        var result = await _handler.Handle(query, default);

        Assert.Equal(personList, result);
        _personRepositoryMock.Verify();
    }
}