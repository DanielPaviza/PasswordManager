using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Enums;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using System;
using System.Collections.ObjectModel;

namespace PasswordManager.Services {
    public partial class LogService : ObservableObject, ILogService {

        [ObservableProperty]
        private ObservableCollection<LogModel> _logs = [];

        private void Log(string message, LogSeverityEnum severity) {

            var log = new LogModel(message, severity);
            Logs.Add(log);
            Console.WriteLine(log.ToString());
        }

        public void LogDebug(string message) {
#if DEBUG
            Log(message, LogSeverityEnum.Debug);
#endif
        }
        public void LogInfo(string message) => Log(message, LogSeverityEnum.Info);
        public void LogSuccess(string message) => Log(message, LogSeverityEnum.Success);
        public void LogWarning(string message) => Log(message, LogSeverityEnum.Warning);
        public void LogError(string message) => Log(message, LogSeverityEnum.Error);
    }
}
