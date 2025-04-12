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
        return this.Set<T>().Add(entity).Entity;
    }

    public IQueryable<T> Query<T>()
        where T : class, IEntityBase
    {
        return this.Set<T>();
    }

    public T RemoveOne<T>(T entity)
        where T : class, IEntityBase
    {
        return this.Set<T>().Remove(entity).Entity;
    }

    public T UpdateOne<T>(T entity)
        where T : class, IEntityBase
    {
        return this.Set<T>().Update(entity).Entity;
    }
}
