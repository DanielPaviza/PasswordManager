using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class PasswordCharConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    => (bool)value ? '\0' : '*';

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException("ConvertBack is not supported.");
}
