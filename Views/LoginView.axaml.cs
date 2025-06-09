using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace PasswordManager.Views;

public partial class LoginView : UserControl {
    public LoginView() {
        InitializeComponent();

        this.AttachedToVisualTree += OnAttachedToVisualTree;
        //var input = this.FindControl<TextBox>("HiddenInput");
        //if (input.IsEffectivelyEnabled) {
        //    input.Focus();
        //}
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e) {
        Dispatcher.UIThread.Post(() => {
            var input = this.FindControl<TextBox>("HiddenInput");
            if (input?.IsVisible == true && input.IsEffectivelyEnabled) {
                input.Focus();
            }
        }, DispatcherPriority.Background);
    }
}
