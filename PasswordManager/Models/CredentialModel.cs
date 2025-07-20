using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Entities;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Models;

public partial class CredentialModel : ObservableValidator {
    public string Id => $"{ServiceName}{Username}";

    [ObservableProperty]
    [Required(ErrorMessage = "Service Name is required")]
    private string _serviceName = string.Empty;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private bool maskUsername = false;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required")]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool maskPassword = true;

    [ObservableProperty]
    private string note = string.Empty;

    public CredentialModel() { }

    public CredentialModel(CredentialEntity credentialEntity) {
        ServiceName = credentialEntity.ServiceName;
        Username = credentialEntity.Username;
        MaskUsername = credentialEntity.MaskUsername;
        Password = credentialEntity.Password;
        MaskPassword = credentialEntity.MaskPassword;
        Note = credentialEntity.Note ?? "";
    }

    public CredentialEntity ToEntity() {
        return new CredentialEntity {
            ServiceName = this.ServiceName,
            Username = this.Username,
            MaskUsername = this.MaskUsername,
            Password = this.Password,
            MaskPassword = this.MaskPassword,
            Note = this.Note
        };
    }

    public void ValidateSingleProperty(string propertyName, object propertyValue) {
        ValidateProperty(propertyValue, propertyName);
    }

    public void ValidateProperties() {
        ValidateAllProperties();
    }
}
