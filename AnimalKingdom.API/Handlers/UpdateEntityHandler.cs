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
    public List<TModel> Entities { get; init; } = default!;
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
        if (request.Entities.Count > 0)
        {
            var entities = request.Entities;
            var bulkValidationResult = await ValidateEntitiesAsync(entities, cancellationToken);

            var notValidated = bulkValidationResult.Where(x => !x.IsValid).ToList();
            if (notValidated.Count > 0)
            {
                return new CommandResponse<TModel>
                {
                    Entities = entities,
                    ValidationResult = bulkValidationResult.FirstOrDefault(),
                };
            }

            var bulkUpdated = command.UpdateMany(entities);
            _ = await command.SaveChangesAsync(cancellationToken);
            return new CommandResponse<TModel> { Entities = bulkUpdated };
        }

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

        var updated = command.UpdateOne(entity);
        _ = await command.SaveChangesAsync(cancellationToken);

        return new CommandResponse<TModel> { Entity = updated };
    }

    protected virtual async Task<ValidationResult> ValidateEntityAsync(
        TModel entity,
        CancellationToken cancellationToken
    )
    {
        return await validator.ValidateAsync(entity, cancellationToken);
    }

    protected virtual async Task<List<ValidationResult>> ValidateEntitiesAsync(
        List<TModel> entities,
        CancellationToken cancellationToken
    )
    {
        var results = new List<ValidationResult>();

        foreach (var entity in entities)
        {
            var result = await ValidateEntityAsync(entity, cancellationToken);
            results.Add(result);
        }

        return results;
    }
}
