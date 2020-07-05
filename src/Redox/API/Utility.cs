using System;
using System.IO;
using System.Xml;
using System.Threading.Tasks;

using static Newtonsoft.Json.JsonConvert;

using System.Collections.Generic;

namespace Redox.API
{
    public class Utility
    {
        public static class Json
        {
            public static string ToJson(object obj)
            {
                return SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            }

            public static object FromJson(string value)
            {
                return FromJson<object>(value);
            }
            public static T FromJson<T>(string value)
            {
                try
                {
                    return DeserializeObject<T>(value);
                }
                catch (Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError("[JSONHelper] An exception has thrown while trying to Deserialize, Error: " + ex.Message);
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
                    }
                }
                catch (Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError(string.Format("[JSONHelper] An exception has thrown at {0}, Error: {1}", "JSONHelper.ToFile", ex.Message));
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

        public static class Xml
        {    
            public static Dictionary<string, string> ToDictionary(string xml)
            {
                if (string.IsNullOrEmpty(xml))
                    return null;
                var dict = new Dictionary<string, string>();
                try
                {
                  
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);
                    XmlNodeList nodes = document.ChildNodes;
                    for(int i = 0; i < nodes.Count; i++)
                    {
                        var node = nodes[i];
                        var key = node.Name;
                        var value = node.InnerText;
                        dict.Add(key, value);
                    }
                    return dict;
                }
                catch(Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogException(ex);
                    return null;
                }

            }
        }
        /*
        public Array CreateEmptyArray()
        {
            return Array.Empty<object>();
        }

        public Vector3 CreateEmptyVector3()
        {
            return new Vector3();
        }
        public Vector3 CreateVector3(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }

        private readonly DateTime epoch = new DateTime(1970, 1, 1);
        public uint GetTimeStamp()
        {
            DateTime utc = DateTime.UtcNow;
            return (uint)(epoch - utc).TotalSeconds;
        }
        */
    }
}
