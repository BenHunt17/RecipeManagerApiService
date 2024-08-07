﻿using FluentValidation;
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
        }
    }
}
