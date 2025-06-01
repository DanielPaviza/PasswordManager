using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services;

public class EncryptionService {
    private readonly byte[] _key;

    public EncryptionService(string masterPassword) {
        // Derive a 256-bit key from the master password using PBKDF2
        var salt = Encoding.UTF8.GetBytes("TvojeMámaJeŽenská");
        using var kdf = new Rfc2898DeriveBytes(masterPassword, salt, 100_000, HashAlgorithmName.SHA256);
        _key = kdf.GetBytes(32);
    }

    public string Encrypt(string plainText) {
        using var aes = Aes.Create();
        aes.Key = _key;
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

    public string Decrypt(string cipherText) {
        var bytes = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        aes.Key = _key;
        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(bytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(bytes, iv.Length, bytes.Length - iv.Length);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
