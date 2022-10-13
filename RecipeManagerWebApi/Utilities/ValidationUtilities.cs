using FluentValidation.Results;
using System.Collections.Generic;
using System.Text;

namespace RecipeManagerWebApi.Utilities
{
    public static class ValidationUtilities
    {
        public static string BuildErrorsString(List<ValidationFailure> validationFailures)
        {
            StringBuilder stringBuilder = new StringBuilder("Errors:\n");
            
            foreach(ValidationFailure failure in validationFailures)
            {
                stringBuilder.AppendLine(failure.ErrorMessage);
            }

            return stringBuilder.ToString();
        }
    }
}
