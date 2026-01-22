using System.Linq.Expressions;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Mapping.Maps.Contacts;

internal static class ContactMappingExtensions
{
    extension(Contact src)
    {
        public static Expression<Func<Contact, GetAllContactsResponse>> ProjectToGetAllContactsResponse() => src => new ()
        {
            Id = src.Id,
            FullName = src.FullName,
        };
        public static Expression<Func<Contact, GetContactByIdResponse>> ProjectToGetContactByIdResponse() => src => new ()
        {
            FirstName = src.FirstName,
            LastName = src.LastName,
            PhoneNumber = src.WAAccount.PhoneNumber,
        };
        public static Contact MapFrom(CreateContactRequest dto, WAAccount account, string createdBy, DateTimeOffset createdAt) 
            => new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            CreatedBy = createdBy,
            CreatedAt = createdAt,
            WAAcountId = account.Id,
            WAAccount = account,
        };
        public Contact MapFrom(CreateContactRequest dto, string accountId)
        {
            src.FirstName = dto.FirstName;
            src.LastName = dto.LastName;
            src.WAAcountId = accountId;
            return src;
        }
        public Contact MapFrom(UpdateContactRequest dto)
        {
            src.FirstName = dto.FirstName;
            src.LastName = dto.LastName;
            return src;
        }
    }
}
