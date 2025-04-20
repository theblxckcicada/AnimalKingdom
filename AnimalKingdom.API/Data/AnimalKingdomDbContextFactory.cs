using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContextFactory : IDesignTimeDbContextFactory<AnimalKingdomDbContext>
{
    public AnimalKingdomDbContext CreateDbContext(string[] args)
    {
        // Load environment variables
        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        var connectionString = config.GetSection("ConnectionStrings")["AnimalKingdom"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "ConnectionStrings:AnimalKingdom is not set in the environment variables."
            );

        var optionsBuilder = new DbContextOptionsBuilder<AnimalKingdomDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AnimalKingdomDbContext(optionsBuilder.Options);
    }
}
