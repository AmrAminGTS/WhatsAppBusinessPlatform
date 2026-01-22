using WhatsAppBusinessPlatform.Domain.Common;
using FluentValidation.Results;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Common;
public sealed record ValidationError : DomainError
{
    public ValidationError(DomainError[] errors)
        : base("Validation.General", ErrorType.Validation) => Errors = errors;
    public DomainError[] Errors { get; }

    public static ValidationError FromValidationFailures(IEnumerable<ValidationFailure> validationFailures)
    {
        IEnumerable<DomainError> validationErrors = validationFailures
            .Select(failure => new DomainError(
                failure.ErrorCode, 
                ErrorType.Validation, 
                failure.PropertyName, 
                failure.ErrorMessage));

        ValidationError validationError = new (validationErrors.ToArray());
        return validationError;
    }
}
