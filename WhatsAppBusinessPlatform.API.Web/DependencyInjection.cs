using WhatsAppBusinessPlatform.API.Web.Infrastructure;
using WhatsAppBusinessPlatform.API.Web.RealTime;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;

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
            services.AddSignalR();

            //Configure CORS policy
            services.AddCors(options => 
                options.AddPolicy("RealTimePolicy", builder => 
                    builder
                    .WithOrigins("https://whatsappui.majedsoft.net", "https://localhost:7260", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials())
                );

            services.AddScoped<IRealTimeMessagingChannel, RealTimeMessagingChannel>();
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
