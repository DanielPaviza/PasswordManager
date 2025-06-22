using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Security;
public class HashService {
    public static string HashPassword(string password, byte[] salt, int memoryKb = 65536, int iterations = 4, int parallelism = 2) {

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)) {
            Salt = salt,
            DegreeOfParallelism = parallelism,
            MemorySize = memoryKb,
            Iterations = iterations
        };

        byte[] hashBytes = argon2.GetBytes(32); // 256-bit output

        return Convert.ToBase64String(hashBytes);
    }

    public static byte[] GenerateRandomKey(int size = 32) {
        var key = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return key;
    }
}
