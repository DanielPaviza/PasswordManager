
namespace PasswordManager.Models;

public class Credential {
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}
