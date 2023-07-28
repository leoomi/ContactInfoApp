using MediatR;
using ContactInfo.App.Models;
using System.Security.Claims;

namespace ContactInfo.App.Commands;

public class CreatePersonCommand: IRequest<MediatorResult<Person>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Contact>? Contacts { get; set; }
    public ClaimsPrincipal? Claims { get; set; }
}