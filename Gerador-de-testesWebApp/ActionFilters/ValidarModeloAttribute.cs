using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TesteFacil.WebApp.ActionFilters;

public class ValidarModeloAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller controller)
            return;

        var modelState = context.ModelState;

        var viewModel = context.ActionArguments.Values
            .FirstOrDefault(x => x?.GetType().Name.EndsWith("ViewModel") == true);

        if (!modelState.IsValid && viewModel is not null)
            context.HttpContext.Items["ModelStateInvalid"] = true;
    }
}