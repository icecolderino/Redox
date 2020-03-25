using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redox.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.WebHooks
{
    
    public  class Webhook
    {

        public void SendToDiscord(string content, string webhookurl, string url = null,  string username = null, string avatar_url = null, List<Embed> embeds = null)
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
        private JObject BuildEmbed(Embed embed)
        {
            JObject embedObject = new JObject();
            Author author = embed.author;
            Field[] fields = embed.fields;
            Footer footer = embed.footer;


            if (embed.title.Length > 0) embedObject.Add(new JProperty("title", embed.title));
            if (embed.description.Length > 0) embedObject.Add(new JProperty("description", embed.description));
            if (embed.url.Length > 0) embedObject.Add(new JProperty("url", embed.url));
            if (embed.color.Length == 6) embedObject.Add(new JProperty("color", Convert.ToInt32(embed.color, 16)));
            if (embed.SendTimestamp) embedObject.Add(new JProperty("timestamp", DateTime.UtcNow.ToString("s")));
            if(embed.thumbnail.Length > 0) embedObject.Add(new JProperty("thumbnail", new JObject(new JProperty("url", embed.thumbnail))));
            if(embed.image.Length > 0) embedObject.Add(new JProperty("image", new JObject(new JProperty("url", embed.image))));
            if (author.name.Length > 0)
            {
                JObject embedObjectAuthor = new JObject(new JProperty("name", author.name));
                if (author.url.Length > 0) embedObjectAuthor.Add(new JProperty("url", author.url));
                if (author.icon_url.Length > 0) embedObjectAuthor.Add(new JProperty("icon_url", author.icon_url));

                embedObject.Add(new JProperty("author", embedObjectAuthor));
            }
            if(footer.text.Length > 0)
            {
                JObject embedObjectFooter = new JObject(new JProperty("text", footer.text));
                if (footer.icon_url.Length > 0) embedObjectFooter.Add(new JProperty("icon_url", footer.icon_url));

                embedObject.Add(new JProperty("footer", embedObjectFooter));
            }
            JArray fieldProperty = new JArray();
            foreach (Field field in embed.fields)
            {
                JObject fieldObject = BuildField(field);
                if (fieldObject != null) fieldProperty.Add(fieldObject);
            }
            if (fieldProperty.Count() > 0) embedObject.Add(new JProperty("fields", fieldProperty));

            return embedObject;
        }
        private static JObject BuildField(Field field)
        {
            if (field.name.Length == 0 || field.value.Length == 0) return null;

            return new JObject(
                new JProperty("name", field.name),
                new JProperty("value", field.value),
                new JProperty("inline", field.inline)
            );
        }


        //Here's the example on sending a webhook to Discord :D

        /*Embed embed = new Embed()
        {
            author = new Author
            {
                icon_url = "https://i.imgur.com/M3bSfZp.png",
                name = "Author name",
                url = "https://github.com/RedoxMod"
            },
            description = "Cool desc",
            fields = new Field[]
            {
                new Field
                {
                    name = "Insane field",
                    inline = false,
                    value = "Example Field"
                },
                new Field
                {
                    name = "Insane field x2",
                    inline = true,
                    value = "Example Field x2"
                }
            },
            footer = new Footer
            {
                icon_url = "https://i.imgur.com/M3bSfZp.png",
                text = "RedoxMod"
            },
            title = "Amazing Tittle for RedoxWebhooks!",
            url = "https://github.com/RedoxMod",
            color = "7771",
            SendTimestamp = true,
            thumbnail = "https://i.imgur.com/M3bSfZp.png",
            image = "https://i.imgur.com/M3bSfZp.png"
        };

        List<Embed> embeds = new List<Embed>();
        embeds.Add(embed);
        Webhook wb = new Webhook();
        wb.SendToDiscord("Message", "https://discordapp.com/api/webhooks/692395753557196880/ch5SvmBPLiY9eVkg0v5eqHzi91WD6lAD8pRq3V3aG8IpFmSh1alXUrj-WGdkTGJ7Hyku", "https://github.com/RedoxMod", "UserName", "https://i.imgur.com/M3bSfZp.png", embeds);
        */
    }
}

