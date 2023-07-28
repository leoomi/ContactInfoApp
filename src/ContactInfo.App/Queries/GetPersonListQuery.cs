using ContactInfo.App.Models;
using MediatR;

namespace ContactInfo.App.Queries;

public class GetPersonListQuery : IRequest<IList<Person>>
{
    public string? Username { get; set; }
}