using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class ValidationStringToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

        var fallbackBrush = Brushes.Black;
        if (parameter is string colorString) {
            try {
                var color = Color.Parse(colorString);
                fallbackBrush = new ImmutableSolidColorBrush(color);
            } catch { /* Ignore and keep fallback as black */ }
        }

        if (value is string str && !string.IsNullOrWhiteSpace(str))
            return Brushes.Red;

        return fallbackBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}