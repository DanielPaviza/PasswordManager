using Avalonia.Data.Converters;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PasswordManager.Converters;

public class TranslationMultiConverter : IMultiValueConverter {

    private static ILanguageService? _service;
    public static ILanguageService? LanguageService => _service;

    public static void Configure(ILanguageService service) => _service = service;

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) {

        var keyCandidate = values.Count > 0 ? values[0] : null;

        if (keyCandidate is string key && _service is not null)
            return _service.Translate(key);

        return keyCandidate ?? string.Empty;
    }

    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
