using Avalonia.Data.Converters;
using Material.Icons;
using System;
using System.Globalization;

namespace PasswordManager.Converters;

public class PasswordShowIconConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? MaterialIconKind.EyeOff : MaterialIconKind.Eye;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException("ConvertBack is not supported.");
}
