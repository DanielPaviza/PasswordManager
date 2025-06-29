using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views;

public partial class CredentialListView : UserControl {
    public CredentialListView() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
