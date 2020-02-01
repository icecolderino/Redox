using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using YamlDotNet.Serialization;

namespace Redox.API.Helpers
{
    public static class YAMLHelper
    {
        private static Serializer serializer = new Serializer();
        private static Deserializer deserializer = new Deserializer();

        public static string ToYaml(object obj)
        {
            return serializer.Serialize(obj);
        }
        public static T FromYaml<T>(string yaml)
        {
            return deserializer.Deserialize<T>(yaml);
        }

        public static void ToFile(string path, object obj)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter stream = new StreamWriter(fs))
                    {
                        stream.Write(ToYaml(obj));
                        stream.Dispose();
                        fs.Dispose();
                    }
                }
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[YAMLHelper] An exception has thrown at {0}, Error: {1}", "YAMLHelper.ToFile", ex.Message));
            }
          
        }

        public static async Task ToFileAsync(string path, object obj)
        {
            await Task.Run(() =>
            {
                ToFile(path, obj);
            });
        }

        public static T FromFile<T>(string path)
        {
            if (!File.Exists(path))
                return default(T);
            return FromYaml<T>(File.ReadAllText(path));
        }

        public static async Task<T> FromFileAsync<T>(string path)
        {
            await Task.Run(() =>
            {
                return FromFile<T>(path);
            });
            return default(T);
        }
    }
}
