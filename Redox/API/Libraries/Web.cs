using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace Redox.API.Libraries
{
    public static class Web
    {
        public static void CreateAsync(string url, Action<int, string> callback, string method = "GET", string[] headers = null)
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
        public static async Task GET(string url, Action<string> callBack)
        {
            await Task.Run(() =>
            {
                using (WebClient web = new WebClient())
                {
                    string json = web.DownloadString(url);
                    callBack.Invoke(json);
                }              
            });
           
        }
        

        public static async Task POST(string url, string data, Action callBack, string application = "application/x-www-form-urlencoded")
        {
            await Task.Run(() =>
            {
                using (WebClient web = new WebClient())
                {
                    web.Headers[HttpRequestHeader.ContentType] = application;
                    web.UploadData(url, Encoding.ASCII.GetBytes(data));
                    callBack.Invoke();
                }
            });
         
        }
    }
}
