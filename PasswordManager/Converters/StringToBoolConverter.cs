using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class StringToBoolConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is string val && val.Length > 0;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}