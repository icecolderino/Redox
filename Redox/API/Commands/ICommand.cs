using Redox.API.Player;


namespace Redox.API.Commands
{
    /// <summary>
    /// Base Interface for Commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// <para>The name of the command</para>
        /// <para>This should never be null</para>
        /// 
        /// </summary>
        string name { get; }

        /// <summary>
        /// The description of the command
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Syntax of the command
        /// </summary>
        string Syntax { get; }

        /// <summary>
        /// The Flag of the command
        /// 
        /// <para>With flags you can determine how a command can be accessed/para>
        /// </summary>
        CommandFlags Flag { get; }

        /// <summary>
        /// Executes when the command is being triggered
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        void Execute(IPlayer player, string[] args);
    }
}
