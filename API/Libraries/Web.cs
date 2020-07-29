using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    public class Web
    {

        internal static Queue<Request> RequestsQueue = new Queue<Request>();
        internal static List<Request> Requests = new List<Request>();

        /// <summary>
        /// Creates a GET request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="headers">List of headers.</param>
        public void Get(string url, Action<int, string> callback, string[] headers = null)
        {
            Request request = new Request(RequestType.Get, url, null, headers, callback);
            RequestsQueue.Enqueue(request);
        }
        /// <summary>
        /// Creates a POST request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="data">The data you want to post</param>
        /// <param name="headers">List of headers</param>
        public void Post(string url, Action<int, string> callback, string data, string[] headers = null)
        {
            Request request = new Request(RequestType.Post, url, data, headers, callback);
            RequestsQueue.Enqueue(request);
        }
        internal class Request
        {
            public RequestType Type { get; }

            public string Url { get; }

            public string[] Headers { get; }

            public string PostData { get; }
            public Action<int, string> CallBack { get; }

            public bool Complete { get; private set; } = false;

            internal Request(RequestType type, string url, string data, string[] headers, Action<int, string> callback)
            {
                Type = type;
                Url = url;
                PostData = data;
                Headers = headers;
                CallBack = callback;

               // Create();
            }

            internal async Task Create()
            {
                switch (Type)
                {
                    case RequestType.Get:
                        await CreateGetRequest();
                        break;
                    case RequestType.Post:
                        await CreatePostRequest();
                        break;
                }
            }

         
            private Task CreateGetRequest()
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Timeout = 2000;
                    if (Headers != null)
                    {
                        foreach (var x in Headers)
                            request.Headers.Add(x);
                    }

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        int code = (int)response.StatusCode;                     
                        string res = reader.ReadToEnd();
                        try
                        {
                            CallBack.Invoke(code, res);
                        }
                        catch (Exception ex)
                        {
                            Bootstrap.RedoxMod.Logger.LogError("[Redox] Failed to callback webrequest, error: " + ex.Message);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError("[Redox] Failed to create GET request, error: " + ex.Message);
                }
                finally
                {
                    Complete = true;
                }

                return Task.CompletedTask;
            }
            private  Task CreatePostRequest()
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    request.Timeout = 2000;
                    byte[] buffer = Encoding.ASCII.GetBytes(PostData);
                    request.ContentLength = buffer.Length;
                    if (Headers != null)
                    {
                        foreach (var x in Headers)
                            request.Headers.Add(x);
                    }
                    using(HttpWebResponse response  = (HttpWebResponse)request.GetResponse())
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream stream = request.GetRequestStream())
                            {
                                stream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        CallBack.Invoke((int)response.StatusCode, string.Empty);
                    }
                  
                }
                catch(Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError("[Redox] Failed to create post request, error: " + ex.Message);
                }
                finally
                {
                    Complete = true;
                }

                return Task.CompletedTask;
            }
        }
        internal enum RequestType
        {
            Get, Post
        }
        /*
        public static void CreateAsync(string url, Action<int, string> callback, string[] headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                int code = (int)response.StatusCode;

                callback.Invoke(code, reader.ReadToEnd());
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
        */
    }
}
