using FluentValidation;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Validators
{
    public class RecipeIngredientValidator : AbstractValidator<RecipeIngredientModel>
    {
        public RecipeIngredientValidator()
        {
            RuleFor(property => property.Quantity)
                .LessThan(9999);
            RuleFor(property => property.Density)
                .LessThan(9999);
            RuleFor(property => property.MeasureTypeValue)
                .NotNull();
        }
    }
}
