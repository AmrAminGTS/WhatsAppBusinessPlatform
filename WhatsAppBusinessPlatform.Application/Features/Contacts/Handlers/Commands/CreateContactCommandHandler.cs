using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Handlers.Commands;
internal sealed class CreateContactCommandHandler : ICommandHandler<CreateContactCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IIdempotencyHandler _idempotencyHandler;
    public CreateContactCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider dateTimeProvider,
        IIdempotencyHandler idempotencyHandler)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
        _idempotencyHandler = idempotencyHandler;
    }

    public async Task<Result<string>> Handle(CreateContactCommand request, CancellationToken cancellationToken = default) 
        => await _idempotencyHandler.HandleWithIdempotencyAsync(request.IdempotencyKey, async Task<Result<string>> () => 
        {
            IRepository<Contact> contactRepo = _unitOfWork.Repository<Contact>();
            IRepository<WAAccount> accountRepo = _unitOfWork.Repository<WAAccount>();

            // check if Anonymous Contact with same phoneNumber exists
            Contact? existingContact = await contactRepo
                .Where(c => c.WAAccount.PhoneNumber == request.ContactRequest.PhoneNumber)
                .SingleOrDefaultAsync(cancellationToken);

            // check if WhatsApp Account already exists
            WAAccount? existingAccount = await accountRepo.Where(a => a.PhoneNumber == request.ContactRequest.PhoneNumber)
                .SingleOrDefaultAsync(cancellationToken);

            WAAccount account;
            if (existingContact != null)
            {
                return ContactErrors.AlreadyExists(existingContact.WAAccount.PhoneNumber);
            }
            else if (existingAccount != null)
            {
                // Link contact with existing account
                account = existingAccount;
            }
            else
            {
                // Create new contact with new Account
                account = new WAAccount()
                {
                    PhoneNumber = request.ContactRequest.PhoneNumber,
                };
            }
            var contact = Contact.MapFrom(request.ContactRequest, account, _userContext.UserId, _dateTimeProvider.UtcNow);
            contactRepo.Add(contact);

            // Save 
            Result<int> insertResult = await _unitOfWork.SaveAsync(cancellationToken);
            if (insertResult.IsFailure)
            {
                return insertResult.Error;
            }
            return contact.Id;
        }, cancellationToken: cancellationToken);
}
