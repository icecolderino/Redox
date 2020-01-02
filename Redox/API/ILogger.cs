using System;
using System.IO;

namespace Redox.API
{
    public interface ILogger
    {

        StreamWriter writer { get;}

        void Log(string message);

        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogSpeed(string message);

        void LogException(Exception ex);

        void LogDebug(string message);

    }
}
