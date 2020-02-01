using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace Redox.API.Libraries
{
    public static class Web
    {
        public static void CreateAsync(string url, Action<int, string> callback, string[] headers = null, string contentType = "application/json")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Content-Type", contentType);
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
     
        public static async Task POST(string url, string data, Action callBack, string[] Headers = null, string application = "application/json")
        {
            await Task.Run(() =>
            {
                using (WebClient web = new WebClient())
                {
                    web.Headers[HttpRequestHeader.ContentType] = application;

                    if (Headers != null)
                        foreach (string header in Headers)
                            web.Headers.Add(header);

                    web.UploadData(url, Encoding.ASCII.GetBytes(data));
                    callBack.Invoke();
                }
            });
         
        }
    }
}
