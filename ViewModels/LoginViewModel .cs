using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace PasswordManager.ViewModels;

public partial class LoginViewModel : ViewModelBase {

    private string _masterPassword = "admin";

    [ObservableProperty]
    private string inputPassword = string.Empty;

    [ObservableProperty]
    private string? validationMessage;

    public IRelayCommand LoginCommand { get; }

    private readonly Action _onLoginSuccess;

    public LoginViewModel(Action onLoginSuccess) {
        _onLoginSuccess = onLoginSuccess;
        LoginCommand = new RelayCommand(Login);
    }

    private void Login() {

        if (string.IsNullOrWhiteSpace(InputPassword)) {
            ValidationMessage = "Please enter your master password.";
            return;
        }

        if (InputPassword == _masterPassword) {
            ValidationMessage = string.Empty;
            _onLoginSuccess.Invoke();
        } else {
            ValidationMessage = "Invalid password.";
        }
    }
}
