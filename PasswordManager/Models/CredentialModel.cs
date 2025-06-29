using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Models;

public class CredentialModel {

    public string Id => $"{this.ServiceName}{this.Username}";

    [Required(ErrorMessage = "Service Name is required")]
    public string ServiceName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public bool MaskUsername { get; set; } = false;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public bool MaskPassword { get; set; } = true;

    public string Note { get; set; } = string.Empty;
}