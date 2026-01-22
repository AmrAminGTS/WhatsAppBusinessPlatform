using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WhatsAppBusinessPlatform.Application.Abstractions.ThirdParties;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

public sealed class WAHttpClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions) : IWAHttpClient
{
    // Fields
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
    // Methods
    public async Task<Result<string>> SendMessageAsync<TMessageContent>(
        SendWAMessageRequest<TMessageContent> messageObj,
        CancellationToken cancellationToken) where TMessageContent : IMessageContentType
    {
        string messageType = messageObj.MessageContent.MessageContentType.ToString().ToLowerInvariant();

        var context = messageObj.ReplyToMessageId != null
            ? new { message_id = messageObj.ReplyToMessageId }
            : null;
        var payload = new Dictionary<string, object?>
        {
            ["messaging_product"] = "whatsapp",
            ["to"] = messageObj.RecipientPhoneNumber,
            ["type"] = messageType,
            [messageType] = messageObj.MessageContent,
        };
        if (context is not null)
        {
            payload["context"] = context;
        }

        string contentJson = JsonSerializer.Serialize(payload, _jsonSerializerOptions);

        Uri uri = new(_httpClient.BaseAddress!, $"{messageObj.BusinessPhoneNumberId}/messages");
        using StringContent content = new(contentJson, Encoding.UTF8, "application/json");
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(uri, content, cancellationToken);
            if (response.IsSuccessStatusCode == false)
            {
                SendMessageErrorResponse? errorResponse = await response
                    .Content.ReadFromJsonAsync<SendMessageErrorResponse>(cancellationToken);
                return MessagingErrors.UnsuccessfulStatusCode(errorResponse!.Error.Message);
            }
            SendMessageAcceptanceResponse? responseJsonContent = await response
                .Content.ReadFromJsonAsync<SendMessageAcceptanceResponse>(cancellationToken: cancellationToken);

            return responseJsonContent!.Messages.First().MessageId;
        }
        catch (Exception ex)
        {
            return MessagingErrors.NotSent(ex.Message);
        }
    }
}
