using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels;

public partial class PasswordsListViewModel : ViewModelBase {
    private readonly INavigationService _navigation;
    private readonly string _masterPassword;

    public PasswordsListViewModel(INavigationService navigation) {
        _navigation = navigation;
    }
}
