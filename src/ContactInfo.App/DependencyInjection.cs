using ContactInfo.App.Commands;
using ContactInfo.App.Repositories;
using ContactInfo.App.Services;
using FluentValidation;

namespace ContactInfo.App;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) 
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<TokenService, TokenService>();
        services.AddScoped(_ =>
        {

            return new ContactInfoContext();
        });
        return services;
    }
}
