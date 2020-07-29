using System.Collections.Generic;

namespace Redox.API.Localization
{
    public sealed class Translation : Dictionary<string, string>
    {
        public string Language { get; }

        public Translation(string language) : base()
        {
            Language = language;
        }
    }
    
}