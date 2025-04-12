using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContextFactory : IDesignTimeDbContextFactory<AnimalKingdomDbContext>
{
    public AnimalKingdomDbContext CreateDbContext(string[] args)
    {
        const string DefaultConnectionString = "Filename=Data\\AnimalKingdom.db";

        var optionsBuilder = new DbContextOptionsBuilder<AnimalKingdomDbContext>();
        optionsBuilder.UseSqlite(DefaultConnectionString);

        return new AnimalKingdomDbContext(optionsBuilder.Options);
    }
}
