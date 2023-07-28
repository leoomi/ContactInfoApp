using MediatR;
using ContactInfo.App.Repositories;
using ContactInfo.App.Models;

namespace ContactInfo.App.Queries;

public class GetPersonListQueryHandler : IRequestHandler<GetPersonListQuery, MediatorResult<IList<Person>>>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonListQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<MediatorResult<IList<Person>>> Handle(GetPersonListQuery query, CancellationToken cancellationToken)
    {
        var username = query?.Claims?.Identity?.Name;
        var userId = query?.Claims?.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
        {
            return Task.FromResult(new MediatorResult<IList<Person>>(MediatorError.Unauthorized));
        }

        var result = _personRepository.GetPersonList(int.Parse(userId));
        return Task.FromResult(new MediatorResult<IList<Person>>(result));
    }
}