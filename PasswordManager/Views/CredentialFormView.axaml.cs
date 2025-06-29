using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views;

public partial class CredentialFormView : UserControl {
    public CredentialFormView() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
