using MediatR;
using ContactInfo.App.Repositories;

namespace ContactInfo.App.Commands;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, bool>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<bool> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
    {
        var result = _personRepository.DeletePerson(command.Id);

        return Task.FromResult(result);
    }
}