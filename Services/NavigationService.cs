using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using System.Collections.Generic;

namespace PasswordManager.Services;

public partial class NavigationService : ObservableObject, INavigationService {

    [ObservableProperty]
    public INamedViewModel? _currentView = null;

    [ObservableProperty]
    private Stack<INamedViewModel> _viewStack = new();

    public void NavigateTo(INamedViewModel viewModel) {
        if (CurrentView != null && CurrentView.IncludeInNavStack)
            ViewStack.Push(CurrentView);

        CurrentView = viewModel;
    }

    public void NavigateBack() {
        if (CanNavigateBack()) {
            CurrentView = ViewStack.Pop();
        }
    }

    public bool CanNavigateBack() {
        return ViewStack.Count > 0;
    }
}
