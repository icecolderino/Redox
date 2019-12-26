using System;
using System.Threading.Tasks;
using Redox.API.Console;
using Redox.API.Player;

namespace Redox.API
{
    public interface IServer  : IPlayerManager
    {
        string ServerName { get; set; }

        string GameName { get; }

        ushort ServerPort { get; }

        Version GameVersion { get; }

        IConsole Console { get; }

        Task Reload();

        Task Shutdown();


        void Broadcast(string message);

        void Broadcast(string prefix, string message);



    }
}
   