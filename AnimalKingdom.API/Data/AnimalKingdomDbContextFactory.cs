using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContextFactory : IDesignTimeDbContextFactory<AnimalKingdomDbContext>
{
    public AnimalKingdomDbContext CreateDbContext(string[] args)
    {
        // Get Default SQL Server  configuration
        var config  = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var DefaultConnectionString = config.GetConnectionString("AnimalKingdom");
        // var DefaultConnectionString =
        //     "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

        var optionsBuilder = new DbContextOptionsBuilder<AnimalKingdomDbContext>();
        optionsBuilder.UseSqlServer(DefaultConnectionString);

        return new AnimalKingdomDbContext(optionsBuilder.Options);
    }
}
