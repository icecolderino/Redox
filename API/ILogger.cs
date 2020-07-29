using System;

namespace Redox.API
{
    public interface ILogger
    {
        void Log(string message, params object[] args);

        void LogInfo(string message, params object[] args);

        void LogWarning(string message, params object[] args);

        void LogError(string message, params object[] args);

        void LogSpeed(string message, params object[] args);

        void LogException(Exception ex, bool verbose = true);

        void LogDebug(string message, params object[] args);
    }
}
