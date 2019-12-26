using System;

using Redox.Core.User;

namespace Redox.API.Console
{
    public interface IConsole : IUser
    {
        void Log(string message);

        void LogWarning(string warning);

        void LogError(string error);

        void LogException(Exception exception);

        void Reply(string message);

        void RunCommand(string command, string[] args);


    }
}
