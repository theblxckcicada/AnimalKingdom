using System.Linq.Expressions;
using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using AutoMapper;
using MediatR;

namespace AnimalKingdom.API.Handlers;

public record QueryEntitiesRequest<TModel, TKey> : IRequest<QueryEntitiesResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    public Expression<Func<TModel, bool>> Filter { get; init; } = (entity) => true;
    public Func<IQueryable<TModel>, IOrderedQueryable<TModel>> Sort { get; init; } =
        (query) => query.OrderBy(x => x.Id);
    public int PageSize { get; init; } = 10;
    public int PageIndex { get; init; } = 0;
}

public record QueryEntitiesResponse<TModel>
{
    public IList<TModel> Entities { get; init; } = new List<TModel>();
    public int Total { get; init; } = 0;
}

public class QueryEntitiesHandler<TModel, TKey>(IQueryRepository query, IMapper mapper)
    : IRequestHandler<QueryEntitiesRequest<TModel, TKey>, QueryEntitiesResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    private readonly IQueryRepository query = query;
    private readonly IMapper mapper = mapper;

    public async Task<QueryEntitiesResponse<TModel>> Handle(
        QueryEntitiesRequest<TModel, TKey> request,
        CancellationToken cancellationToken
    )
    {
        if (request.PageSize <= 0 || request.PageIndex < 0)
        {
            return new QueryEntitiesResponse<TModel>();
        }

        var entityQuery = query.Query<TModel>();

        entityQuery = entityQuery.Where(request.Filter);

        var total = entityQuery.Count();

        if (total == 0)
        {
            return new QueryEntitiesResponse<TModel>();
        }

        entityQuery = request.Sort(entityQuery);

        entityQuery = entityQuery.Skip(request.PageIndex * request.PageSize);
        /* Return entities based on user id only if user id is not null,
         * if user id is null that may mean we are not querying based on user access
         * */
        // Get the total entities based on the returned query
        total = total = entityQuery.Count();

        // Get entities for the requested page size
        entityQuery = entityQuery.Take(request.PageSize);

        var entities = entityQuery.ToList();

        return new QueryEntitiesResponse<TModel>
        {
            Total = total,
            Entities = entities.Select(entity => mapper.Map<TModel>(entity)).ToList(),
        };
    }
}
