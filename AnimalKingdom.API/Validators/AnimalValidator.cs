using AnimalKingdom.API.Models;
using FluentValidation;

namespace AnimalKingdom.API.Validators;

public class AnimalValidator : AbstractValidator<Animal>
{
    public AnimalValidator()
    {
        // RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
