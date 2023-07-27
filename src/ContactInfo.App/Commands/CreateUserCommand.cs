using MediatR;
using ContactInfo.App.Models;

namespace ContactInfo.App.Commands;

public class CreateUserCommand: IRequest<User>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirmation { get; set; }
}