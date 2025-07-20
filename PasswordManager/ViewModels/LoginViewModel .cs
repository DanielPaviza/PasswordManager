using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Enums;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels;

public partial class LoginViewModel : ViewModelBase, INamedViewModel {

    private static readonly Random Random = new();

    private readonly ILogService _logService;

    public bool IncludeInNavStack => false;
    public string Title => "Login";

    [ObservableProperty]
    private string _inputPassword = string.Empty;

    [ObservableProperty]
    private bool _renderInput;

    [ObservableProperty]
    private ObservableCollection<string> _decoyMessages = [];

    public IRelayCommand LoginCommand { get; }

    private readonly Action _onLoginSuccess;

    private readonly List<string> decoyMessagesAll = [
        "[ERROR] Failed to initialize module: SyncService",
        "[WARN ] Skipping invalid config line: 0x1A3",
        "[FATAL] Unhandled exception in ProcessMonitor thread",
        "[ERROR] Network adapter reset failed",
        "[WARN ] Configuration file checksum mismatch",
        "[ERROR] Access violation at address 0x7ffeefbff6",
        "[ERROR] Exiting",
    ];

    public LoginViewModel(Action onLoginSuccess, ILogService logService) {
        _logService = logService;
        _onLoginSuccess = onLoginSuccess;
        LoginCommand = new RelayCommand(Login);
        RenderDecoyMessages();
        _logService.Log("LoginViewModel initialized");
    }

    private async void RenderDecoyMessages() {

        DecoyMessages = [];
        foreach (var message in decoyMessagesAll) {
            await Task.Delay(Random.Next(5, 61));
            DecoyMessages.Add(message);
        }

        RenderInput = true;
    }

    private void Login() {

        if (EncryptionService.VerifyMasterPassword(InputPassword)) {
            _onLoginSuccess.Invoke();
            return;
        }

        _logService.Log("Login failed - Incorrect master password.", LogSeverityEnum.Warning);
        InputPassword = "";
    }
}
