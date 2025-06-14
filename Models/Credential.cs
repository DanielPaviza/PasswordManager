
namespace PasswordManager.Models;

public class Credential {
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool MaskUsername { get; set; } = false;
    public string Password { get; set; } = string.Empty;
    public bool MaskPassword { get; set; } = true;
    public string Note { get; set; } = string.Empty;
}

