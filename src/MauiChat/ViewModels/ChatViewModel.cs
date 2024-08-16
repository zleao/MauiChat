using zoft.MauiExtensions.Core.Extensions;

namespace MauiChat.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private ObservableCollection<object> _groupedMessages = [];

    [ObservableProperty]
    private bool _isSendingMessage;
    partial void OnIsSendingMessageChanged(bool value) => SendMessageCommand.NotifyCanExecuteChanged();

    [ObservableProperty]
    private bool _isTyping;

    [ObservableProperty]
    private string _messageBody = string.Empty;
    partial void OnMessageBodyChanged(string value) => SendMessageCommand.NotifyCanExecuteChanged();

    [ObservableProperty]
    private ObservableCollection<MediaItem> _messageAttachments = [];
    partial void OnMessageAttachmentsChanged(ObservableCollection<MediaItem> value) => SendMessageCommand.NotifyCanExecuteChanged();

    public void LoadInitialMessages()
    {
        var messages = ChatService.GetInitialMessages();

        var groupedMessages = messages.GroupBy(m => m.Created.Date)
                                      .OrderBy(g => g.Key);

        var messagesToShow = new ObservableCollection<object>();

        foreach (var group in groupedMessages)
        {
            messagesToShow.Add(new MessageGroup(GetGroupHeaderName(group.Key)));
            messagesToShow.AddRange(group.Select(m => m));
        }

        GroupedMessages = messagesToShow;
    }

    private static string GetGroupHeaderName(DateTime value)
    {
        if (value is DateTime date)
        {
            var dayDiff = DateTime.UtcNow.Day - date.Day;
            string groupHeader;

            if (dayDiff == 0)
            {
                groupHeader = "TODAY";
            }
            else if (dayDiff == 1)
            {
                groupHeader = "YESTERDAY";
            }
            else
            {
                groupHeader = date.ToString("dd-MM-yyyy");
            }

            return groupHeader;
        }

        return null;
    }

    [RelayCommand]
    public async Task TakePhoto()
    {
        var photo = await MediaPicker.CapturePhotoAsync();

        if (photo is not null)
        {
            MessageAttachments.Add(new MediaItem { Source = photo.FullPath });
        }
    }

    [RelayCommand]
    public async Task PickPhoto()
    {
        var photo = await MediaPicker.PickPhotoAsync();

        if (photo is not null)
        {
            MessageAttachments.Add(new MediaItem { Source = photo.FullPath });
        }
    }

    [RelayCommand(CanExecute = nameof(CanSendMessage))]    public async Task SendMessage()
    {
        // Check if the message can be sent
        if (!CanSendMessage)
        {
            return;
        }

        try
        {
            // Indicate that a message is being sent
            IsSendingMessage = true;

            // Send the message using the ChatService
            var newMessage = ChatService.SendMessage(MessageBody, MessageAttachments);

            // Add the new message to the list of grouped messages
            AddMessageToList(newMessage);

            // Simulate a delay to mimic message echo
            await Task.Delay(1000);

            // Get the echo message from the ChatService
            var newEchoMessage = ChatService.GetEchoMessage(newMessage);

            // Add the echo message to the list of grouped messages
            AddMessageToList(newEchoMessage);

            // Clear the message body after sending
            MessageBody = string.Empty;

            // Clear the message attachments after sending
            MessageAttachments.Clear();
        }
        finally
        {
            // Indicate that the message sending process is complete
            IsSendingMessage = false;
        }
    }
    public bool CanSendMessage => !IsSendingMessage && (!string.IsNullOrWhiteSpace(MessageBody) || MessageAttachments.Any());

    [RelayCommand]
    public void RemoveAttachment(MediaItem attachment)
    {
        MessageAttachments.Remove(attachment);
    }

    /// <summary>
    /// Adds a new message to the list of grouped messages.
    /// </summary>
    /// <param name="newMessage">The new message to add.</param>
    private void AddMessageToList(MessageItem newMessage)
    {
        if (GroupedMessages.LastOrDefault(m => m is MessageGroup) is not MessageGroup lastGroup ||
                lastGroup.Name != GetGroupHeaderName(newMessage.Created))
        {
            GroupedMessages.Add(new MessageGroup(GetGroupHeaderName(newMessage.Created)));
        }

        GroupedMessages.Add(newMessage);
    }

    public ChatViewModel()
    {
        _messageAttachments.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(MessageAttachments));
            SendMessageCommand.NotifyCanExecuteChanged();
        };
    }
}
