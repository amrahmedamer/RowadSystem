using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RowadSystem.Extensions;

public static class ValidationExtensions
{
    public static async Task<IActionResult?> ValidateWithModelStateAsync<T>(this IValidator<T> validator,
        T model,
        ModelStateDictionary modelState) where T : class
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return new BadRequestObjectResult(modelState);
        }

        return null;
    }

}

