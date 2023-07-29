using MediatR;
using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Repositories;
using ContactInfo.App.Services;
using ContactInfo.App.Models;

namespace ContactInfo.App.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public LoginQueryHandler(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public Task<LoginResponse?> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserByUsername(query.Username!);
        if (user == null)
        {
            return Task.FromResult((LoginResponse?) null);
        }

        if (!BC.Verify(query.Password, user!.Password))
        {
            return Task.FromResult((LoginResponse?) null);
        }

        var token = _tokenService.GenerateToken(user);
        return Task.FromResult((LoginResponse?) new LoginResponse
        {
            Token = token,
        });
    }
}