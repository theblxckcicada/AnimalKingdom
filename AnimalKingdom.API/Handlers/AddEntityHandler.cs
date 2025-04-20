using AnimalKingdom.API.Data;
using AnimalKingdom.API.Models;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AnimalKingdom.API.Handlers;

public record AddEntityRequest<TModel, TKey> : IRequest<CommandResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    public TModel Entity { get; init; } = default!;
    public List<TModel> Entities { get; init; } = [];
}

public class AddEntityHandler<TModel, TKey>(
    IValidator<TModel> validator,
    ICommandRepository command,
    IMapper mapper,
    IEntityBaseKeyGenerator<TKey> keyGenerator
) : IRequestHandler<AddEntityRequest<TModel, TKey>, CommandResponse<TModel>>
    where TModel : class, IEntityBase<TKey>
{
    protected readonly IValidator<TModel> validator = validator;
    protected readonly ICommandRepository command = command;
    protected readonly IMapper mapper = mapper;
    protected readonly IEntityBaseKeyGenerator<TKey> keyGenerator = keyGenerator;

    public async Task<CommandResponse<TModel>> Handle(
        AddEntityRequest<TModel, TKey> request,
        CancellationToken cancellationToken
    )
    {
        if (request.Entities.Count > 0)
        {
            var entities = await PopulateEntitiesAsync(request.Entities, cancellationToken);
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

            var bulkAdded = command.AddMany(entities);
            _ = await command.SaveChangesAsync(cancellationToken);
            return new CommandResponse<TModel> { Entities = bulkAdded };
        }

        var entity = await PopulateEntityAsync(request.Entity, cancellationToken);
        var validationResult = await ValidateEntityAsync(entity, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new CommandResponse<TModel>
            {
                Entity = entity,
                ValidationResult = validationResult,
            };
        }

        var added = command.AddOne(entity);
        _ = await command.SaveChangesAsync(cancellationToken);

        return new CommandResponse<TModel> { Entity = added };
    }

    protected virtual async Task<TModel> PopulateEntityAsync(
        TModel entity,
        CancellationToken cancellationToken
    )
    {
        return await Task.FromResult(keyGenerator.Generate(entity));
    }

    protected virtual async Task<List<TModel>> PopulateEntitiesAsync(
        List<TModel> entities,
        CancellationToken cancellationToken
    )
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i] = await PopulateEntityAsync(entities[i], cancellationToken);
        }

        return entities;
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
