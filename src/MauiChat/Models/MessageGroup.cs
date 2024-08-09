namespace MauiChat.Models;

public class MessageGroup : ObservableCollection<MessageItem>
{
    public string Name { get; private set; }

    public MessageGroup(string name, List<MessageItem> messages) : base(messages)
    {
        Name = name;
    }
}
