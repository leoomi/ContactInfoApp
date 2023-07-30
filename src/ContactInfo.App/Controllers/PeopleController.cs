using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentValidation;
using ContactInfo.App.Commands;
using Microsoft.AspNetCore.Authorization;
using ContactInfo.App.Queries;
using ContactInfo.App.Models;

namespace ContactInfo.App.Controllers;

[ApiController]
[Route("/api/people/")]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateUserCommand> _createUserValidator;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(
        IMediator mediator,
        IValidator<CreateUserCommand> createUserValidator,
        ILogger<PeopleController> logger)
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

        if (!result.IsSuccess)
        {
            return result.ReturnMediatorResultError();
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<ActionResult<IList<Person>>> GetPersonDetails(int id)
    {
        var result = await _mediator.Send(new GetPersonDetailsQuery
        {
            Id = id,
            Claims = User
        });

        if (!result.IsSuccess)
        {
            return result.ReturnMediatorResultError();
        }

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
        command.Id = id;
        command.Claims = User;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ReturnMediatorResultError();
        }

        return Ok(result.Value);
    }

    [HttpDelete]
    [Authorize]
    [Route("{id}")]
    public async Task<ActionResult<Person>> DeletePerson(int id)
    {
        var result = await _mediator.Send(new DeletePersonCommand
        {
            Id = id,
            Claims = User,
        });

        if (!result.IsSuccess)
        {
            return result.ReturnMediatorResultError();
        }

        return Ok(result.Value);
    }
}
