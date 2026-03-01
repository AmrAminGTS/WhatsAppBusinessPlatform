using System.Threading.Channels;
using System.Net.ServerSentEvents;
using Microsoft.AspNetCore.Mvc;

using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.API.Web.RealTime;

namespace WhatsAppBusinessPlatform.API.Web.Extensions;
internal static class ApplicationBuilderExtensions
{
    extension(WebApplication app)
    {
        public IApplicationBuilder UseSwaggerWithUi()
        {
            app.UseSwagger();
            app.UseSwaggerUI(uiOpt =>
            {
                uiOpt.InjectJavascript("/swagger-ui/custom-controls.js");
                uiOpt.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
            return app;
        }
    }
    public static void MapServerSentEventEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("chats/realtime_events", (IServerEventChannel eventChannel, CancellationToken cancellationToken) =>
        {
            ChannelReader<SseItem<object>> reader = eventChannel.Subscribe();
            return Results.ServerSentEvents(reader.ReadAllAsync(cancellationToken));
        });

        // place holder -delete later
        app.MapGet("chats/sse-test", (CancellationToken cancellationToken) 
            => Results.ServerSentEvents(DummyTestData(), "dummy"));

#pragma warning disable S2190 // Loops and recursions should not be infinite
        static async IAsyncEnumerable<string> DummyTestData()
        {
            int counter = 0;
            while (true)
            {
                await Task.Delay(2000);
                counter++;
                yield return $"Dummy message no: {counter}";
            }
        }
#pragma warning restore S2190 // Loops and recursions should not be infinite
    }

    public static void MapSignalRHubs(this IEndpointRouteBuilder app) 
        => app.MapHub<MessagingHub>("RealTimeHubs/Messaging");
}
