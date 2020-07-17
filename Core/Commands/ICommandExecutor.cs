using Redox.API.Console;
using Redox.API.Player;
using Redox.Core.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redox.Core.Commands
{
    public interface ICommandExecutor
    {
        IUser User { get; }

        bool IsPlayer { get; }

        bool IsConsole { get; }

        IPlayer GetPlayer();
        IConsole GetConsole();

    
    }
}
