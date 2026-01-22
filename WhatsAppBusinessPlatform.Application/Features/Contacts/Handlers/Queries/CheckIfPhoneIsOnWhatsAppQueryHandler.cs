using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Handlers.Queries;

internal sealed class CheckIfPhoneIsOnWhatsAppQueryHandler : IQueryHandler<CheckIfPhoneIsOnWhatsAppQuery, Result>
{
    public Task<Result> Handle(CheckIfPhoneIsOnWhatsAppQuery request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
