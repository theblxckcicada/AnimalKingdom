using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using AutoMapper;
using MediatR;

namespace AnimalKingdom.API.Handlers;

public record GetEntityRequest<TModel, TKey> : IRequest<TModel?>
    where TModel : class, IEntityBase<TKey>
{
    public TKey Id { get; init; } = default!;
}

public class GetEntityHandler<TModel, TKey>(IQueryRepository query, IMapper mapper)
    : IRequestHandler<GetEntityRequest<TModel, TKey>, TModel?>
    where TModel : class, IEntityBase<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly IQueryRepository query = query;
    private readonly IMapper mapper = mapper;

    public async Task<TModel?> Handle(
        GetEntityRequest<TModel, TKey> request,
        CancellationToken cancellationToken
    )
    {
        var account = query.Query<TModel>().FirstOrDefault(x => x.Id.Equals(request.Id));
        return mapper.Map<TModel>(account);
    }
}
