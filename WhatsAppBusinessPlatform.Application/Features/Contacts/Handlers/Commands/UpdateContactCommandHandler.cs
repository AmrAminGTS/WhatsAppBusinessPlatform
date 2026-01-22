using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Handlers.Commands;

internal sealed class UpdateContactCommandHandler(IUnitOfWork _unitOfWork) 
    : ICommandHandler<UpdateContactCommand, Result>
{
    public async Task<Result> Handle(UpdateContactCommand request, CancellationToken cancellationToken = default)
    {
        Contact? contact = await _unitOfWork.Repository<Contact>()
            .Where(c => c.Id == request.ContactId)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (contact is null)
        {
            return ContactErrors.NotFound(request.ContactId);
        }
        contact.MapFrom(request.Request);
        _unitOfWork.Repository<Contact>().Update(contact);
        return await _unitOfWork.SaveAsync(cancellationToken);
    }
}
