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

        public void Log(string message, LogSeverityEnum severity = LogSeverityEnum.Info) {

            var log = new LogModel(message, severity);
            Logs.Add(log);
            Console.WriteLine(log.ToString());
        }
    }
}
