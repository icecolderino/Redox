using System;
using System.Threading;
using Redox.Core.User;

namespace Redox.API.Console
{
    public interface IConsole : IUser
    {
        void Reply(string message);

        void RunCommand(string command, string[] args);

    }
}
