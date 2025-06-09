using PasswordManager.Configs;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services;

public class EncryptionService {

    private readonly byte[] _credentialKey;

    public EncryptionService() {

        _credentialKey = CreateKdfKey();
    }

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
        return PasswordConfig.MasterPassword == MakeSha256Hash(input);
    }

    private static string MakeSha256Hash(string input) {
        var saltedPassword = string.Format("{0}{1}", PasswordConfig.MasterPasswordSalt, input);
        var bytes = Encoding.UTF8.GetBytes(saltedPassword);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }


    // Derive a 256-bit key from the master password using PBKDF2
    private static byte[] CreateKdfKey() {
        var password = Encoding.UTF8.GetBytes(PasswordConfig.MasterPassword);
        var salt = Encoding.UTF8.GetBytes(PasswordConfig.CredentialPasswordSalt);
        using var kdf = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        return kdf.GetBytes(32);
    }
}
