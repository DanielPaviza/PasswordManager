using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System.Linq;

namespace PasswordManager;

public partial class App : Application {

    public static ServiceProvider? Services { get; private set; }

    public override void Initialize() {
        SQLitePCL.Batteries.Init();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=vault.db"));

            // Register services
            serviceCollection.AddSingleton<INavigationService, NavigationService>();
            serviceCollection.AddSingleton<IEncryptionService, EncryptionService>();

            // Register ViewModels
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<TopBarViewModel>();
            serviceCollection.AddSingleton<CredentialListViewModel>();
            serviceCollection.AddSingleton<CredentialFormViewModel>();

            Services = serviceCollection.BuildServiceProvider();

            var mainWindowVm = Services.GetRequiredService<MainWindowViewModel>();
            var mainWindow = new Views.MainWindow { DataContext = mainWindowVm };

            desktop.MainWindow = mainWindow;


            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation() {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}