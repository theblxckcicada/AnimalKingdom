using AnimalKingdom.API.Models;

namespace AnimalKingdom.API.Data;

public interface IQueryRepository
{
    IQueryable<T> Query<T>()
        where T : class, IEntityBase;
}

public interface ICommandRepository
{
    T AddOne<T>(T entity)
        where T : class, IEntityBase;
    List<T> AddMany<T>(List<T> entities)
        where T : class, IEntityBase;

    T RemoveOne<T>(T entity)
        where T : class, IEntityBase;

    T UpdateOne<T>(T entity)
        where T : class, IEntityBase;
    List<T> UpdateMany<T>(List<T> entities)
        where T : class, IEntityBase;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository : IQueryRepository, ICommandRepository { }
