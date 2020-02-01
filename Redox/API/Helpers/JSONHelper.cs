using System;
using System.IO;
using System.Threading.Tasks;

using static Newtonsoft.Json.JsonConvert;

namespace Redox.API.Helpers
{
    public static class JSONHelper
    {
     
        public static string ToJson(object obj)
        {
            return SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
        
        public static T FromJson<T>(string value)
        {
            try
            {
                return DeserializeObject<T>(value);
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError("[JSONHelper] An exception has thrown while trying to Deserialize, Error: " + ex.Message);
                return default(T);
            }
            
        }

        public static void ToFile(string path, object obj)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    string json = ToJson(obj);
                    writer.Write(json);
                    writer.Close();
                    fs.Dispose();
                }
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[JSONHelper] An exception has thrown at {0}, Error: {1}", "JSONHelper.ToFile", ex.Message));
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

            string value = File.ReadAllText(path);
            return FromJson<T>(value);
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
