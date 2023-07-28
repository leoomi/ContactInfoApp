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
        if (User?.Identity?.Name == null)
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new GetPersonListQuery
        {
            Username = User.Identity.Name,
        });
        return Ok(result);
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
