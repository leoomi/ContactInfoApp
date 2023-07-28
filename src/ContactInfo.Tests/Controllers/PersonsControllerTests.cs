using System.Security.Claims;
using ContactInfo.App.Commands;
using ContactInfo.App.Controllers;
using ContactInfo.App.Models;
using ContactInfo.App.Queries;
using FluentValidation;
using MediatR;
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
    public async Task GetPersonList_Authenticated_ReturnsOkList()
    {
        var username = "username";
        var userId = 1;
        SetupClaims.AddUserInfo(_controller, username, userId);

        var personList = new List<Person>{
            new Person { FirstName = "name1"},
            new Person { FirstName = "name2"},
        };
        var mediatorResult = new MediatorResult<IList<Person>>(personList);
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPersonListQuery>(), default))
            .ReturnsAsync(mediatorResult)
            .Verifiable();

        var result = await _controller.GetPersonList();

        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult) result.Result;
        Assert.Equal(mediatorResult, okResult.Value);
        _mediatorMock.Verify();
    }

    [Fact]
    public async Task SavePerson_MediatorError_ReturnsError()
    {
        var username = "username";
        var userId = 1;
        SetupClaims.AddUserInfo(_controller, username, userId);

        var mediatorResult = new MediatorResult<Person>(MediatorError.Unauthorized);
        var command = new SavePersonCommand
        {
            Id = 1,
            FirstName = "First",
        };
        _mediatorMock
            .Setup(m => m
                .Send(It.Is<SavePersonCommand>(c =>
                    c.Claims!.Identity!.Name == username &&
                    c.Claims!.FindFirst("sub")!.Value == userId.ToString()
                ), default))
            .ReturnsAsync(mediatorResult)
            .Verifiable();

        var result = await _controller.SavePerson(command);

        Assert.IsType<UnauthorizedResult>(result.Result);
        _mediatorMock.Verify();
    }

    [Fact]
    public async Task SavePerson_SuccesfulCall_ReturnsOkPerson()
    {
        var username = "username";
        var userId = 1;
        SetupClaims.AddUserInfo(_controller, username, userId);

        var person = new Person
        {
            Id = 1,
            UserId = userId,
            FirstName = "First"
        };
        var mediatorResult = new MediatorResult<Person>(person);
        var command = new SavePersonCommand
        {
            Id = 1,
            UserId = userId,
            FirstName = "First",
        };

        _mediatorMock
            .Setup(m => m
                .Send(It.Is<SavePersonCommand>(c =>
                    c.Claims!.Identity!.Name == username &&
                    c.Claims!.FindFirst("sub")!.Value == userId.ToString()
                ), default))
            .ReturnsAsync(mediatorResult)
            .Verifiable();

        var result = await _controller.SavePerson(command);

        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult) result.Result;

        Assert.Equal(person, okResult.Value);
        _mediatorMock.Verify();
    }
}