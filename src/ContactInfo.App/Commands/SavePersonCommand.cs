using MediatR;
using ContactInfo.App.Models;
using System.Security.Claims;

namespace ContactInfo.App.Commands;

public class SavePersonCommand: IRequest<MediatorResult<Person>>
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Contact>? Contacts { get; set; }
    public int? UserId { get; set; }
    public ClaimsPrincipal? Claims { get; set; }
}