using MediatR;
using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Models;
using ContactInfo.App.Repositories;

namespace ContactInfo.App.Commands;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, MediatorResult<Person>>
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<MediatorResult<Person>> Handle(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var username = command?.Claims?.Identity?.Name;
        var sub = command?.Claims?.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(sub))
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.Unauthorized));
        }

        var userId = int.Parse(sub);
        var createdPerson = _personRepository.CreatePerson(new Person
        {
            FirstName = command!.FirstName,
            LastName = command.LastName,
            UserId = userId,
            Contacts = command.Contacts
        });

        return Task.FromResult(new MediatorResult<Person>(createdPerson));
    }
}