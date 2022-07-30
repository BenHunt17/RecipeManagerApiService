using FluentValidation;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Validators
{
    public class InstructionValidator : AbstractValidator<InstructionModel>
    {
        public InstructionValidator()
        {
            RuleFor(property => property.InstructionNumber)
                .GreaterThan(0);
            RuleFor(property => property.InstructionText)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
