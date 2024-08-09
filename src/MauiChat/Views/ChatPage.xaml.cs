using CommunityToolkit.Mvvm.Messaging;

namespace MauiChat.Views;

public partial class ChatPage : ContentPage
{
    private readonly ChatViewModel ViewModel;

	public ChatPage(ChatViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = ViewModel = viewModel;

        MessagesList.Scrolled += MessagesList_Scrolled;

        ViewModel.LoadInitialMessages();
    }

    private void MessagesList_Scrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        ButtonScrollToBottom.IsVisible = e.LastVisibleItemIndex != ViewModel.GroupedMessagesIndexCount;
    }

    private void ButtonScrollToBottom_Clicked(object sender, EventArgs e)
    {
        MessagesList.ScrollTo(ViewModel.GroupedMessagesIndexCount);
    }

    private void EntryFocused(object sender, FocusEventArgs e)
    {
		ViewModel.IsTyping = true;
    }

    private void EntryUnfocused(object sender, FocusEventArgs e)
    {
		ViewModel.IsTyping = false;
    }
}
