
using PasswordManager.Enums;
using System;

namespace PasswordManager.Extensions;

public static class AppLanguageExtensions {
    public static string ToCultureCode(this AppLanguageEnum lang) => lang switch {
        AppLanguageEnum.English => "en",
        AppLanguageEnum.Czech => "cs-CZ",
        _ => "cs-CZ"
    };

    public static string ToDisplayName(this AppLanguageEnum lang) => lang switch {
        AppLanguageEnum.English => "English",
        AppLanguageEnum.Czech => "Česky",
        _ => "Unknown"
    };

    public static AppLanguageEnum FromDisplayName(string lang) => lang switch {
        "English" => AppLanguageEnum.English,
        "Česky" => AppLanguageEnum.Czech,
        _ => throw new ArgumentException($"Unknown language: {lang}", nameof(lang))
    };
}
