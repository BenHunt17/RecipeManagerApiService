using FluentValidation;
using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Validators
{
    public class RecipeIngredientValidator : AbstractValidator<RecipeIngredientModel>
    {
        public RecipeIngredientValidator()
        {
            RuleFor(property => property.IngredientId)
                .NotNull();
            RuleFor(property => property.Quantity)
                .LessThan(9999);
        }
    }
}
