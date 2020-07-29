using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Redox.Core.Data;
using Redox.Core.Plugins;

namespace Redox.API.Localization
{
    public sealed class TranslationList : IData
    {
        private readonly Plugin _plugin;
        private readonly string _path;
        
        private IList<Translation> _translations;

        public bool Exists
        {
            get
            {
                return File.Exists(_path);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _translations.Count == 0;
            }
        }
        
        /// <summary>
        /// Register a new translation.
        /// </summary>
        /// <param name="translation">The translation you want to register.</param>
        /// <returns></returns>
        public Task RegisterAsync(Translation translation)
        {
            if (translation == null) 
                return Task.CompletedTask;

            if (_translations.Any(x => x.Language != translation.Language))
            {
                _translations.Add((translation));
            }
            return Task.CompletedTask;
        }

        public async Task<string> TranslateAsync(CultureInfo culture, string key) =>
            await TranslateAsync(culture.Parent, key);
        
        public Task<string> TranslateAsync(string language, string key)
        {
            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(key))
                return Task.FromResult(string.Empty);

            Translation translation = _translations.FirstOrDefault(x => x.Language == language);
            if (translation != null)
            {
                string message = string.Empty;

                translation.TryGetValue(key, out message);

                return Task.FromResult(message);
            }

            return Task.FromResult(string.Empty);
        }

       

        public async Task SaveAsync()
        {
            await Utility.Json.ToFileAsync(_path, _translations);
        }

        public async Task LoadAsync()
        {
            _translations = await Utility.Json.FromFileAsync<IList<Translation>>(_path);
        }
        
        public TranslationList(Plugin plugin)
        {
            this._plugin = plugin;
            this._path = Path.Combine(plugin.FileInfo.DirectoryName, "Translation.json");
            this._translations = new List<Translation>();
        }

    }
}