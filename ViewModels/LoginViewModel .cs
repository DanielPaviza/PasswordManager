using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels;

public partial class LoginViewModel : ViewModelBase {

    private static readonly Random Random = new();

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

    public LoginViewModel(Action onLoginSuccess) {
        _onLoginSuccess = onLoginSuccess;
        LoginCommand = new RelayCommand(Login);
        RenderDecoyMessages();
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

        InputPassword = "";
    }
}
