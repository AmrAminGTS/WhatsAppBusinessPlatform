using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;

namespace WhatsAppBusinessPlatform.Application.Abstractions.ThirdParties;

public interface IWAHttpClient
{
    Task<Result<string>> SendMessageAsync<TMessageType>(
        SendWAMessageRequest<TMessageType> messageObj,
        CancellationToken cancellationToken) where TMessageType : IMessageContentType;
}
