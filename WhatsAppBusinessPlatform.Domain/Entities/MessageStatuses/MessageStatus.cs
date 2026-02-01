using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;

public sealed class MessageStatus : BaseDomainEntity<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public required DateTimeOffset DateTimeOffset { get; set; }
    public required MessageStatusType Status { get; set; }
    public bool IsBillable { get; set; }
    // Pricing_Model ("PMP", ...)
    public string? PricingModel { get; set; }
    // Category ("utility", ...)
    public string? PricingCategory { get; set; }
    // Type ("free_customer_service", ...)
    public string? PricingType { get; set; }
    public required byte[] RawWebhookReqest { get; set; } 
    public required string MessageId { get; set; }

    // Navigation Properties
    public required WAMessage Message { get; set; }
    public StatusError? Error { get; set => field = Status == MessageStatusType.Failed ? value : null;}
}

public enum MessageStatusType
{
    Failed,
    Sent,
    Delivered,
    Read,
}

public class StatusError : BaseDomainEntity<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString() ;
    public int Code { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required string Details { get; set; }
    public required string Href { get; set; }
    public string? StatusId { get; set; }

    // Navigation Properties
    public MessageStatus? Status { get; set; }
}
