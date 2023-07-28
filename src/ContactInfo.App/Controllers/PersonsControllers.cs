using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentValidation;
using ContactInfo.App.Commands;
using Microsoft.AspNetCore.Authorization;
using ContactInfo.App.Queries;
using ContactInfo.App.Models;

namespace ContactInfo.App.Controllers;

[ApiController]
[Route("/api/persons/")]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateUserCommand> _createUserValidator;
    private readonly ILogger<PersonsController> _logger;

    public PersonsController(
        IMediator mediator,
        IValidator<CreateUserCommand> createUserValidator,
        ILogger<PersonsController> logger)
    {
        _mediator = mediator;
        _createUserValidator = createUserValidator;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IList<Person>>> GetPersonList()
    {
        var result = await _mediator.Send(new GetPersonListQuery
        {
            Claims = User
        });

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Person>> CreatePerson(CreatePersonCommand command)
    {
        command.Claims = User;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ReturnMediatorResultError();
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize]
    [Route("{id}")]
    public async Task<ActionResult<Person>> SavePerson(SavePersonCommand command, int id)
    {
        if (command.Id != id)
        {
            return BadRequest();
        }

        command.Claims = User;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ReturnMediatorResultError();
        }

        return Ok(result.Value);
    }
}
