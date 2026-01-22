namespace WhatsAppBusinessPlatform.API.Web.Extensions;

internal static class EndpointExtensions
{
    extension(RouteHandlerBuilder app)
    {
        public RouteHandlerBuilder HasPermission(string permission) => app.RequireAuthorization(permission);
    }
}
