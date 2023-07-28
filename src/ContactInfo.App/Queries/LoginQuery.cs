using MediatR;

namespace ContactInfo.App.Queries;

public class LoginQuery : IRequest<string>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}