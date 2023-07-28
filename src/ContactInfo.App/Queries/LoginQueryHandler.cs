using MediatR;
using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Repositories;
using ContactInfo.App.Services;

namespace ContactInfo.App.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, string?>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public LoginQueryHandler(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public Task<string?> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserByUsername(query.Username!);
        if (user == null)
        {
            return Task.FromResult((string?) null);
        }

        if (!BC.Verify(query.Password, user!.Password))
        {
            return Task.FromResult((string?) null);
        }

        var token = _tokenService.GenerateToken(user);
        return Task.FromResult((string?) token);
    }
}