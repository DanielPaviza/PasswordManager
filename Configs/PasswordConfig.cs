using Microsoft.Extensions.Configuration;
using PasswordManager.Models;
using System;

namespace PasswordManager.Configs;

public static class PasswordConfig {
    public static String MasterPassword { get; }
    public static String MasterPasswordSalt { get; }
    public static String CredentialPasswordSalt { get; }

    static PasswordConfig() {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var passwordSettings = config.Get<PasswordSettings>()
            ?? throw new InvalidOperationException("Missing or invalid password config in appsettings.json");

        MasterPassword = passwordSettings.MasterPasswordHash;
        MasterPasswordSalt = passwordSettings.MasterPasswordSalt;
        CredentialPasswordSalt = passwordSettings.CredentialPasswordSalt;
    }
}
