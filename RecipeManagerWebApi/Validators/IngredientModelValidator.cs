using FluentValidation;
using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Validators
{
    public class IngredientModelValidator : AbstractValidator<IngredientModel>
    {
        public IngredientModelValidator()
        {
            //Defines validation rules for ingredient model. The statistics properties have a very lenient allowance being any 0 or positive number below 9999
            
            RuleFor(property => property.IngredientName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(80);
            RuleFor(property => property.IngredientDescription)
                .MaximumLength(512);
            RuleFor(property => property.Calories)
                .GreaterThanOrEqualTo(0)
                .LessThan(9999);
            RuleFor(property => property.Fat)
                .GreaterThanOrEqualTo(0)
                .LessThan(9999);
            RuleFor(property => property.Salt)
                .GreaterThanOrEqualTo(0)
                .LessThan(9999);
            RuleFor(property => property.Protein)
                .GreaterThanOrEqualTo(0)
                .LessThan(9999);
            RuleFor(property => property.Carbs)
                .GreaterThanOrEqualTo(0)
                .LessThan(9999);
        }
    }
}
