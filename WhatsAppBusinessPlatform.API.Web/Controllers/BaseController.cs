using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WhatsAppBusinessPlatform.API.Web.Localization;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.API.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class BaseController : ControllerBase
{        
    [NonAction]
    public ObjectResult ToProblemDetails(DomainError error)
    {
        IStringLocalizer localizer = HttpContext.RequestServices
            .GetService(typeof(IStringLocalizer<SharedResource>)) as IStringLocalizer
            ?? throw new InvalidOperationException("IStringLocalizer<SharedResource> not registered.");

        ObjectResult objResult = Problem(
            title: GetTitle(error),
            detail: GetDetail(error),
            type: GetType(error),
            statusCode: GetStatusCode(error));

        var problemDetails = objResult.Value as ProblemDetails;
        List<DomainError>? validationErrors = GetErrors(error);
        if (validationErrors is not null)
        {
            foreach (DomainError ve in validationErrors)
            {
                Console.WriteLine(ve.Args);
            }
            var errors = validationErrors
                .Select(e => new {
                    Property = e.Args[0],
                    Detail = localizer[e.Code].ResourceNotFound ? e.Args[1] : localizer[e.Code].Value
                })
                .ToList();
            problemDetails!.Extensions["errors"] = errors;
        }
        return objResult;

        static string GetTitle(DomainError error) =>
            error.ErrorType switch
            {
                ErrorType.Validation => error.Code,
                ErrorType.Problem => error.Code,
                ErrorType.NotFound => error.Code,
                ErrorType.Conflict => error.Code,
                ErrorType.Unauthorized => error.Code,
                _ => "Server failure"
            };
        string GetDetail(DomainError error)
        {
            LocalizedString detail = localizer[error.Code, error.Args];
            return detail;
        }
        static string GetType(DomainError error) =>
            error.ErrorType switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                ErrorType.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
        static int GetStatusCode(DomainError error) =>
            error.ErrorType switch
            {
                ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
        static List<DomainError>? GetErrors(DomainError error)
        {
            if (error is not ValidationError validationError)
            {
                return null;
            }
            return validationError.Errors.ToList();
        }
    }
}
