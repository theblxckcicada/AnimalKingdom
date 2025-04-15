using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using AutoMapper;
using MediatR;

namespace AnimalKingdom.API.Handlers;

public record GetEntitiesRequest<TModel, TKey> : IRequest<IList<TModel>>
    where TModel : class, IEntityBase<TKey> { }

public class GetEntitiesHandler<TModel, TKey>(IQueryRepository query, IMapper mapper)
    : IRequestHandler<GetEntitiesRequest<TModel, TKey>, IList<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    private readonly IQueryRepository query = query;
    private readonly IMapper mapper = mapper;

    public async Task<IList<TModel>> Handle(
        GetEntitiesRequest<TModel, TKey> request,
        CancellationToken cancellationToken
    )
    {
        var entities = query.Query<TModel>().ToList();
        return [.. entities.Select(mapper.Map<TModel>)];
    }
}
