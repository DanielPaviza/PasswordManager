using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.Services;

namespace PasswordManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase {

    [ObservableProperty]
    private INamedViewModel? _currentView;

    [ObservableProperty]
    private bool _isLoggedIn = false;

    public INavigationService Nav { get; }

    [ObservableProperty]
    private string _windowTitle = "hpagent";

    [ObservableProperty]
    private int _windowWidth = 800;

    [ObservableProperty]
    private int _windowHeight = 400;

    [ObservableProperty]
    private bool _windowCanResize = false;

    [ObservableProperty]
    private Avalonia.Media.IImmutableSolidColorBrush _windowBgColor = Avalonia.Media.Brushes.Black;

    public TopBarViewModel TopBarViewModel { get; }
    private readonly CredentialsListViewModel CredentialsListViewModel;
    private readonly LoginViewModel LoginViewModel;

    public MainWindowViewModel(INavigationService nav, CredentialsListViewModel _credentialsListViewModel, TopBarViewModel _topBarViewModel) {

        Nav = nav;
        CredentialsListViewModel = _credentialsListViewModel;
        TopBarViewModel = _topBarViewModel;
        LoginViewModel = new LoginViewModel(OnLoginSuccess);

        if (Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    CurrentView = navImpl.CurrentView;
                }
            };
        }

        Nav.NavigateTo(LoginViewModel);

        // skip login for debugging purposes
        OnLoginSuccess();
    }

    private void OnLoginSuccess() {
        // Po přihlášení zvětšit okno
        WindowWidth = 1200;
        WindowHeight = 800;
        WindowCanResize = true;
        WindowBgColor = Avalonia.Media.Brushes.White;
        WindowTitle = "Password Manager";
        IsLoggedIn = true;

        Nav.NavigateTo(CredentialsListViewModel);
    }
}
