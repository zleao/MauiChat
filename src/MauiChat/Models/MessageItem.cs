namespace MauiChat.Models;

public class MessageItem
{
    public string? Body { get; set; }

    public DateTime Created { get; set; }

    public bool IsMyMessage { get; set; }

    public List<MediaItem> Attachments { get; set; } = [];
}
