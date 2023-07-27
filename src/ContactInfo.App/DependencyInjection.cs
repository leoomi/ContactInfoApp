using ContactInfo.App.Commands;
using ContactInfo.App.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;

namespace ContactInfo.App;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) 
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(_ =>
        {

            return new ContactInfoContext();
        });
        return services;
    }
}
