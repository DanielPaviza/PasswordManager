using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PasswordManager.ViewModels;

public partial class CredentialFormViewModel : ViewModelBase, INamedViewModel {

    private readonly INavigationService Nav;
    private readonly CredentialListViewModel CredentialListViewModel;

    [ObservableProperty]
    private CredentialModel _credential;

    [ObservableProperty]
    private bool _showPassword = false;

    public bool IsEditMode { get; }

    public bool IncludeInNavStack => true;
    public string Title => IsEditMode ? "Edit Credential" : "Add Credential";

    //[ObservableProperty]
    //public List<PropertyValidationModel> _validationErrors = [];

    [ObservableProperty]
    public Dictionary<string, string> _validationErrors = [];

    [ObservableProperty]
    public string _serviceNameError = "";

    [ObservableProperty]
    public string _passwordError = "";

    public CredentialFormViewModel(INavigationService _nav, CredentialListViewModel _credentialsListViewModel, CredentialModel? credential = null) {
        Nav = _nav;
        CredentialListViewModel = _credentialsListViewModel;
        Credential = credential ?? new();

        // Reset data after view change
        if (!IsEditMode && Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    RefreshPage();
                }
            };
        }
    }

    [RelayCommand]
    private void ToggleShowPassword() {
        ShowPassword = !ShowPassword;
    }

    [RelayCommand]
    private void NavigateToVault() {
        Nav.NavigateTo(CredentialListViewModel);
    }

    [RelayCommand]
    private void SaveCredential() {

        var validationContext = new ValidationContext(Credential);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(Credential, validationContext, results, true);
        SetValidationErrors(results);

        if (!isValid) {
            return;
        }
    }

    private void SetValidationErrors(List<ValidationResult> results) {

        ServiceNameError = results.Find(result => result.MemberNames.Contains("ServiceName"))?.ErrorMessage ?? "";
        PasswordError = results.Find(result => result.MemberNames.Contains("Password"))?.ErrorMessage ?? "";
    }

    private void RefreshPage() {
        Credential = new();
        ServiceNameError = "";
        PasswordError = "";
    }
}
