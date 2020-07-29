using Redox.API.Player;
using Redox.Core.User;

namespace Redox.API.Events
{

    public sealed class ChatEvent
    {
        /// <summary>
        /// The user that sent the chat message.
        /// <para>
        /// The user can be IPlayer or IConsole.
        /// </para>
        /// </summary>
        public IUser Sender { get; }
        /// <summary>
        /// The chat message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Cancel the chat message from being sent in chat.
        /// </summary>
        public bool Cancel { get; set; } = false;

        public ChatEvent(IUser sender, string message)
        {
            this.Sender = sender;
            this.Message = message;
        }

    }
}
