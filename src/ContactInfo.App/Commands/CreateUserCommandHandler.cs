using MediatR;
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
        var createdUser = _userRepository.CreateUser(new User{
            Username = command.Username,
            Password = command.Password
        });

        return Task.FromResult(createdUser);
    }
}