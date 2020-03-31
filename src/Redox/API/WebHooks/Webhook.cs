
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redox.API.Exceptions;
using Redox.API.Helpers;

namespace Redox.API.WebHooks
{
    
    public static class Webhook
    {
        /// <summary>
        /// Send an embed message to a discord webhook
        /// </summary>
        /// <param name="content">The cont of the message</param>
        /// <param name="webhookurl">The URL of the discord webhook</param>
        /// <param name="url">The rederect url (Optional)</param>
        /// <param name="username">The name of the webhook (Optional)</param>
        /// <param name="avatar_url">The URL of the avatar image (Optional)</param>
        /// <param name="embeds">List of embeds (Optional)</param>
        public static void SendToDiscord(string content, string webhookurl, string url = null,  string username = null, string avatar_url = null, List<Embed> embeds = null)
        {
            try
            {
                if (content.Length > 1999) content = content.Substring(0, 1999);

                JObject data = new JObject(
                    new JProperty("content", content),
                    new JProperty("username", username),
                    new JProperty("avatar_url", avatar_url)
                );
                JArray embedProperty = new JArray();
                foreach (Embed em in embeds)
                {
                    JObject embedObject = BuildEmbed(em);
                    if (embedObject.Count > 0) embedProperty.Add(embedObject);
                }
                if (embedProperty.Count() > 0) data.Add(new JProperty("embeds", embedProperty));
                using (WebClient webclient = new WebClient())
                {
                    string datacontainer = JSONHelper.ToJson(data);
                    webclient.Encoding = Encoding.UTF8;
                    webclient.Headers.Set(HttpRequestHeader.ContentType, "application/json");
                    webclient.UploadString(webhookurl, datacontainer);
                }
            }
            catch(WebhookException ex)
            {
                Redox.Logger.LogException(ex);
            }
          
        }
        private static JObject BuildEmbed(Embed embed)
        {
            JObject embedObject = new JObject();
            Author author = embed.Author;
            Field[] fields = embed.Fields;
            Footer footer = embed.Footer;


            if (embed.Title.Length > 0) embedObject.Add(new JProperty("title", embed.Title));
            if (embed.Description.Length > 0) embedObject.Add(new JProperty("description", embed.Description));
            if (embed.Url.Length > 0) embedObject.Add(new JProperty("url", embed.Url));
            if (embed.Color.Length == 6) embedObject.Add(new JProperty("color", Convert.ToInt32(embed.Color, 16)));
            if (embed.SendTimestamp) embedObject.Add(new JProperty("timestamp", DateTime.UtcNow.ToString("s")));
            if(embed.Thumbnail.Length > 0) embedObject.Add(new JProperty("thumbnail", new JObject(new JProperty("url", embed.Thumbnail))));
            if(embed.Image.Length > 0) embedObject.Add(new JProperty("image", new JObject(new JProperty("url", embed.Image))));
            if (author.Name.Length > 0)
            {
                JObject embedObjectAuthor = new JObject(new JProperty("name", author.Name));
                if (author.Url.Length > 0) embedObjectAuthor.Add(new JProperty("url", author.Url));
                if (author.Icon_url.Length > 0) embedObjectAuthor.Add(new JProperty("icon_url", author.Icon_url));

                embedObject.Add(new JProperty("author", embedObjectAuthor));
            }
            if(footer.Text.Length > 0)
            {
                JObject embedObjectFooter = new JObject(new JProperty("text", footer.Text));
                if (footer.Icon_url.Length > 0) embedObjectFooter.Add(new JProperty("icon_url", footer.Icon_url));

                embedObject.Add(new JProperty("footer", embedObjectFooter));
            }
            JArray fieldProperty = new JArray();
            foreach (Field field in embed.Fields)
            {
                JObject fieldObject = BuildField(field);
                if (fieldObject != null) fieldProperty.Add(fieldObject);
            }
            if (fieldProperty.Count() > 0) embedObject.Add(new JProperty("fields", fieldProperty));

            return embedObject;
        }
        private static JObject BuildField(Field field)
        {
            if (field.Name.Length == 0 || field.Value.Length == 0) return null;

            return new JObject(
                new JProperty("name", field.Name),
                new JProperty("value", field.Value),
                new JProperty("inline", field.Inline)
            );
        }
    }
}

