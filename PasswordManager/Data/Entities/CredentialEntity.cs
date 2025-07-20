namespace PasswordManager.Entities;

public class CredentialEntity {

    public string Id => $"{this.ServiceName}{this.Username}";

    public string ServiceName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public bool MaskUsername { get; set; } = false;

    public string Password { get; set; } = string.Empty;

    public bool MaskPassword { get; set; } = true;

    public string? Note { get; set; } = string.Empty;
}