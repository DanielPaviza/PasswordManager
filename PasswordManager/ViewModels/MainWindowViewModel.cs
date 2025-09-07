using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.Services;

namespace PasswordManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase {

    [ObservableProperty]
    private INamedViewModel _currentView;

    [ObservableProperty]
    private bool _isLoggedIn = false;

    private readonly ILogService LogService;

    private readonly INavigationService Nav;

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

    public LogViewModel LogViewModel { get; }
    public TopBarViewModel TopBarViewModel { get; }

    private readonly CredentialListViewModel CredentialsListViewModel;
    private readonly LoginViewModel LoginViewModel;

    public MainWindowViewModel() { }

    public MainWindowViewModel(
        INavigationService _nav,
        ILogService _logService,
        LoginViewModel _loginViewModel,
        TopBarViewModel _topBarViewModel,
        CredentialListViewModel _credentialsListViewModel,
        LogViewModel _logViewModel
        ) {

        Nav = _nav;
        CurrentView = _nav.CurrentView;
        CredentialsListViewModel = _credentialsListViewModel;
        LogService = _logService;
        TopBarViewModel = _topBarViewModel;
        LogViewModel = _logViewModel;
        LoginViewModel = _loginViewModel;

        if (Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    CurrentView = navImpl.CurrentView;
                }
            };
        }

        LoginViewModel.PropertyChanged += (s, e) => {
            if (e.PropertyName == nameof(LoginViewModel.IsLoggedIn) && LoginViewModel.IsLoggedIn) {
                OnLoginSuccess();
            }
        };

#if DEBUG
        LogService.LogInfo("Debug enviroment detected - Skipping login.");
        OnLoginSuccess();
#endif

        _logService.LogInfo("MainWindowViewModel initialized");
    }

    private void OnLoginSuccess() {
        // Po přihlášení zvětšit okno
        WindowWidth = 1200;
        WindowHeight = 800;
        WindowCanResize = true;
        WindowBgColor = Avalonia.Media.Brushes.White;
        WindowTitle = "Password Manager";
        IsLoggedIn = true;
        LogService.LogSuccess("Successfully logged in");
        // Default show of log view in debug mode
#if DEBUG
        LogViewModel.IsVisible = true;
#endif

        Nav.NavigateTo(CredentialsListViewModel);
    }
}
