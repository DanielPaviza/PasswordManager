using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Enums;
using PasswordManager.Interfaces;
using PasswordManager.Services;

namespace PasswordManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase {

    [ObservableProperty]
    private INamedViewModel? _currentView;

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

    public MainWindowViewModel(
        INavigationService _nav,
        ILogService _logService,
        CredentialListViewModel _credentialsListViewModel,
        TopBarViewModel _topBarViewModel,
        LogViewModel _logViewModel
        ) {

        Nav = _nav;
        CredentialsListViewModel = _credentialsListViewModel;
        LogService = _logService;
        TopBarViewModel = _topBarViewModel;
        LogViewModel = _logViewModel;
        LoginViewModel = new LoginViewModel(OnLoginSuccess, _logService);

        if (Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    CurrentView = navImpl.CurrentView;
                }
            };
        }

        Nav.NavigateTo(LoginViewModel);

#if DEBUG
        LogService.Log("Debug enviroment detected - Skipping login.", LogSeverityEnum.Info);
        OnLoginSuccess();
#endif

        _logService.Log("MainWindowViewModel initialized");
    }

    private void OnLoginSuccess() {
        // Po přihlášení zvětšit okno
        WindowWidth = 1200;
        WindowHeight = 800;
        WindowCanResize = true;
        WindowBgColor = Avalonia.Media.Brushes.White;
        WindowTitle = "Password Manager";
        IsLoggedIn = true;
        LogService.Log("Successfully logged in", LogSeverityEnum.Info);
        // Default show of log view in debug mode
#if DEBUG
        LogViewModel.IsVisible = true;
#endif

        Nav.NavigateTo(CredentialsListViewModel);
    }
}
