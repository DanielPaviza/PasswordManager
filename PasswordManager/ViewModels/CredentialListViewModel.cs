using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels;

public class CredentialListViewModel : ViewModelBase, INamedViewModel {

    private readonly INavigationService _nav;
    private readonly ILogService _logService;

    public CredentialListViewModel(INavigationService nav, ILogService logService) {
        _nav = nav;
        _logService = logService;
        _logService.Log("CredentialListViewModel initialized");
    }

    public bool IncludeInNavStack => true;
    public string Title => "Credential vault";
}
