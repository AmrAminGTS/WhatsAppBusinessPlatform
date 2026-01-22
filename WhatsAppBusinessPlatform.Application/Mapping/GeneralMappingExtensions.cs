using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;

namespace WhatsAppBusinessPlatform.Application.Mapping;

internal static class GeneralMappingExtensions
{
    extension(DateTimeOffset)
    {
        public static DateTimeOffset FromTimestampString(string timestamp) 
            => DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp, CultureInfo.InvariantCulture));
    }

    extension(MessageContentType)
    {
        public static MessageContentType FromString(string value)
        {
            bool isParsable = Enum.TryParse(value, true, out MessageContentType type);
            if (isParsable)
            {
                return type;
            }
            else
            {
                return MessageContentType.Unknown;
            }
        }
    }
    extension(MessageStatusType)
    {
        public static MessageStatusType FromStatusString(string value)
        {
            bool isParsable = Enum.TryParse(value, true, out MessageStatusType type);
            if (isParsable)
            {
                return type;
            }
            else
            {
                throw new InvalidStatusException($"{value} is not Parable to type {nameof(MessageStatusType)}");
            }
        }
    }
}

[Serializable]
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
public class InvalidStatusException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
{
    public InvalidStatusException()
    {
    }

    public InvalidStatusException(string? message) : base(message)
    {
    }

    public InvalidStatusException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
