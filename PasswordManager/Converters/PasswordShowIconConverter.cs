using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class PasswordShowIconConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

        bool showPassword = (bool)value;
        var geometryKey = showPassword ? "eye_show_regular" : "eye_hide_regular";
        var geometry = App.Current!.Resources.TryGetResource(geometryKey, null, out var icon) ? icon : null;

        if (geometry != null)
            return geometry;

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException("ConvertBack is not supported.");
}
