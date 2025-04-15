using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AnimalKingdom.API.DependencyInjection;

internal static class IServiceConfigurationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    )
    {
        services.AddDbContext<AnimalKingdomDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AnimalKingdom"))
        );
        services.Add(
            new ServiceDescriptor(
                typeof(IRepository),
                typeof(AnimalKingdomDbContext),
                ServiceLifetime.Scoped
            )
        );
        services.Add(
            new ServiceDescriptor(
                typeof(IQueryRepository),
                typeof(AnimalKingdomDbContext),
                ServiceLifetime.Scoped
            )
        );
        services.Add(
            new ServiceDescriptor(
                typeof(ICommandRepository),
                typeof(AnimalKingdomDbContext),
                ServiceLifetime.Scoped
            )
        );

        services.Add(
            new ServiceDescriptor(
                typeof(IEntityBaseKeyGenerator<Guid>),
                typeof(GuidKeyGenerator),
                ServiceLifetime.Scoped
            )
        );

        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(Program).Assembly);

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        // NOTE: Sequence is important here to add specific handler implementations before generic
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddEntityHandlers();

        return services;
    }
}
