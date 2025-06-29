
using System;

namespace PasswordManager.Models;

public class PasswordSettingsModel {
    public required string MasterPasswordHash { get; set; }
    public string MasterPasswordSalt { get; set; } = String.Empty;
    public string CredentialPasswordSalt { get; set; } = String.Empty;
}
