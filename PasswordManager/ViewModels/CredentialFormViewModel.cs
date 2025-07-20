using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PasswordManager.ViewModels;

public partial class CredentialFormViewModel : ViewModelBase, INamedViewModel {

    private readonly ILogService LogService;
    private readonly INavigationService Nav;
    private readonly CredentialListViewModel CredentialListViewModel;

    [ObservableProperty]
    private CredentialModel _credential;

    [ObservableProperty]
    private bool _showPassword = false;

    public bool IsEditMode { get; }

    public bool IncludeInNavStack => true;
    public string Title => IsEditMode ? "Edit Credential" : "Save new Credentials";

    [ObservableProperty]
    public string _serviceNameError = "";

    [ObservableProperty]
    public string _passwordError = "";

    public CredentialFormViewModel(
        INavigationService _nav,
        ILogService _logService,
        CredentialListViewModel _credentialsListViewModel,
        CredentialModel? credential = null
        ) {
        Nav = _nav;
        LogService = _logService;
        CredentialListViewModel = _credentialsListViewModel;
        Credential = credential ?? new();
        RegisterDataResetOnViewChange();

        _logService.Log("CredentialFormViewModel initialized");
    }

    private string GetFirstErrorByProperty(string propertyName, object propertyValue) {

        Credential.ValidateSingleProperty(propertyName, propertyValue);
        var errorList = Credential.GetErrors(propertyName).ToList();
        if (errorList.Count <= 0) return "";

        return errorList.First().ErrorMessage!;
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

        if (!isValid) {
            SetValidationErrors();
            LogService.Log("Credential save failed. Invalid credential data.");
            return;
        }

        //TODO CREDENTIALSERVICE.SAVE
    }

    // Credential instance changes when switching tabs
    // Need to add propertyChanged to Credential again and detach the old one
    partial void OnCredentialChanging(CredentialModel value) => DetachCredentialEvents(value);
    partial void OnCredentialChanged(CredentialModel value) => value.PropertyChanged += CredentialPropertyChanged;

    private void DetachCredentialEvents(CredentialModel? oldValue) {
        if (oldValue != null)
            oldValue.PropertyChanged -= CredentialPropertyChanged;
    }

    private void CredentialPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        switch (e.PropertyName) {
            case nameof(Credential.ServiceName):
                ServiceNameError = GetFirstErrorByProperty(e.PropertyName, Credential.ServiceName);
                break;

            case nameof(Credential.Password):
                PasswordError = GetFirstErrorByProperty(e.PropertyName, Credential.Password);
                break;
        }
    }

    // Set Credential data to default on tab switch
    private void RegisterDataResetOnViewChange() {
        if (!IsEditMode && Nav is NavigationService navImpl) {
            navImpl.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(navImpl.CurrentView)) {
                    RefreshPage();
                }
            };
        }
    }

    private void SetValidationErrors() {
        ServiceNameError = GetFirstErrorByProperty(nameof(Credential.ServiceName), Credential.ServiceName);
        PasswordError = GetFirstErrorByProperty(nameof(Credential.Password), Credential.Password);
    }

    private void RefreshPage() {
        LogService.Log("Refreshing create credential data");
        Credential = new();
        ServiceNameError = "";
        PasswordError = "";
    }
}
