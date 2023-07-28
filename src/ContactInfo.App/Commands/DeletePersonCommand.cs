using MediatR;
using ContactInfo.App.Models;

namespace ContactInfo.App.Commands;

public class DeletePersonCommand: IRequest<bool>
{
    public int Id { get; set; }
}