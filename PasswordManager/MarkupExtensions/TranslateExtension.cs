using Avalonia.Data;
using Avalonia.Markup.Xaml;
using PasswordManager.Converters;
using PasswordManager.Interfaces;
using System;
using System.ComponentModel;

namespace PasswordManager.MarkupExtensions;

public class TranslateExtension(object key) : MarkupExtension {

    public object? Key { get; set; } = key;

    public override object ProvideValue(IServiceProvider serviceProvider) {

        if (Key is IBinding binding) {
            // Wrap original binding with a converter
            return new MultiBinding {
                Bindings =
                     {
                binding, // original binding (e.g. Title)
                new Binding
                {
                    // bind to CurrentLanguage
                    Source = TranslationMultiConverter.LanguageService,
                    Path = nameof(ILanguageService.CurrentLanguage)
                }
            },
                Converter = new TranslationMultiConverter()
            };
        }

        if (Key is not string key) return "";

        if (string.IsNullOrEmpty(key))
            return Key;

        if (App.Services?.GetService(typeof(ILanguageService)) is not ILanguageService langService)
            return Key;

        var proxy = new TranslationProxy(langService, key);
        return new Binding {
            Source = proxy,
            Path = "Value",
            Mode = BindingMode.OneWay
        };
    }

    private class TranslationProxy : INotifyPropertyChanged {
        private readonly ILanguageService _langService;
        private readonly string _key;
        private string _value = string.Empty;

        public string Value {
            get => _value;
            private set {
                if (_value == value) return;
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public TranslationProxy(ILanguageService langService, string key) {
            _langService = langService;
            _key = key;
            Value = _langService.Translate(_key);

            if (_langService is INotifyPropertyChanged npc)
                npc.PropertyChanged += OnLanguageServicePropertyChanged;
        }

        private void OnLanguageServicePropertyChanged(object? sender, PropertyChangedEventArgs e) {
            // refresh on any relevant change
            Value = _langService.Translate(_key);
        }
    }
}