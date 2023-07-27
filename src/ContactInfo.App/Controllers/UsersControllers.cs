using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentValidation;
using ContactInfo.App.Models;
using ContactInfo.App.Commands;

namespace ContactInfo.App.Controllers;

[ApiController]
[Route("/api/users/")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateUserCommand> _createUserValidator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IMediator mediator,
        IValidator<CreateUserCommand> createUserValidator,
        ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _createUserValidator = createUserValidator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IResult> CreateUser(CreateUserCommand command)
    {
        var validation = await _createUserValidator.ValidateAsync(command);
        if (!validation.IsValid) {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        var result = await _mediator.Send(command);
        return Results.Ok(result);
    }
}
