using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.Services;

namespace PasswordManager.ViewModels;

public partial class TopBarViewModel : ViewModelBase {

    private readonly INavigationService _nav;
    private readonly CredentialListViewModel _credentialsListViewModel;
    private readonly CredentialFormViewModel _credentialAddViewModel;
    private readonly ILogService _logService;

    [ObservableProperty]
    private LogViewModel _logViewModel;

    public static bool IncludeInNavStack => false;

    [ObservableProperty]
    public string _title = "";

    [NotifyCanExecuteChangedFor(nameof(NavigateToAddCredentialCommand))]
    [ObservableProperty]
    public bool _isCredentialsListTabSelected;

    [NotifyCanExecuteChangedFor(nameof(NavigateToCredentialListCommand))]
    [ObservableProperty]
    public bool _isCredentialAddTabSelected;

    public TopBarViewModel(
        INavigationService Nav,
        CredentialListViewModel CredentialsListViewModel,
        CredentialFormViewModel CredentialAddViewModel,
        LogViewModel __logViewModel,
        ILogService logService
        ) {

        _nav = Nav;
        _credentialsListViewModel = CredentialsListViewModel;
        _credentialAddViewModel = CredentialAddViewModel;
        LogViewModel = __logViewModel;
        _logService = logService;

        // Title initialization
        if (Nav.CurrentView != null) {
            UpdateTabSelection();
            Title = Nav.CurrentView.Title;
        }

        if (Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    UpdateTabSelection();
                    Title = navImpl.CurrentView!.Title;
                    NavigateBackCommand.NotifyCanExecuteChanged();
                }
            };
        }


        _logService.Log("TopBarViewModel initialized");
    }

    private bool CanNavigateBack() => _nav.CanNavigateBack();

    [RelayCommand(CanExecute = nameof(CanNavigateBack))]
    private void NavigateBack() {
        _nav.NavigateBack();
    }

    [RelayCommand]
    private void NavigateToCredentialList() {
        if (_nav.CurrentView != _credentialsListViewModel) {
            _nav.NavigateTo(_credentialsListViewModel);
        }
    }

    [RelayCommand]
    private void NavigateToAddCredential() {
        if (_nav.CurrentView != _credentialAddViewModel) {
            _nav.NavigateTo(_credentialAddViewModel);
        }
    }

    private void UpdateTabSelection() {
        IsCredentialsListTabSelected = _nav.CurrentView == _credentialsListViewModel;
        IsCredentialAddTabSelected = _nav.CurrentView is CredentialFormViewModel form && !form.IsEditMode;
    }
}
