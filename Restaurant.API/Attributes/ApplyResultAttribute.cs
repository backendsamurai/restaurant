using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Restaurant.API.Types;

namespace Restaurant.API.Attributes;

public class ApplyResultAttribute(ILogger<ApplyResultAttribute> logger) : ActionFilterAttribute
{
    private readonly ILogger<ApplyResultAttribute> _logger = logger;

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if ((context.Result as ObjectResult)?.Value is not Types.IResult result) return;

        if (context.Controller is not ControllerBase controller) return;

        if (result.Status is ResultStatus.NotFound)
            _logger.LogWarning("Data not found: {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.Error)
            _logger.LogWarning("Internal error: {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.Forbidden)
            _logger.LogWarning("Forbidden access: {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.Unauthorized)
            _logger.LogWarning("Unauthorized request: {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.Invalid)
            _logger.LogWarning("Validation error: {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.Conflict)
            _logger.LogWarning("Conflict : {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.Unavailable)
            _logger.LogError("Service unavailable: {@Err}", result.GetDetailedError());

        if (result.Status is ResultStatus.CriticalError)
            _logger.LogError("Critical Error: {@Err}", result.GetDetailedError());

        context.Result = controller.StatusCode(result.Status, result.GetValue());
    }
}
