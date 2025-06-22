namespace PasswordManager.Interfaces;

public interface IEncryptionService {
    string EncryptCredentials(string plainText);
    string DecryptCredentials(string encryptedText);
}
