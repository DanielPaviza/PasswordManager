using PasswordManager.Enums;
using System.Collections.Generic;

namespace PasswordManager.Interfaces;

public interface ILanguageService {
    string this[string key] => Translate(key);
    AppLanguageEnum CurrentLanguage { get; }
    static List<string> AllLanguages { get; } = [];
    void SetLanguage(AppLanguageEnum language);
    string Translate(string key);
}
