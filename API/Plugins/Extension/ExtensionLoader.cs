using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Redox.API.Plugins.Extension
{
    public sealed class ExtensionLoader
    {
        public static readonly Dictionary<string, RedoxExtension> Extensions = new Dictionary<string, RedoxExtension>();

        private static readonly string Pattern = "Redox.*.dll";

        private static string path => Bootstrap.RedoxMod.AssemblePath;

        public static void Load()
        {
            List<string> files = new List<string>();

            foreach (var file in Directory.GetFiles(path, Pattern))
                files.Add(file);
            foreach (var file in Directory.GetFiles(Bootstrap.RedoxMod.ExtensionPath, Pattern))
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

                             //  Redox.Logger.LogColor(string.Format("[Redox] Succesfully loaded extension {0}, {1}, Author {2} ({3}", extension.Title, extension.Version, extension.Author, extension.Description), ConsoleColor.Yellow);
                            }
                        }
                    }
                    else
                    {
                       // Redox.Logger.LogWarning(string.Format("[Warning] Skipping extension {0} because its already loaded!", name));
                    }
                        
                }
                catch (Exception ex)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(string.Format("[Error] An exception has thrown while trying to load {0}, Error: " + ex));
                    System.Console.ResetColor();
                    continue;
                }
            }
            foreach (var ex in Extensions.Values)
                ex.OnExtensionsLoaded();
        }
    }
}
