using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

internal sealed class WAHttpSettings
{
    public const string ConfigurationSection = "WAHttpSettings";
    public required string UserAccessToken { get; init; }
    public required string BaseUri { get; init; }
}
