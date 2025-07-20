using PasswordManager.Enums;
using PasswordManager.Models;
using System.Collections.ObjectModel;

namespace PasswordManager.Interfaces {
    public interface ILogService {

        ObservableCollection<LogModel> Logs { get; }

        void Log(string message, LogSeverityEnum severity = LogSeverityEnum.Info);
    }

}
