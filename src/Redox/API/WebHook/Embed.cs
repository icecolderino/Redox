using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.WebHooks
{
    public class Embed
    {
        public string title { get; set; } = null;
        public string description { get; set; } = null;
        public string url { get; set; } = null;
        public string color { get; set; } = null;
        public bool SendTimestamp { get; set; } = false;
        public Footer footer { get; set; } = null;
        public Author author { get; set; } = null;
        public Field[] fields { get; set; } = null;
        public string thumbnail { get; set; } = null;
        public string image { get; set; } = null;
    }
    public class Footer
    {
        public string text { get; set; }
        public string icon_url { get; set; }
    }
    public class Author
    {
        public string name { get; set; }
        public string url { get; set; }
        public string icon_url { get; set; }
    }
    public class Field
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool inline { get; set; }
    }
}
