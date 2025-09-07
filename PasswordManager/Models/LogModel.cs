using PasswordManager.Enums;
using System;

namespace PasswordManager.Models;

public class LogModel(string message, LogSeverityEnum Severity = LogSeverityEnum.Info) {

    public DateTime Timestamp { get; set; } = DateTime.Now;

    public LogSeverityEnum Severity { get; set; } = Severity;

    public string Message { get; set; } = message;

    public override string ToString() => $"{Timestamp:yyyy-MM-dd HH:mm:ss} [{GetSeverityCode(Severity)}] {Message}";

    private static string GetSeverityCode(LogSeverityEnum severity) {
        return severity switch {
            LogSeverityEnum.Debug => "DBG",
            LogSeverityEnum.Info => "INF",
            LogSeverityEnum.Success => "SUC",
            LogSeverityEnum.Warning => "WAR",
            LogSeverityEnum.Error => "ERR",
            _ => "UNK", // Unknown
        };
    }
}
