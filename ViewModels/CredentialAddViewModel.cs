using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Services;

namespace PasswordManager.ViewModels;

public partial class CredentialAddViewModel : ViewModelBase, INamedViewModel {

    private readonly INavigationService Nav;
    private readonly CredentialsListViewModel CredentialsListViewModel;

    [ObservableProperty]
    private Credential _newCredential = new();

    [ObservableProperty]
    private bool _showPassword = false;

    public bool IncludeInNavStack => true;
    public string Title => "Add new credentials";


    public CredentialAddViewModel(INavigationService _nav, CredentialsListViewModel _credentialsListViewModel) {
        Nav = _nav;
        CredentialsListViewModel = _credentialsListViewModel;

        // Reset data after view change
        if (Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    NewCredential = new();
                }
            };
        }
    }

    [RelayCommand]
    private void NavigateToVault() {
        Nav.NavigateTo(CredentialsListViewModel);
    }

    [RelayCommand]
    private void SaveCredential() { /* uložení credentialu */ }
}
