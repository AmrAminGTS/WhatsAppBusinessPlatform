namespace WhatsAppBusinessPlatform.Domain.Entities.Messages;

public enum MessageContentType
{
    Unknown,
    Address,
    Audio,
    Contacts,
    Document,
    Image,
    Location,
    Reaction,
    Sticker,
    Text,
    Video,
        // Not yet supported
        InteractiveURLButton,
        InteractiveFlow,
        InteractiveList,
        InteractiveLocationRequest,
        InteractiveReplyButtons,
        Template,
}
