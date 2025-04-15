using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using AutoMapper;
using FluentValidation.Results;
using MediatR;

namespace AnimalKingdom.API.Handlers;

public record RemoveEntityRequest<TModel, TKey> : IRequest<CommandResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    public TKey Id { get; init; } = default!;
}

public class RemoveEntityHandler<TModel, TKey>(
    IQueryRepository query,
    ICommandRepository command,
    IMapper mapper
) : IRequestHandler<RemoveEntityRequest<TModel, TKey>, CommandResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IQueryRepository query = query;
    private readonly ICommandRepository command = command;
    private readonly IMapper mapper = mapper;

    public async Task<CommandResponse<TModel>> Handle(
        RemoveEntityRequest<TModel, TKey> request,
        CancellationToken cancellationToken
    )
    {
        var entity = query.Query<TModel>().FirstOrDefault(x => x.Id.Equals(request.Id));

        if (entity == null)
        {
            return new CommandResponse<TModel>();
        }

        var model = entity;
        var validationResult = await ValidateEntityAsync(model, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new CommandResponse<TModel>
            {
                Entity = model,
                ValidationResult = validationResult,
            };
        }

        var removed = command.RemoveOne(entity);
        _ = await command.SaveChangesAsync(cancellationToken);

        return new CommandResponse<TModel> { Entity = removed };
    }

    protected virtual Task<ValidationResult> ValidateEntityAsync(
        TModel entity,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new ValidationResult());
    }
}
