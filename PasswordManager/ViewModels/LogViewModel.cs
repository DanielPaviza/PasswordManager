using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using System;
using System.Linq;

namespace PasswordManager.ViewModels;

public partial class LogViewModel : ViewModelBase {

    private readonly ILogService _logService;

    [ObservableProperty]
    private bool _isVisible = false;

    [ObservableProperty]
    private bool _isOpen = false;

    [ObservableProperty]
    private string _textLogs = "";

    public LogViewModel(ILogService logService) {

        _logService = logService;
        _logService.Logs.CollectionChanged += (s, e) => {
            TextLogs = GetTextLogs();
        };

        _logService.Log("LogViewModel initialized");
    }

    private string GetTextLogs() => string.Join(Environment.NewLine, _logService.Logs.Reverse());

    [RelayCommand]
    public void CompletelyClose() {
        IsOpen = false;
        IsVisible = false;
    }

    [RelayCommand]
    public void ToggleOpen() => IsOpen = !IsOpen;

    [RelayCommand]
    public void ToggleVisibility() {
        _logService.Log($"Toggling log view visibility to {!IsVisible}");
        if (IsVisible) {
            CompletelyClose();
        } else {
            IsVisible = true;
        }
    }
}
