using MediatR;
using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Repositories;
using ContactInfo.App.Services;
using ContactInfo.App.Models;

namespace ContactInfo.App.Queries;

public class GetPersonListQueryHandler: IRequestHandler<GetPersonListQuery, IList<Person>>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonListQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<IList<Person>> Handle(GetPersonListQuery query, CancellationToken cancellationToken)
    {
        var result = _personRepository.GetPersonList(query.UserId);
        return Task.FromResult(result);
    }
}