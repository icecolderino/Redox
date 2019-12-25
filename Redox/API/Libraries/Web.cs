using System;
using System.IO;
using System.Net;
using System.Text;



namespace Redox.API.Libraries
{
    public static class Web
    {
        public static void CreateAsync(string url, Action<int, string> callback, string method = "POST", string[] headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Headers.Add("Content-Type", "application/json");
            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                int code = (int)response.StatusCode;

                if (code != 202)
                    callback.Invoke(code, null);
                else
                {
                    string data = reader.ReadToEnd();
                    callback.Invoke(code, data);

                }
            }
        }

        public static string GET(string url)
        {
            using (WebClient web = new WebClient())
            {
                return web.DownloadString(url);
            }
        }
        
        public static void POST(string url, string data)
        {
            using (WebClient web = new WebClient())
            {
                web.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                web.UploadData(url, Encoding.ASCII.GetBytes(data));
            }
        }

    }
}
