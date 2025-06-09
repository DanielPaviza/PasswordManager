using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels;

public partial class PasswordsListViewModel : ViewModelBase {
    private readonly INavigationService _navigation;

    public PasswordsListViewModel(INavigationService navigation) {
        _navigation = navigation;
    }
}
