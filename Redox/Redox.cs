
using System;
using System.IO;
using System.Reflection;

using UnityEngine;

using Redox.API.Libraries;
using Redox.Core.Extension;


namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static readonly string DefaultPath = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static readonly string PluginPath = Path.Combine(DefaultPath, "Plugins\\");
        public static readonly string LoggingPath = Path.Combine(DefaultPath, "Logs\\");
        public static readonly string AssemblePath = Path.GetDirectoryName(assembly.Location);

        #endregion Paths

        void Start()
        {
            try
            {
                if (!Directory.Exists(DefaultPath)) Directory.CreateDirectory(DefaultPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);

                ExtensionLoader.Load();

                SQLiteConnector.GetInstance();
                DataStore.GetInstance().Save();
                PluginCollector.GetCollector();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[Error] Failed to load Redox, Error: {0}", ex));
            }
           
        }


    }
}

