using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Chats.Requests.Commands;

namespace WhatsAppBusinessPlatform.Application.Features.Chats.Handlers.Commands;

internal sealed class DeleteChatCommandHandler : ICommandHandler<DeleteChatCommand, Result>
{
    public Task<Result> Handle(DeleteChatCommand request, CancellationToken cancellationToken = default) 
        => throw new NotImplementedException();
}
