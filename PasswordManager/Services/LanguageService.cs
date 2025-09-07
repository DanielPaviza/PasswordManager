using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Enums;
using PasswordManager.Extensions;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace PasswordManager.Services;

public partial class LanguageService : ObservableObject, ILanguageService {

    private readonly ILogService _logService;
    private readonly ResourceManager _resourceManager;
    public string this[string key] => Translate(key);

    [ObservableProperty]
    private AppLanguageEnum _currentLanguage;

    public static List<AppLanguageEnum> AllLanguages => [
        AppLanguageEnum.English,
        AppLanguageEnum.Czech
    ];

    public LanguageService(ILogService logService) {
        _logService = logService;
        _resourceManager = new ResourceManager("PasswordManager.Resources.Translations.Translations", Assembly.GetExecutingAssembly());
        SetLanguage(AppLanguageEnum.Czech); // default
    }

    public void SetLanguage(AppLanguageEnum language) {
        var culture = new CultureInfo(language.ToCultureCode());
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        OnPropertyChanged("Translate");
        CurrentLanguage = language;
        _logService.LogInfo($"Language changed to {CurrentLanguage.ToDisplayName()} ({culture.Name})");
    }

    public string Translate(string key) {
        key = key.Trim();
        if (string.IsNullOrWhiteSpace(key))
            return key;

        // Split on | if multiple keys
        var keys = key.Split('|', StringSplitOptions.RemoveEmptyEntries);

        var translations = keys
            .Select(k => _resourceManager.GetString(k.ToLower()) ?? k)
            .ToArray();

        return string.Join(" ", translations);
    }
}
