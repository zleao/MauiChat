
namespace MauiChat.Converters;

public class MessageDateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var date = value as DateTime?;
        if (date is null)
        {
            return string.Empty;
        }

        return ((DateTime)date).ToLocalTime().ToString("HH:mm");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
