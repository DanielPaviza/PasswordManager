using PasswordManager.Models;
using System.Collections.ObjectModel;

namespace PasswordManager.Interfaces {
    public interface ILogService {
        ObservableCollection<LogModel> Logs { get; }
        void LogDebug(string message);
        void LogInfo(string message);
        void LogSuccess(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
