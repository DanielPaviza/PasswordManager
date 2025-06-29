using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels;

public class CredentialListViewModel : ViewModelBase, INamedViewModel {

    private readonly INavigationService Nav;

    public bool IncludeInNavStack => true;
    public string Title => "Credential vault";

    public CredentialListViewModel(INavigationService nav) {
        Nav = nav;
    }
}
