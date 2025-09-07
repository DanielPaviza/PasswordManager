namespace PasswordManager.Interfaces;

public interface IEncryptionService {
    bool VerifyMasterPassword(string input);
    string EncryptCredentials(string plainText);
    string DecryptCredentials(string encryptedText);
}
