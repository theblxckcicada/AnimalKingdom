using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContextFactory : IDesignTimeDbContextFactory<AnimalKingdomDbContext>
{
    public AnimalKingdomDbContext CreateDbContext(string[] args)
    {
        // Determine if we're in development
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true) // base config
            .AddJsonFile($"appsettings.{env}.json", optional: true) // environment-specific (e.g. appsettings.Development.json)
            .AddEnvironmentVariables() // this will map ConnectionStrings__AnimalKingdom, etc.
            .Build();

        var connectionString = config.GetConnectionString("AnimalKingdom");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Connection string 'AnimalKingdom' is not configured."
            );

        var optionsBuilder = new DbContextOptionsBuilder<AnimalKingdomDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AnimalKingdomDbContext(optionsBuilder.Options);
    }
}
