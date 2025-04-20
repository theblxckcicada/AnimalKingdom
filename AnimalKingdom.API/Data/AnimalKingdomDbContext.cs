using AnimalKingdom.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimalKingdom.API.Data;

public class AnimalKingdomDbContext(DbContextOptions<AnimalKingdomDbContext> options)
    : DbContext(options),
        IRepository
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimalKingdomDbContext).Assembly);
    }

    public T AddOne<T>(T entity)
        where T : class, IEntityBase
    {
        return Set<T>().Add(entity).Entity;
    }

    public IQueryable<T> Query<T>()
        where T : class, IEntityBase
    {
        return Set<T>();
    }

    public T RemoveOne<T>(T entity)
        where T : class, IEntityBase
    {
        return Set<T>().Remove(entity).Entity;
    }

    public T UpdateOne<T>(T entity)
        where T : class, IEntityBase
    {
        return Set<T>().Update(entity).Entity;
    }

    public List<T> AddMany<T>(List<T> entities)
        where T : class, IEntityBase
    {
        List<T> results = [];
        foreach (var entity in entities)
        {
            results.Add(AddOne(entity));
        }
        return results;
    }
    public List<T> UpdateMany<T>(List<T> entities)
        where T : class, IEntityBase
    {
        List<T> results = [];
        foreach (var entity in entities)
        {
            results.Add(UpdateOne(entity));
        }
        return results;
    }
}
