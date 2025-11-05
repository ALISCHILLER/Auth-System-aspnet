using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthSystem.Api.Filters;

public sealed class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Any() == true)
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(new
            {
                title = "Validation failed",
                status = 400,
                errors
            });
        }
    }
}