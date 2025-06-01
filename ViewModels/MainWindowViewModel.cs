using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using System.Collections.Generic;

namespace PasswordManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase, INavigationService {
    [ObservableProperty]
    private ViewModelBase currentView;

    private Stack<ViewModelBase> _viewStack = new();

    public MainWindowViewModel() {
        var loginViewModel = new LoginViewModel(OnLoginSuccess);
        CurrentView = loginViewModel;
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
        NavigateTo(new PasswordsListViewModel(this));
    }
}
