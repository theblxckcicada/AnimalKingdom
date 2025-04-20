using AnimalKingdom.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace AnimalKingdom.API.DependencyInjection;

internal static class IServiceAuthenticationExtension
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    )
    {
        services
             .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddMicrosoftIdentityWebApi(configuration.GetSection(nameof(AzureAdB2C)))
             .EnableTokenAcquisitionToCallDownstreamApi()
             .AddDownstreamApi(nameof(DownstreamApi), configuration.GetSection(nameof(DownstreamApi)))
             .AddInMemoryTokenCaches();

        services.AddAuthorization();
        return services;
    }
}
