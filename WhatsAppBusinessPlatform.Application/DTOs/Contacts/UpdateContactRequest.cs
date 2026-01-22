namespace WhatsAppBusinessPlatform.Application.DTOs.Contacts;
public sealed class UpdateContactRequest
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
}
