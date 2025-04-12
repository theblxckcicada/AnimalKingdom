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

        var added = command.AddOne(mapper.Map<TModel>(entity));
        _ = await command.SaveChangesAsync(cancellationToken);

        return new CommandResponse<TModel> { Entity = mapper.Map<TModel>(added) };
    }

    protected virtual async Task<TModel> PopulateEntityAsync(
        TModel entity,
        CancellationToken cancellationToken
    )
    {
        return await Task.FromResult(keyGenerator.Generate(entity));
    }

    protected virtual async Task<ValidationResult> ValidateEntityAsync(
        TModel entity,
        CancellationToken cancellationToken
    )
    {
        return await validator.ValidateAsync(entity, cancellationToken);
    }
}
