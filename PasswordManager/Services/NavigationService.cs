using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System.Collections.Generic;

namespace PasswordManager.Services;

public partial class NavigationService(ILogService logService, LoginViewModel loginViewModel) : ObservableObject, INavigationService {

    [ObservableProperty]
    public INamedViewModel _currentView = loginViewModel;

    [ObservableProperty]
    private Stack<INamedViewModel> _viewStack = new();

    private readonly ILogService _logService = logService;

    public void NavigateTo(INamedViewModel viewModel) {

        _logService.LogInfo($"Navigation from view {CurrentView.Title} to {viewModel.Title}");

        if (CurrentView.IncludeInNavStack) {
            ViewStack.Push(CurrentView);
            _logService.LogDebug($"Current view pushed to viewStack");
        }

        CurrentView = viewModel;
    }

    public void NavigateBack() {

        if (!CanNavigateBack()) {
            _logService.LogInfo("Cannot navigate back, view stack is empty.");
            return;
        }

        _logService.LogInfo($"Navigating back from view {CurrentView.Title} to {ViewStack.Peek().Title}");
        CurrentView = ViewStack.Pop();
    }

    public bool CanNavigateBack() => ViewStack.Count > 0;
}
