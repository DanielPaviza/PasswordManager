using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using System.Collections.Generic;

namespace PasswordManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase, INavigationService {

    [ObservableProperty]
    private ViewModelBase currentView;

    private Stack<ViewModelBase> _viewStack = new();

    [ObservableProperty]
    private int _windowWidth = 800;

    [ObservableProperty]
    private int _windowHeight = 400;

    [ObservableProperty]
    private bool _windowCanResize = false;

    [ObservableProperty]
    private Avalonia.Media.IImmutableSolidColorBrush _windowBgColor = Avalonia.Media.Brushes.Black;

    private readonly CredentialsListViewModel CredentialsListViewModel;

    public MainWindowViewModel() {

        CurrentView = new LoginViewModel(OnLoginSuccess);
        CredentialsListViewModel = new CredentialsListViewModel(this);
    }

    public void NavigateTo(ViewModelBase viewModel) {
        _viewStack.Push(CurrentView);
        CurrentView = viewModel;
    }

    public void NavigateBack() {
        if (_viewStack.Count > 0)
            CurrentView = _viewStack.Pop();
    }

    private void OnLoginSuccess() {
        // Po přihlášení zvětšit okno
        WindowWidth = 1200;
        WindowHeight = 800;
        WindowCanResize = true;
        WindowBgColor = Avalonia.Media.Brushes.White;

        CurrentView = CredentialsListViewModel;
    }
}
