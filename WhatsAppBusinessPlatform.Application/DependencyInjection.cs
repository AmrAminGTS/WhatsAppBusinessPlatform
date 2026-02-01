using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using MediatAmR;
using MediatAmR.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Commands;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

namespace WhatsAppBusinessPlatform.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        services.AddSingleton(options);
        services.AddMemoryCache();
        services.AddMediatAmR();

        services.AddScoped<IRequestHandler<SendMessageCommand<TextMessageContent>, Result<string>>,
            SendMessageCommandHandler<TextMessageContent>>();
        services.AddScoped<IRequestHandler<SendMessageCommand<ReactionMessageContent>, Result<string>>,
            SendMessageCommandHandler<ReactionMessageContent>>();
        services.AddScoped<IRequestHandler<ReactToMessageCommand, Result<string>>,
            ReactToMessageCommandHandler>();
        services.AddScoped<IRequestHandler<RemoveMessageReactionCommand, Result<ChatLastUpdateResponse?>>,
            RemoveMessageReactionCommandHandler>();

        return services;
    }
}
