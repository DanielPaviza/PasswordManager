using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views;

public partial class TopBarView : UserControl {
    public TopBarView() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
