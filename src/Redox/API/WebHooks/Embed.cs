using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.WebHooks
{
    public struct Embed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Color { get; set; }
        public bool SendTimestamp { get; set; }
        public Footer Footer { get; set; }
        public Author Author { get; set; }
        public Field[] Fields { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
    }
    public struct Footer
    {
        public string Text { get; set; }
        public string Icon_url { get; set; }
    }
    public struct Author
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon_url { get; set; }
    }
    public struct Field
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Inline { get; set; }
    }
}
