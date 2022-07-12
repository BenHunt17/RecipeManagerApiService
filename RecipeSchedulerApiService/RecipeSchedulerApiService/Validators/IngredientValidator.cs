using FluentValidation;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Validators
{
    public class IngredientValidator : AbstractValidator<IngredientModel>
    {
        public IngredientValidator()
        {
            //Defines validation rules for ingredient model. Name is required and the other types are quite lenient in that only size limits apply 
            RuleFor(property => property.IngredientName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(80);
            RuleFor(property => property.IngredientDescription)
                .MaximumLength(512);
            RuleFor(property => property.Calories)
                .LessThan(9999);
            RuleFor(property => property.Fat)
                .LessThan(9999);
            RuleFor(property => property.Salt)
                .LessThan(9999);
            RuleFor(property => property.Protein)
                .LessThan(9999);
            RuleFor(property => property.Carbs)
                .LessThan(9999);
        }
    }
}
