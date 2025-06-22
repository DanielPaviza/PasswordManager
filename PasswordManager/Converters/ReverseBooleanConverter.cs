using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class ReverseBooleanConverter : IValueConverter {

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool boolVal && !boolVal;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException("ConvertBack is not supported.");
}
