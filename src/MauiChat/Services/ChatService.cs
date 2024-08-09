namespace MauiChat.Services;

public class ChatService
{
    public static IEnumerable<MessageItem> GetInitialMessages()
	{
        return
        [
            new() {
                Body = "Hello!",
                Created = DateTime.Now.AddDays(-10),
                IsMyMessage = false
            },
            new() {
                Body = "I'm an echo chat. Write something and I'll echo it back",
                Created = DateTime.Now.AddDays(-1),
                IsMyMessage = false
            }
        ];
	}

    public static MessageItem SendMessage(string messageBody, ObservableCollection<MediaItem> messageAttachments)
    {
        var message = new MessageItem
        {
            Body = messageBody,
            Created = DateTime.Now,
            IsMyMessage = true
        };

        if (messageAttachments.Any())
        {
            message.Attachments.AddRange(messageAttachments);
        }

        return message;
    }

    public static MessageItem GetEchoMessage(MessageItem originalMessage)
    {
        return new MessageItem
        {
            Body = "Echo - " + originalMessage.Body,
            Created = DateTime.Now,
            IsMyMessage = false,
            Attachments = originalMessage.Attachments
        };
    }
}
