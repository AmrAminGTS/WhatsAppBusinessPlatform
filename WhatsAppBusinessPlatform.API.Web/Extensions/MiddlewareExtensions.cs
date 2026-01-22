using WhatsAppBusinessPlatform.API.Web.Middleware;

namespace WhatsAppBusinessPlatform.API.Web.Extensions;

internal static class MiddlewareExtensions
{
    extension(IApplicationBuilder app)
    {
        public IApplicationBuilder UseRequestContextLogging()
        {
            app.UseMiddleware<RequestContextLoggingMiddleware>();

            return app;
        }
    }
}
