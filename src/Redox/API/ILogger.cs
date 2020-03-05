using System;
using System.IO;

namespace Redox.API
{
    public interface ILogger
    {

        TextWriter writer { get;}

        void Log(string message);

        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogColor(string message, ConsoleColor Color);
        void LogSpeed(string message);

        void LogException(Exception ex);

        void LogDebug(string message);

    }
}
