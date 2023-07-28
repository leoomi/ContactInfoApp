using System.Security.Claims;
using ContactInfo.App.Commands;
using ContactInfo.App.Controllers;
using ContactInfo.App.Models;
using ContactInfo.App.Queries;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactInfo.Tests.Controllers;

public class PersonsControllerTest
{
    private readonly PersonsController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IValidator<CreateUserCommand>> _createUserValidatorMock;

    public PersonsControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _createUserValidatorMock = new Mock<IValidator<CreateUserCommand>>();
        _controller = new PersonsController(
            _mediatorMock.Object,
            _createUserValidatorMock.Object,
            null);
    }

    [Fact]
    public async Task GetPersonList_NotAuthenticated_ReturnsUnauthorized()
    {
        var result = await _controller.GetPersonList();

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task GetPersonList_Authenticated_ReturnsOkList()
    {
        var username = "username";
        var userId = 1;
        SetupClaims.AddUserInfo(_controller, username, userId);

        var personList = new List<Person>{
            new Person { FirstName = "name1"},
            new Person { FirstName = "name2"},
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPersonListQuery>(), default))
            .ReturnsAsync(personList);

        var result = await _controller.GetPersonList();

        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult) result.Result;
        Assert.Equal(personList, okResult.Value);
    }
}