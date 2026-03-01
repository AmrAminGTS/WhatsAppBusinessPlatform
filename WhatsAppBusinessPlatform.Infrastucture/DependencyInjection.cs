using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Abstractions.ThirdParties;
using WhatsAppBusinessPlatform.Infrastucture.Authentication;
using WhatsAppBusinessPlatform.Infrastucture.Persistence;
using WhatsAppBusinessPlatform.Infrastucture.Realtime;
using WhatsAppBusinessPlatform.Infrastucture.Shared;
using WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

namespace WhatsAppBusinessPlatform.Infrastucture;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurePersistenceServices(configuration);
        services.ConfigureIdentityServices(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddOptions<WAHttpSettings>()
            .BindConfiguration(WAHttpSettings.ConfigurationSection)
            .ValidateOnStart();

        services.AddHttpClient<IWAHttpClient, WAHttpClient>((serviceProvider, httpClient) =>
        {
            WAHttpSettings waHttpSettings = serviceProvider.GetRequiredService<IOptions<WAHttpSettings>>().Value;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {waHttpSettings.UserAccessToken}");
            httpClient.BaseAddress = new Uri(waHttpSettings.BaseUri);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddSingleton<IServerEventChannel, ServerEventChannel>();
        services.AddScoped<IIdempotencyHandler, IdempotencyHandler>();

        return services;
    }
}
