using PasswordManager.Configs;
using PasswordManager.Interfaces;
using PasswordManager.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services;

public class EncryptionService : IEncryptionService {

    private static byte[] _credentialKey = [];

    public string EncryptCredentials(string plainText) {
        using var aes = Aes.Create();
        aes.Key = _credentialKey;
        aes.GenerateIV();
        using var encryptor = aes.CreateEncryptor();

        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length); // Prepend IV to ciphertext
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs)) {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public string DecryptCredentials(string encryptedText) {
        var bytes = Convert.FromBase64String(encryptedText);
        using var aes = Aes.Create();
        aes.Key = _credentialKey;
        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(bytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(bytes, iv.Length, bytes.Length - iv.Length);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

    public static bool VerifyMasterPassword(string input) {
        var saltBytes = Convert.FromBase64String(PasswordConfig.MasterPasswordSalt);
        var inputHash = HashService.HashPassword(input, saltBytes);
        bool success = PasswordConfig.MasterPassword.Equals(inputHash, StringComparison.CurrentCultureIgnoreCase);
        if (success) _credentialKey = CreateKdfKey(inputHash);

        return success;
    }

    // Derive a 256-bit key from the master password using PBKDF2
    private static byte[] CreateKdfKey(string masterPassword) {
        var password = Encoding.UTF8.GetBytes(PasswordConfig.MasterPassword);
        var salt = Encoding.UTF8.GetBytes(PasswordConfig.CredentialPasswordSalt);
        using var kdf = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        return kdf.GetBytes(32);
    }
}
