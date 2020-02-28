using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace Redox.API.Libraries
{
    public enum DataType : ushort
    {
        None = 0,
        JSON = 1,
        XML = 2
    }

    public sealed class Rest
    {
        private RestClient _client;
        private RestRequest _request;
        private IRestResponse _response;

        private readonly string _username;
        private readonly string _password;

        private readonly string _url;

        public Rest(string url, string username, string password)
        {
            _url = url;
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Starts a new connection with the server
        /// </summary>
        public void Open()
        {
            _client = new RestClient(_url)
            {
                Authenticator = new HttpBasicAuthenticator(_username, _password)
            };
        }

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="dataType"></param>
        public void CreateRequest(string resource, DataType dataType = DataType.JSON)
        {
            DataFormat format = GetDataFormat(dataType);
            _request = new RestRequest(resource, format);
            _response = _client.Get(_request);
            
        }
        /// <summary>
        /// Create a async request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public async Task<T> CreateAsyncRequest<T>(string resource, DataType dataType = DataType.JSON) where T : new()
        {
            DataFormat format = GetDataFormat(dataType);
            _request = new RestRequest(resource, format);
            return await _client.GetAsync<T>(_request);
        }
        /// <summary>
        /// Create a post request
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="dataType"></param>
        public void Post(string resource, DataType dataType = DataType.JSON)
        {
            DataFormat format = GetDataFormat(dataType);
            _request = new RestRequest(resource, format);
            _client.Post(_request);
        }

        /// <summary>
        /// Create a async post request
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public async Task PostAsync(string resource, DataType dataType = DataType.JSON)
        {
            await Task.Run(() =>
            {
                Post(resource, dataType);
            });
           
        }

        public async Task PatchAsync(string resource, DataType dataType = DataType.JSON)
        {
            await Task.Run(() =>
            {
                DataFormat format = GetDataFormat(dataType);
                _request = new RestRequest(resource, format);
                _client.Patch(_request);
            });
        }


        public async Task DeleteAsync(string resource, DataType dataType = DataType.JSON)
        {
            await Task.Run(() =>
            {
                DataFormat format = GetDataFormat(dataType);
                _request = new RestRequest(resource, format);
                _client.Delete(_request);
            });
        }

        private DataFormat GetDataFormat(DataType dataType)
        {
            DataFormat format = DataFormat.Json;
            switch(dataType)
            {
                case DataType.None:
                    format = DataFormat.None;
                    break;
                case DataType.XML:
                    format = DataFormat.Xml;
                    break;
            }
            return format;
        }
    }
}
