using System.Security.Claims;

namespace WhatsAppBusinessPlatform.Infrastucture.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId ?? throw new UserIdUnavailableException();
    }
}

public class UserIdUnavailableException : Exception
{
    public UserIdUnavailableException() : base("User id is unavailable")
    {
    }

    public UserIdUnavailableException(string message) : base(message)
    {
    }

    public UserIdUnavailableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
