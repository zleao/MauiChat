namespace MauiChat.Views;

public class MessageTemplateSelector : DataTemplateSelector
{
    public DataTemplate MessageSent { get; set; } = null!;
    public DataTemplate MessageReceived { get; set; } = null!;


    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((MessageItem)item).IsMyMessage ? MessageSent : MessageReceived;
    }
}
