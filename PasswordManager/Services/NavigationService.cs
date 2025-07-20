using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using System.Collections.Generic;

namespace PasswordManager.Services;

public partial class NavigationService(ILogService logService) : ObservableObject, INavigationService {

    [ObservableProperty]
    public INamedViewModel? _currentView = null;

    [ObservableProperty]
    private Stack<INamedViewModel> _viewStack = new();

    private readonly ILogService _logService = logService;

    public void NavigateTo(INamedViewModel viewModel) {

        _logService.Log($"Navigation from view {CurrentView?.Title} to {viewModel.Title}");

        if (CurrentView != null && CurrentView.IncludeInNavStack) {
            ViewStack.Push(CurrentView);
            _logService.Log($"Current view pushed to viewStack");
        }

        CurrentView = viewModel;
    }

    public void NavigateBack() {

        if (!CanNavigateBack()) {
            _logService.Log("Cannot navigate back, view stack is empty.");
            return;
        }

        _logService.Log($"Navigating back from view {CurrentView!.Title} to {ViewStack.Peek().Title}");
        CurrentView = ViewStack.Pop();
    }

    public bool CanNavigateBack() {
        return ViewStack.Count > 0;
    }
}
