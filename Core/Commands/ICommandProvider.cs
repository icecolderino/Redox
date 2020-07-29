using Redox.Core.Plugins;
using System.Threading.Tasks;

namespace Redox.Core.Commands
{
    public interface ICommandProvider
    {
        Task RegisterAsync<TCommand>(Plugin plugin) where TCommand : ICommand;

        Task UnregisterAsync<TCommand>(Plugin plugin) where TCommand : ICommand;
        
        Task<bool> HasCommandAsync(string name, Plugin plugin);

        Task<ICommandContext> GetCommandAsync(string name, Plugin plugin);
        
        Task CallAsync(ICommandExecutor executor, string command, string[] args);

    }
}
