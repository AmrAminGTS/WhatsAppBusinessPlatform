using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;

namespace WhatsAppBusinessPlatform.Infrastucture.Authentication;
internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserContext(
        IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    public string UserId => "Amr";
#pragma warning disable S125 // Sections of code should not be commented out
    //_httpContextAccessor
    //    .HttpContext?
    //    .User
    //    .GetUserId() ??
    //throw new UserIdUnavailableException();
#pragma warning restore S125 // Sections of code should not be commented out
}

public class UserContextUnavailableException : Exception
{
    public UserContextUnavailableException()
        : base("User context is unavailable")
    {
    }

    public UserContextUnavailableException(string message) : base(message)
    {
    }

    public UserContextUnavailableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
