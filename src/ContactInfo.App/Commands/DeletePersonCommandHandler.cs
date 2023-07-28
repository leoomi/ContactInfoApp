using MediatR;
using ContactInfo.App.Repositories;

namespace ContactInfo.App.Commands;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, MediatorResult<bool>>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<MediatorResult<bool>> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
    {
        var username = command?.Claims?.Identity?.Name;
        var sub = command?.Claims?.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(sub))
        {
            return Task.FromResult(new MediatorResult<bool>(MediatorError.Unauthorized));
        }

        var person = _personRepository.GetPerson(command!.Id);
        if (person == null)
        {
            return Task.FromResult(new MediatorResult<bool>(MediatorError.NotFound));
        }

        var userId = int.Parse(sub);
        if (person.UserId != userId)
        {
            return Task.FromResult(new MediatorResult<bool>(MediatorError.Unauthorized));
        }

        var result = _personRepository.DeletePerson(command.Id);

        return Task.FromResult(new MediatorResult<bool>(result));
    }
}