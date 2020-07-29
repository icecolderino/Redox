using System.IO;
using System.Collections.Generic;

using Redox.Core.Configuration;
using Redox.Core.Plugins;
using System.Threading.Tasks;

namespace Redox.API.Configuration
{

    
    /// <summary>
    /// Represents a json configuration
    /// </summary>
    public class Configuration : IConfiguration
    {
             
        private readonly Plugin _plugin;
        private readonly string _path;
        private Dictionary<string, object> _settings;

        public bool Exists
        {
            get
            {
                return File.Exists(_path);
            }
        }

        public Configuration(string name, Plugin plugin)
        {
            _settings = new Dictionary<string, object>();
            this._plugin = plugin;
            this._path = Path.Combine(this._plugin.FileInfo.DirectoryName, name + ".json");
        }

        public object this[string key]
        {
            get
            {
                if (_settings.ContainsKey(key))
                    return _settings[key];
                return null;
            }   
        }
        public Task AddSettingAsync(string key, object value)
        {
           
            if (!_settings.ContainsKey(key))
            {
                _settings.Add(key, value);
            }
            return Task.CompletedTask;   
                
        }

        public async Task SetSettingAsync(string key, object value)
        {
           // this[key] = value;
            
            if (_settings.ContainsKey(key))
                _settings[key] = value;
            else
                await AddSettingAsync(key, value);
        }
        public Task<object> GetSettingAsync(string key)
        {
          //  return this[key];
            
            if (_settings.ContainsKey(key))
                return Task.FromResult(_settings[key]);
            return null;
           
        }  
        public Task<bool> TryGetSettingAsync(string key, out object ob)
        {
            if(_settings.ContainsKey(key))
            {
                ob = _settings[key];
                return Task.FromResult(true);
            }
            ob = null;
            return Task.FromResult(false);
        }

        public Task<bool> HasSettingAsync(string key)
        {
            return Task.FromResult(_settings.ContainsKey(key));
        }
        public Dictionary<string, object> GetSettings()
        {
            return _settings;
        }
        public async Task LoadAsync()
        {
            _settings = await Utility.Json.FromFileAsync<Dictionary<string, object>>(_path);              
        }
        public async Task SaveAsync()
        {
            
            await Utility.Json.ToFileAsync(_path, _settings);       
        }

        public void Write(object obj)
        {
            Utility.Json.ToFile(_path, obj);
        }
        public T Read<T>()
        {
            if(Exists)
            {
                return Utility.Json.FromFile<T>(_path);
            }
            return default(T);
        }
    }
    
}
  