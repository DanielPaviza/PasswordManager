namespace PasswordManager.Interfaces;

public interface INamedViewModel {
    string Title { get; }
    bool IncludeInNavStack { get; }
}
