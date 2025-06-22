using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class TabSelectionBrushConverter : IValueConverter {
    private static readonly IBrush Selected = new SolidColorBrush(Color.Parse("#2c7af5"));
    private static readonly IBrush Unselected = Brushes.Transparent;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool selected && selected ? Selected : Unselected;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException("ConvertBack is not supported.");
}
