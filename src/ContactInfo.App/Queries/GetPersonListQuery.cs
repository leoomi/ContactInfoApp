using System.Security.Claims;
using ContactInfo.App.Models;
using MediatR;

namespace ContactInfo.App.Queries;

public class GetPersonListQuery : IRequest<MediatorResult<IList<Person>>>
{
    public ClaimsPrincipal? Claims { get; set; }
}