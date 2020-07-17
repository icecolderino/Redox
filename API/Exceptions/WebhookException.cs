using System;

namespace Redox.API.Exceptions
{
    public class WebhookException : Exception
    {
        public WebhookException()  : base() { }
        public WebhookException(string message) : base(message) { }

    }
}
