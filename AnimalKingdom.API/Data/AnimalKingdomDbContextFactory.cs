using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContextFactory : IDesignTimeDbContextFactory<AnimalKingdomDbContext>
{
    public AnimalKingdomDbContext CreateDbContext(string[] args)
    {
        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
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
