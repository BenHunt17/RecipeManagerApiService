﻿using FluentValidation;
using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Validators
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
