using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.Services;

namespace PasswordManager.ViewModels;

public partial class TopBarViewModel : ViewModelBase {

    private readonly INavigationService Nav;
    private readonly CredentialListViewModel CredentialsListViewModel;
    private readonly CredentialFormViewModel CredentialAddViewModel;

    public static bool IncludeInNavStack => false;

    [ObservableProperty]
    public string _title = "";

    [NotifyCanExecuteChangedFor(nameof(NavigateToAddCredentialCommand))]
    [ObservableProperty]
    public bool _isCredentialsListTabSelected;

    [NotifyCanExecuteChangedFor(nameof(NavigateToCredentialListCommand))]
    [ObservableProperty]
    public bool _isCredentialAddTabSelected;

    public TopBarViewModel(INavigationService _nav, CredentialListViewModel _credentialsListViewModel, CredentialFormViewModel _credentialAddViewModel) {

        Nav = _nav;
        CredentialsListViewModel = _credentialsListViewModel;
        CredentialAddViewModel = _credentialAddViewModel;

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

    }

    private bool CanNavigateBack() => Nav.CanNavigateBack();

    [RelayCommand(CanExecute = nameof(CanNavigateBack))]
    private void NavigateBack() {
        Nav.NavigateBack();
    }

    [RelayCommand]
    private void NavigateToCredentialList() {
        if (Nav.CurrentView != CredentialsListViewModel) {
            Nav.NavigateTo(CredentialsListViewModel);
        }
    }

    [RelayCommand]
    private void NavigateToAddCredential() {
        if (Nav.CurrentView != CredentialAddViewModel) {
            Nav.NavigateTo(CredentialAddViewModel);
        }
    }

    private void UpdateTabSelection() {
        IsCredentialsListTabSelected = Nav.CurrentView == CredentialsListViewModel;
        IsCredentialAddTabSelected = Nav.CurrentView is CredentialFormViewModel form && !form.IsEditMode;
    }
}
