using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class TabSelectionFontWeightConverter : IValueConverter {
    private static readonly FontWeight Selected = FontWeight.Bold;
    private static readonly FontWeight Unselected = FontWeight.Bold;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool selected && selected ? Selected : Unselected;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException("ConvertBack is not supported.");
}
