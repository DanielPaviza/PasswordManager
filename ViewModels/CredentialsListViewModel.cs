using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels;

public class CredentialsListViewModel : ViewModelBase, INamedViewModel {

    private readonly INavigationService Nav;

    public bool IncludeInNavStack => true;
    public string Title => "Credential vault";

    public CredentialsListViewModel(INavigationService nav) {
        Nav = nav;
    }
}
