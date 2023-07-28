using MediatR;
using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Models;
using ContactInfo.App.Repositories;

namespace ContactInfo.App.Commands;

public class SavePersonCommandHandler : IRequestHandler<SavePersonCommand, MediatorResult<Person>>
{
    private readonly IPersonRepository _personRepository;

    public SavePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<MediatorResult<Person>> Handle(SavePersonCommand command, CancellationToken cancellationToken)
    {
        var username = command?.Claims?.Identity?.Name;
        var sub = command?.Claims?.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(sub))
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.Unauthorized));
        }

        var person = _personRepository.GetPerson(command!.Id!.Value);
        if (person == null)
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.NotFound));
        }

        var userId = int.Parse(sub);
        if (person.UserId != userId)
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.Unauthorized));
        }

        var savedPerson = _personRepository.SavePerson(new Person{
            Id = command.Id.Value,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Contacts = command.Contacts,
            UserId = userId,
        });
        return Task.FromResult(new MediatorResult<Person>(savedPerson));
    }
}