
using PasswordManager.ViewModels;

namespace PasswordManager.Interfaces;

public interface INavigationService {
    void NavigateTo(ViewModelBase viewModel);
    void NavigateBack();
}
