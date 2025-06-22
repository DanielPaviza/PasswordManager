using Newtonsoft.Json.Linq;
using PasswordManager.Security;
using System.Reflection;
using System.Text;
class Program {
    static void Main(string[] args) {

        int passwordMinLength = 1;
        bool passwordMatch = false;
        string password = string.Empty;
        RenderHeading();

        while (!passwordMatch || password.Length < passwordMinLength) {

            password = ReadPassword("\nEnter password: ");
            if (password.Length < passwordMinLength) {
                Console.Clear();
                RenderHeading();
                Console.WriteLine($"\nPassword must not be empty. Try again.");
                continue;
            }

            string confirm = ReadPassword("Confirm password: ");
            passwordMatch = password == confirm;
            if (!passwordMatch) {
                Console.Clear();
                RenderHeading();
                Console.WriteLine($"\nPasswords do not match. Try again.");
            }
        }

        byte[] masterPasswordSalt = HashService.GenerateRandomKey();
        string masterPasswordSaltString = Convert.ToBase64String(masterPasswordSalt);
        string masterPasswordHash = HashService.HashPassword(password, masterPasswordSalt);
        string credentialSalt = Convert.ToBase64String(HashService.GenerateRandomKey(16));

        Console.WriteLine($"\nCredentials generated successfully.");
        Console.WriteLine($"Please paste them to PasswordManager/appsettings.json or update the file automatically.");
        Console.WriteLine($"---------------------------------------------------------------------");
        Console.WriteLine($"MasterPasswordHash: {masterPasswordHash}");
        Console.WriteLine($"MasterPasswordSalt: {masterPasswordSaltString}");
        Console.WriteLine($"CredentialPasswordSalt: {credentialSalt}");
        Console.WriteLine();

        Console.WriteLine("Do you want to update the appsettings.json file automatically?");
        Console.Write("Edit file (y/n):");
        string? input = Console.ReadLine();
        bool edit = input?.Trim().ToLower() == "y";

        if (edit) EditAppsettings(masterPasswordHash, masterPasswordSaltString, credentialSalt);
    }

    static void EditAppsettings(string masterPasswordHash, string masterPasswordSalt, string credentialSalt) {

        string appsettingsFileName = "appsettings.json";
        string configPath = string.Empty;
        string? current = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        DirectoryInfo? dir = new DirectoryInfo(current!);

        // Find PasswordManager parent folder
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "PasswordManager"))) {
            dir = dir.Parent;
        }

        if (dir != null) {
            configPath = Path.Combine(dir.FullName, "PasswordManager", appsettingsFileName);
        } else {
            Console.WriteLine("Could not locate PasswordManager project folder.");
            Console.Write("Please input the absolute path of the folder containing the appsettings.json: \n");
            configPath = Console.ReadLine()?.Trim() ?? string.Empty;
            configPath = Path.Combine(configPath, appsettingsFileName);
        }

        if (!File.Exists(configPath)) {
            Console.WriteLine($"Could not find appsettings.json at: {configPath}\\{appsettingsFileName}");
            Console.WriteLine("Please copy the secrects manually.");
            return;
        }

        try {

            var json = JObject.Parse(File.ReadAllText(configPath));
            json["MasterPasswordHash"] = masterPasswordHash;
            json["MasterPasswordSalt"] = masterPasswordSalt;
            json["CredentialPasswordSalt"] = credentialSalt;

            File.WriteAllText(configPath, json.ToString());

            Console.WriteLine($"appsettings.json updated successfully at {configPath}");

        } catch {
            Console.WriteLine($"Error while trying to edit the appsettings.json at {configPath}");
        }
    }

    static void RenderHeading() {
        Console.WriteLine($"PasswordManager hash generator tool.");
        Console.WriteLine($"Creates random salts and generate secure Argon2 master password hash from input string.");
    }

    static string ReadPassword(string prompt) {
        Console.Write(prompt);
        var sb = new StringBuilder();
        ConsoleKeyInfo key;

        do {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && sb.Length > 0) {
                sb.Remove(sb.Length - 1, 1);
                Console.Write("\b \b");
            } else if (!char.IsControl(key.KeyChar)) {
                sb.Append(key.KeyChar);
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return sb.ToString();
    }
}
