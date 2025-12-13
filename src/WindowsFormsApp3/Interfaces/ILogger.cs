using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3.Interfaces
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception ex, string message);
        void LogDebug(string message);
        void LogCritical(string message);
        void LogCritical(Exception ex, string message);
    }

    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }
}