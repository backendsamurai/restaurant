using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurant.API.Attributes;

public class ApplyResultAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if ((context.Result as ObjectResult)?.Value is not Types.IResult result) return;

        if (context.Controller is not ControllerBase controller) return;

        context.Result = controller.StatusCode(result.Status, result.GetValue());
    }
}
