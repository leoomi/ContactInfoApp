using ContactInfo.App.Models;
using MediatR;

namespace ContactInfo.App.Queries;

public class GetPersonListQuery : IRequest<IList<Person>>
{
    public int UserId { get; set; }
}