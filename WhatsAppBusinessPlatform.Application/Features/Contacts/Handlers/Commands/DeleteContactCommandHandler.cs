using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Handlers.Commands;

internal sealed class DeleteContactCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteContactCommand, Result>
{
    public async Task<Result> Handle(DeleteContactCommand request, CancellationToken cancellationToken = default)
    {
        Contact? contact = await unitOfWork.Repository<Contact>()
            .Where(c => c.Id == request.ContactId)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (contact is null)
        {
            return ContactErrors.NotFound(request.ContactId);
        }
        unitOfWork.Repository<Contact>().Delete(contact);
        return await unitOfWork.SaveAsync(cancellationToken);
    }
}
