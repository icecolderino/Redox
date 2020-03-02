using Redox.API.Player;


namespace Redox.API.Events
{
    public sealed class ChatEvent
    {
        public string Message { get; set; }

        public IPlayer Sender { get; }

        public bool Cancel { get; set; } = false;

        public ChatEvent(IPlayer sender, ref string message)
        {
            this.Sender = sender;
            this.Message = message;
        }

    }
}
