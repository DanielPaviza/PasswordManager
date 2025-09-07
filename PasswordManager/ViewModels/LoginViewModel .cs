using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels;

public partial class LoginViewModel : ViewModelBase, INamedViewModel {

    private static readonly Random Random = new();

    private readonly ILogService _logService;
    private readonly IEncryptionService _encyptionService;

    public bool IncludeInNavStack => false;
    public string Title => "Login";

    [ObservableProperty]
    private string _inputPassword = string.Empty;

    [ObservableProperty]
    private bool _renderInput;

    [ObservableProperty]
    private ObservableCollection<string> _decoyMessages = [];

    [ObservableProperty]
    private bool _isLoggedIn = false;

    public IRelayCommand LoginCommand { get; }

    private readonly List<string> decoyMessagesAll = [
        "[ERROR] Failed to initialize module: SyncService",
        "[WARN ] Skipping invalid config line: 0x1A3",
        "[FATAL] Unhandled exception in ProcessMonitor thread",
        "[ERROR] Network adapter reset failed",
        "[WARN ] Configuration file checksum mismatch",
        "[ERROR] Access violation at address 0x7ffeefbff6",
        "[ERROR] Exiting",
    ];

    public LoginViewModel(ILogService logService, IEncryptionService encyptionService) {
        _logService = logService;
        _encyptionService = encyptionService;
        LoginCommand = new RelayCommand(Login);
        RenderDecoyMessages();
        _logService.LogInfo("LoginViewModel initialized");
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

        if (_encyptionService.VerifyMasterPassword(InputPassword)) {
            IsLoggedIn = true;
            return;
        }

        _logService.LogWarning("Login failed - Incorrect master password.");
        InputPassword = "";
    }
}
