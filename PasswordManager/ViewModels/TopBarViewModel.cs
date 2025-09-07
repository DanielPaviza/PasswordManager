using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Enums;
using PasswordManager.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using System.Collections.Generic;
using System.Linq;

namespace PasswordManager.ViewModels;

public partial class TopBarViewModel : ViewModelBase {

    private readonly INavigationService _nav;
    private readonly CredentialListViewModel _credentialsListViewModel;
    private readonly CredentialFormViewModel _credentialAddViewModel;
    private readonly ILogService _logService;
    private readonly ILanguageService _languageService;

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

    [ObservableProperty]
    private string _currentLanguage;

    public IEnumerable<string> LanguageOptions { get; } = LanguageService.AllLanguages.Select(l => l.ToDisplayName());

    public TopBarViewModel(
        INavigationService Nav,
        CredentialListViewModel CredentialsListViewModel,
        CredentialFormViewModel CredentialAddViewModel,
        LogViewModel __logViewModel,
        ILogService logService,
        ILanguageService languageService
        ) {

        _nav = Nav;
        _credentialsListViewModel = CredentialsListViewModel;
        _credentialAddViewModel = CredentialAddViewModel;
        LogViewModel = __logViewModel;
        _logService = logService;
        _languageService = languageService;

        CurrentLanguage = _languageService.CurrentLanguage.ToDisplayName();

        OnCurrentViewChange();

        Nav.PropertyChanged += (s, e) => {
            if (e.PropertyName == nameof(Nav.CurrentView)) {
                OnCurrentViewChange();
                NavigateBackCommand.NotifyCanExecuteChanged();
            }
        };

        this.PropertyChanged += (s, e) => {
            if (e.PropertyName == nameof(CurrentLanguage)) {
                OnLanguageChanged();
            }
        };

        _logService.LogInfo("TopBarViewModel initialized");
    }

    private void OnCurrentViewChange() {
        UpdateTabSelection();
        Title = _nav.CurrentView.Title;
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

    private void OnLanguageChanged() {
        AppLanguageEnum newLang = AppLanguageExtensions.FromDisplayName(CurrentLanguage);
        _languageService.SetLanguage(newLang);
    }
}
