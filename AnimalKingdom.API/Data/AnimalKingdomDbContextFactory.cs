using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContextFactory : IDesignTimeDbContextFactory<AnimalKingdomDbContext>
{
    public AnimalKingdomDbContext CreateDbContext(string[] args)
    {
        var envConnection = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        string? connectionString = !string.IsNullOrWhiteSpace(envConnection)
            ? envConnection
            : new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build()
                .GetConnectionString("AnimalKingdom");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is not configured.");

        var optionsBuilder = new DbContextOptionsBuilder<AnimalKingdomDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AnimalKingdomDbContext(optionsBuilder.Options);
    }
}
