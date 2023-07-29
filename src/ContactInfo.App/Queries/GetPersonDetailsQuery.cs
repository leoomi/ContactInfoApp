using System.Security.Claims;
using ContactInfo.App.Models;
using MediatR;

namespace ContactInfo.App.Queries;

public class GetPersonDetailsQuery : IRequest<MediatorResult<Person>>
{
    public int Id { get; set; }
    public ClaimsPrincipal? Claims { get; set; }
}