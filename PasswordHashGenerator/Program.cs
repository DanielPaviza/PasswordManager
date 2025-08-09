using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PasswordManager.Security;
using System.Reflection;
using System.Text;
class Program {

    static readonly string appsettingsFolderName = "PasswordManager";
    static readonly string appsettingsFileName = "appsettings.json";
    static readonly int passwordMinLength = 1;

    static async Task Main() {
        Task.Delay(5).Wait(); // Sometimes, the text console does not render correctly on first run, adding a delay to fix it
        await StartProgram();
    }

    static async Task StartProgram() {
        Console.Clear();

        RenderHeading();
        string password = ReadPassword();

        byte[] masterPasswordSalt = HashService.GenerateRandomKey();
        string masterPasswordSaltString = Convert.ToBase64String(masterPasswordSalt);
        string masterPasswordHash = HashService.HashPassword(password, masterPasswordSalt);
        string credentialSalt = Convert.ToBase64String(HashService.GenerateRandomKey(16));
        RenderSecrets(masterPasswordSaltString, masterPasswordHash, credentialSalt);

        bool editAppsettings = RenderYesNoPrompt("Do you want to edit the appsettings.json file automatically?");
        if (editAppsettings) {
            await EditAppsettings(masterPasswordHash, masterPasswordSaltString, credentialSalt);
        } else {
            RenderCopySecretsManuallyMessage();
        }
    }

    static string ReadPassword() {
        bool passwordMatch = false;
        string password = string.Empty;

        while (!passwordMatch || password.Length < passwordMinLength) {

            password = ReadPasswordInput("\nEnter password: ");
            if (password.Length < passwordMinLength) {
                Console.WriteLine($"\nPassword must not be empty. Try again.");
                continue;
            }

            string confirm = ReadPasswordInput("Confirm password: ");
            passwordMatch = password == confirm;
            if (!passwordMatch) Console.WriteLine($"Passwords do not match. Try again.");
        }

        return password;
    }

    static void RenderSecrets(string masterPasswordHash, string masterPasswordSalt, string credentialSalt) {
        Console.WriteLine($"\nSecrets generated successfully.");
        Console.WriteLine($"Please paste them to PasswordManager/appsettings.json or update the file automatically.");
        Console.WriteLine($"------------------------------------------------------------------------");
        Console.WriteLine("{");
        Console.WriteLine($"    \"MasterPasswordHash\": \"{masterPasswordHash}\"");
        Console.WriteLine($"    \"MasterPasswordSalt\": \"{masterPasswordSalt}\"");
        Console.WriteLine($"    \"CredentialPasswordSalt\": \"{credentialSalt}\"");
        Console.WriteLine("}");
        Console.WriteLine($"------------------------------------------------------------------------");
        Console.WriteLine();
    }

    static async Task EditAppsettings(string masterPasswordHash, string masterPasswordSalt, string credentialSalt) {

        string? current = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (current == null) {
            RenderEditAppsettingsError();
            return;
        }

        var currentDir = new DirectoryInfo(current!);
        string folderPath = Path.Combine(currentDir.Parent?.FullName ?? "", appsettingsFolderName);
        string configPath = Path.Combine(folderPath, appsettingsFileName);

        // Try to find PasswordManager directory in one of the the parent directories
        while (currentDir.Parent != null && !Directory.Exists(folderPath)) {
            currentDir = currentDir.Parent;
            folderPath = Path.Combine(currentDir.FullName, appsettingsFolderName);
            configPath = Path.Combine(folderPath, appsettingsFileName);
        }

        // If the folder was not found, ask the user for the path
        if (!Directory.Exists(folderPath)) {
            Console.WriteLine("Could not locate the parent PasswordManager project folder.");
            Console.WriteLine("Please input the absolute path of the directory that contains the appsettings.json.");

            bool repeatAskingForValidDirectory = true;
            do {
                Console.Write("Path: ");
                folderPath = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!Directory.Exists(folderPath)) {
                    Console.WriteLine($"Could not find folder at: {folderPath}");
                    repeatAskingForValidDirectory = RenderYesNoPrompt("Try again?");
                }

            } while (repeatAskingForValidDirectory && !Directory.Exists(folderPath));


            while (repeatAskingForValidDirectory && !Directory.Exists(folderPath)) {
                Console.WriteLine($"Could not find folder at: {folderPath}");
                repeatAskingForValidDirectory = RenderYesNoPrompt("Try again?");
            }

            if (!Directory.Exists(folderPath)) {
                RenderCopySecretsManuallyMessage();
                return;
            }

            configPath = Path.Combine(folderPath, appsettingsFileName);
        }

        try {

            JObject json;
            try {

                // Create empty appsettings.json if it does not exist
                if (!File.Exists(configPath)) {
                    Console.WriteLine($"Appsettings.json file created at {configPath}");
                    File.WriteAllText(configPath, "{}");
                }

                var jsonText = File.ReadAllText(configPath);
                json = JObject.Parse(jsonText);
            } catch (Exception parseEx) when (parseEx is IOException || parseEx is JsonReaderException) {
                json = [];
            }

            json["MasterPasswordHash"] = masterPasswordHash;
            json["MasterPasswordSalt"] = masterPasswordSalt;
            json["CredentialPasswordSalt"] = credentialSalt;

            File.WriteAllText(configPath, json.ToString());
            await File.WriteAllTextAsync(configPath, json.ToString());
            Console.WriteLine($"appsettings.json updated successfully at {configPath}");
        } catch (Exception ex) {

            RenderEditAppsettingsError(configPath);
            if (ex is IOException) {
                Console.WriteLine("Ensure you have write permissions to the file and that it is not open in another program.");
            }

            RenderCopySecretsManuallyMessage();
        }
    }

    static bool RenderYesNoPrompt(string prompt) {
        Console.WriteLine(prompt);
        Console.Write("(y/n): ");
        string? input = Console.ReadLine()?.Trim().ToLowerInvariant();
        return input == "y" || input == "yes";
    }

    static void RenderCopySecretsManuallyMessage() => Console.WriteLine("Please copy the secrets manually.");

    private static void RenderEditAppsettingsError(string? configPath = null) =>
        Console.WriteLine($"Unexpected error while trying to edit the appsettings.json{(configPath == null ? "." : " at " + configPath)}");

    static void RenderHeading() {
        Console.WriteLine($"------------------------------------------------------------------------");
        Console.WriteLine($"PasswordManager hash generator tool.");
        Console.WriteLine($"Creates a secure Argon2 master password hash and salts based on a text password.");
        Console.WriteLine($"Paste the secrets into the appsettings.json and then use the original text password to log into the manager.");
    }

    static string ReadPasswordInput(string prompt) {
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
