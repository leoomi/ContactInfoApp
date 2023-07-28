using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentValidation;
using ContactInfo.App.Commands;
using Microsoft.AspNetCore.Authorization;
using ContactInfo.App.Queries;

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
    [AllowAnonymous]
    public async Task<IResult> CreateUser(CreateUserCommand command)
    {
        var validation = await _createUserValidator.ValidateAsync(command);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        var result = await _mediator.Send(command);
        result.Password = null;
        return Results.Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login/")]
    public async Task<ActionResult<string?>> Login(LoginQuery query)
    {
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }
}
