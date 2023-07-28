using MediatR;
using BC = BCrypt.Net.BCrypt;
using ContactInfo.App.Models;
using ContactInfo.App.Repositories;

namespace ContactInfo.App.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var hashedPassword = BC.HashPassword(command.Password);
        var createdUser = _userRepository.CreateUser(new User{
            Username = command.Username,
            Password = hashedPassword
        });

        return Task.FromResult(createdUser);
    }
}