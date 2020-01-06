using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Redox.Core.Extension
{
    public sealed class ExtensionLoader
    {
        private static readonly Dictionary<string, RedoxExtension> Extensions = new Dictionary<string, RedoxExtension>();

        private static readonly string Pattern = "*.dll";

        private static readonly string path = Redox.AssemblePath;

        public static void Load()
        {
            Console.WriteLine("[Redox] Loading extension...");

            List<string> files = new List<string>();


            foreach (var file in Directory.GetFiles(path, Pattern))
                files.Add(file);
            foreach (var file in Directory.GetFiles(Redox.ExtensionPath, Pattern))
                files.Add(file);

            foreach (var file in files)
            {
                try
                {
                    FileInfo info = new FileInfo(file);
                    string name = Path.GetFileNameWithoutExtension(file);

                    if (!Extensions.ContainsKey(name))
                    {
                        Assembly assembly = Assembly.Load(File.ReadAllBytes(file));

                        foreach (Type type in assembly.GetExportedTypes())
                        {
                            if (type.IsSubclassOf(typeof(RedoxExtension)) && type.IsPublic && !type.IsAbstract)
                            {
                                RedoxExtension extension = (RedoxExtension)Activator.CreateInstance(type);
                                extension.Init();
                                Extensions.Add(name, extension);

                                Console.ForegroundColor = ConsoleColor.Yellow;
                               Console.WriteLine(string.Format("[Redox] Succesfully loaded extension {0}, {1}, Author {2} ({3}", extension.Title, extension.Version, extension.Author, extension.Description));
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("[Warning] Skipping extension {0} because its already loaded!", name));
                    }
                        
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("[Error] An exception has thrown while trying to load {0}, Error: " + ex));
                }
            }
            foreach (var ex in Extensions.Values)
                ex.OnExtensionsLoaded();
        }
    }
}
