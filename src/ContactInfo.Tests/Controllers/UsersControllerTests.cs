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

public class UsersControllerTest
{
    private readonly UsersController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IValidator<CreateUserCommand>> _createUserValidatorMock;

    public UsersControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _createUserValidatorMock = new Mock<IValidator<CreateUserCommand>>();
        _controller = new UsersController(
            _mediatorMock.Object,
            _createUserValidatorMock.Object,
            null);
    }

    // TODO User concrete validator later if I have the time
    [Fact]
    public async Task CreateUser_InvalidCommand_ReturnsValidationProblem()
    {
        _createUserValidatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(
                new List<ValidationFailure>
                {
                    new ValidationFailure("password", "Password is too weak!")
                }
            ))
            .Verifiable();
        var command = new CreateUserCommand
        {
            Username = "username",
            Password = "password",
            PasswordConfirmation = "password"
        };
        var result = await _controller.CreateUser(command);

        Assert.IsType<ProblemHttpResult>(result);

        var problemResult = (ProblemHttpResult)result;
        _createUserValidatorMock.Verify();
    }

    [Fact]
    public async Task CreateUser_ValidCommand_ReturnsOk()
    {
        _createUserValidatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();

        var resultUser = new User
        {
            Id = 1,
            Username = "username",
            Password = "password"
        };
        var command = new CreateUserCommand
        {
            Username = resultUser.Username,
            Password = resultUser.Password,
            PasswordConfirmation = resultUser.Password,
        };
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultUser)
            .Verifiable();

        var result = await _controller.CreateUser(command);

        Assert.IsType<Ok<User>>(result);

        var okResult = (Ok<User>)result;
        Assert.Equal(resultUser, okResult.Value);

        _mediatorMock.Verify();
        _createUserValidatorMock.Verify();
    }

    [Fact]
    public async Task Login_ValidCommand_ReturnsToken()
    {
        var query = new LoginQuery
        {
            Username = "username",
            Password = "password",
        };
        var token = "token";
        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(token)
            .Verifiable();

        var result = await _controller.Login(query);

        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult) result.Result;

        Assert.Equal(token, okResult.Value);
        _mediatorMock.Verify();
    }

    [Fact]
    public async Task Login_ValidCommand_ReturnsUnauthorized()
    {
        var query = new LoginQuery
        {
            Username = "username",
            Password = "password",
        };
        _mediatorMock
            .Setup(m => m.Send<string?>(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?) null)
            .Verifiable();

        var result = await _controller.Login(query);

        Assert.IsType<UnauthorizedResult>(result.Result);
        _mediatorMock.Verify();
    }
}