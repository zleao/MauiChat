namespace MauiChat;
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("MaterialIcons.ttf", "MaterialIcons");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureMauiHandlers(d => 
			{
                //Remove bottom line and border from Editor
                Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
                {
                    if (view is not Editor)
                    {
                        return;
                    }

#if ANDROID
                    handler.PlatformView.SetPadding(11, 30, 11, 30);
                    handler.PlatformView.BackgroundTintList
                        = Android.Content.Res.ColorStateList.ValueOf(view.Background.ToColor()?.ToAndroid() ?? Android.Graphics.Color.Transparent);

                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        handler.PlatformView.TextCursorDrawable?.SetTint(Colors.Black.ToAndroid());
#pragma warning restore CA1416 // Validate platform compatibility
                    }
#elif IOS
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.Layer.BorderWidth = 0;
                    handler.PlatformView.TintColor = UIKit.UIColor.Black;
                    if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(17, 0))
                    {
                        handler.PlatformView.BorderStyle = UIKit.UITextViewBorderStyle.None;
                    }
#endif
                });

            });

		builder.Services.AddSingleton<ChatViewModel>();
		builder.Services.AddSingleton<ChatPage>();

		return builder.Build();
	}
}
