using FluentValidation.Results;

namespace AnimalKingdom.API.Models;

public record CommandResponse<TModel>
{
    public ValidationResult ValidationResult { get; init; } = new ValidationResult();
    public TModel? Entity { get; init; }
}
