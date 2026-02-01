using WhatsAppBusinessPlatform.Application.DTOs.Webhook;
using WhatsAppBusinessPlatform.Application.Mapping;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Webhook;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;

namespace WhatsAppBusinessPlatform.Application.Mapping.Maps.Webhook;

internal static class WAMessageStatusMappingExtensions
{
    extension(WebhookReceivedMessageStatus src)
    {
        public MessageStatus MapToWAMessageStatus(
            WAMessage wAMessage,
            byte[] rawBytes) => new ()
        {
            MessageId = src.MessageId,
            DateTimeOffset = DateTimeOffset.FromTimestampString(src.Timestamp),
            Status = MessageStatusType.FromStatusString(src.Status),
            IsBillable = src.Pricing != null && src.Pricing.Billable,
            PricingModel = src.Pricing?.PricingModel,
            PricingCategory = src.Pricing?.Category,
            PricingType = src.Pricing?.Type,
            Message = wAMessage,
            RawWebhookReqest = rawBytes,
            Error = src.Errors?.Select(e => new StatusError()
            {
                Code = e.Code,
                Title = e.Title,
                Message = e.Message,
                Details = e.ErrorData.Details,
                Href = e.Href,
            }).FirstOrDefault()
        };
    }
}
