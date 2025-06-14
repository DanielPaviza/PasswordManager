using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels;

public partial class CredentialsListViewModel : ViewModelBase {
    private readonly INavigationService _navigation;

    public CredentialsListViewModel(INavigationService navigation) {
        _navigation = navigation;
    }
}
