using FluentValidation;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Validators
{
    public class RecipeValidator : AbstractValidator<RecipeModel>
    {
        public RecipeValidator()
        {
            //Defines validation rules for ingredient model. Name is required and the other types are quite lenient in that only size limits apply 
            RuleFor(property => property.RecipeName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(80);
            RuleFor(property => property.RecipeDescription)
                .MaximumLength(512);
            RuleFor(property => property.Ingredients)
                .NotEmpty();
            RuleForEach(property => property.Ingredients)
                .SetValidator(new RecipeIngredientValidator());
            RuleFor(property => property.Instructions)
                .NotEmpty();
            RuleForEach(property => property.Instructions)
                .SetValidator(new InstructionValidator());
            RuleFor(property => property.Rating)
                .GreaterThan(0)
                .LessThan(6);
            RuleFor(property => property.PrepTime)
                .GreaterThan(0)
                .LessThan(9999);
            RuleFor(property => property.ServingSize)
                .GreaterThan(0)
                .LessThan(13); //Maximum 12 people (can change if the situation requires in in future)
            RuleFor(property => property.Breakfast).Equal(true).When(property => !(property.Lunch || property.Dinner)); //Breakfast flag cannot be false if both lunch and dinner are also false. This is because the recipe has to be one of three meals of the day
            RuleFor(property => property.Lunch).Equal(true).When(property => !(property.Breakfast || property.Dinner)); 
            RuleFor(property => property.Dinner).Equal(true).When(property => !(property.Breakfast || property.Lunch)); 
        }
    }
}
