using Avalonia.Controls;
using PasswordManager.ViewModels;

namespace PasswordManager.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        var vm = new MainWindowViewModel();
        DataContext = vm;

        // Předplatit se na změny ve ViewModelu
        vm.PropertyChanged += Vm_PropertyChanged;

        // Inicialní nastavení velikosti okna podle VM
        this.Width = vm.WindowWidth;
        this.Height = vm.WindowHeight;
        this.CanResize = vm.WindowCanResize;
        this.Background = vm.WindowBgColor;
    }

    private void Vm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
        if (sender is MainWindowViewModel vm) {
            if (e.PropertyName == nameof(MainWindowViewModel.WindowWidth))
                this.Width = vm.WindowWidth;

            else if (e.PropertyName == nameof(MainWindowViewModel.WindowHeight))
                this.Height = vm.WindowHeight;

            else if (e.PropertyName == nameof(MainWindowViewModel.WindowCanResize))
                this.CanResize = vm.WindowCanResize;

            else if (e.PropertyName == nameof(MainWindowViewModel.WindowBgColor))
                this.Background = vm.WindowBgColor;
        }
    }
}
