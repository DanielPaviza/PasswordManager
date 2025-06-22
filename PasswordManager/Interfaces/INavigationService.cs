namespace PasswordManager.Interfaces;

public interface INavigationService {

    INamedViewModel? CurrentView { get; }
    void NavigateTo(INamedViewModel viewModel);
    void NavigateBack();
    bool CanNavigateBack();
}
