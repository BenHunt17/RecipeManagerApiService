using FluentValidation;
using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(property => property.Username)
                .NotEmpty()
                .MaximumLength(80);
            //TODO - Maybe password before it is hashed. Again will need to think aabout using these validators on inputs not models
        }
    }
}
