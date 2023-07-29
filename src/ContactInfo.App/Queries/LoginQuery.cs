using ContactInfo.App.Models;
using MediatR;

namespace ContactInfo.App.Queries;

public class LoginQuery : IRequest<LoginResponse?>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}