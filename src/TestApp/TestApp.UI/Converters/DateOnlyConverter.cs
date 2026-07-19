using System.Globalization;
using System.Windows.Data;

namespace TestApp.UI.Converters;

public sealed class DateOnlyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return null;

        return ((DateOnly)value).ToDateTime(TimeOnly.MinValue);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return null;

        return DateOnly.FromDateTime((DateTime)value);
    }
}
