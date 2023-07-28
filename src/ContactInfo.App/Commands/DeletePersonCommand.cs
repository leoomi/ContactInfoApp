using MediatR;
using ContactInfo.App.Models;
using System.Security.Claims;

namespace ContactInfo.App.Commands;

public class DeletePersonCommand: IRequest<MediatorResult<bool>>
{
    public int Id { get; set; }
    public ClaimsPrincipal? Claims { get; set; }
}