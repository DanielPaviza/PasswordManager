using System.ComponentModel;

namespace PasswordManager.Interfaces;

public interface INavigationService : INotifyPropertyChanged {

    INamedViewModel CurrentView { get; }
    void NavigateTo(INamedViewModel viewModel);
    void NavigateBack();
    bool CanNavigateBack();
}
