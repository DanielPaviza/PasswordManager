using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using PasswordManager.ViewModels;

namespace PasswordManager;

public static class DependencyInjection {
    public static ServiceProvider Configure() {
        var services = new ServiceCollection();

        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=vault.db"));

        // Services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ICredentialService, CredentialService>();
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<ILanguageService, LanguageService>();

        // ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<TopBarViewModel>();
        services.AddSingleton<LogViewModel>();
        services.AddSingleton<CredentialListViewModel>();
        services.AddSingleton<CredentialFormViewModel>();

        return services.BuildServiceProvider();
    }
}
