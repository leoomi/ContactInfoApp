using MediatR;
using ContactInfo.App.Repositories;
using ContactInfo.App.Models;
using System.Security.Claims;

namespace ContactInfo.App.Queries;

public class GetPersonDetailsQueryHandler : IRequestHandler<GetPersonDetailsQuery, MediatorResult<Person>>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonDetailsQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<MediatorResult<Person>> Handle(GetPersonDetailsQuery query, CancellationToken cancellationToken)
    {
        var username = query?.Claims?.Identity?.Name;
        var sub = query?.Claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(sub))
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.Unauthorized));
        }

        var person = _personRepository.GetPersonWithContacts(query!.Id);
        if (person == null)
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.NotFound));
        }

        var userId = int.Parse(sub);

        if (person.UserId != userId)
        {
            return Task.FromResult(new MediatorResult<Person>(MediatorError.Unauthorized));
        }
        return Task.FromResult(new MediatorResult<Person>(person));
    }
}