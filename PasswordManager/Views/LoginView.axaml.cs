using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using PasswordManager.ViewModels;

namespace PasswordManager.Views;

public partial class LoginView : UserControl {

    private readonly TextBox? _passwordBox;

    public LoginView() {
        InitializeComponent();
        _passwordBox = this.FindControl<TextBox>("passwordInput");
        _passwordBox!.AttachedToVisualTree += (_, __) => {
            _passwordBox.Focus();
            Dispatcher.UIThread.Post(() => {
                _passwordBox.FocusAdorner = null;
            });
        };
    }

    private void PasswordInput_KeyDown(object? sender, KeyEventArgs e) {

        if (DataContext is LoginViewModel vm) {
            if (e.Key == Key.Enter && vm.LoginCommand.CanExecute(null))
                vm.LoginCommand.Execute(null); ;
        }
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
