using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AnimalKingdom.API.Handlers;

public record UpdateEntityRequest<TModel, TKey> : IRequest<CommandResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    public TKey Id { get; init; } = default!;
    public TModel Entity { get; init; } = default!;
}

public class UpdateEntityHandler<TModel, TKey>(
    IValidator<TModel> validator,
    IQueryRepository query,
    ICommandRepository command,
    IMapper mapper
) : IRequestHandler<UpdateEntityRequest<TModel, TKey>, CommandResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IValidator<TModel> validator = validator;
    protected readonly IQueryRepository query = query;
    protected readonly ICommandRepository command = command;
    protected readonly IMapper mapper = mapper;

    public async Task<CommandResponse<TModel>> Handle(
        UpdateEntityRequest<TModel, TKey> request,
        CancellationToken cancellationToken
    )
    {
        var entity = request.Entity;

        var validationResult = await ValidateEntityAsync(entity, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new CommandResponse<TModel>
            {
                Entity = entity,
                ValidationResult = validationResult,
            };
        }

        var updated = command.UpdateOne(mapper.Map<TModel>(entity));
        _ = await command.SaveChangesAsync(cancellationToken);

        return new CommandResponse<TModel> { Entity = mapper.Map<TModel>(updated) };
    }

    protected virtual async Task<ValidationResult> ValidateEntityAsync(
        TModel entity,
        CancellationToken cancellationToken
    )
    {
        return await validator.ValidateAsync(entity, cancellationToken);
    }
}
