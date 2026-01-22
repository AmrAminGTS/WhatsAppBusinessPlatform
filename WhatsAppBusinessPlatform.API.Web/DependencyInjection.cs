using System.Reflection;
using WhatsAppBusinessPlatform.API.Web.Infrastructure;

namespace WhatsAppBusinessPlatform.API.Web;
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPresentation()
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddControllers();

            //Configure CORS policy
            services.AddCors(options => 
                options.AddPolicy("AllowAllOrigins", builder => 
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails(options => options.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method}{context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
                    // traceId is automatically add
                });

            return services;
        }
    }
}
