using System.Net.Http;

namespace WhatsAppBusinessPlatform.Domain.Common;
public record DomainError
{
    public static readonly DomainError None = new(string.Empty, ErrorType.None);
    public static readonly DomainError NullValue = new("General.Null", ErrorType.Failure);
    public DomainError(string code, ErrorType type, params object[] args)
    {
        Code = code;
        ErrorType = type;
        if (args is not null)
        {
            Args = args;
        }
    }
    public string Code { get; }
    public object[] Args { get; } = [];
    public ErrorType ErrorType { get; }

    // Methods to create specific error instances
    public static DomainError Failure(string code, params object[] args) =>
        new(code, ErrorType.Failure, args);
    public static DomainError NotFound(string code, params object[] args) =>
        new(code, ErrorType.NotFound, args);
    public static DomainError Problem(string code, params object[] args) =>
        new(code, ErrorType.Problem, args);
    public static DomainError Conflict(string code, params object[] args) =>
        new(code, ErrorType.Conflict, args);
    public static DomainError Unauthorized(string code, params object[] args) =>
        new(code, ErrorType.Unauthorized, args);
}

