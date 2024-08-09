namespace MauiChat.Views;

public class MessageTemplateSelector : DataTemplateSelector
{
    public DataTemplate GroupHeader { get; set; } = null!;

    public DataTemplate MessageSent { get; set; } = null!;
    
    public DataTemplate MessageReceived { get; set; } = null!;


    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is MessageGroup)
        {
            return GroupHeader;
        }

        return ((MessageItem)item).IsMyMessage ? MessageSent : MessageReceived;
    }
}
