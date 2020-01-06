
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

using Redox.API.Libraries;
using Redox.API.DependencyInjection;
using Redox.Core.Extension;
using Redox.API;
using Redox.Core.PluginEngines;

namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static readonly string DefaultPath = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static readonly string PluginPath = Path.Combine(DefaultPath, "Plugins\\");
        public static readonly string ExtensionPath = Path.Combine(DefaultPath, "Extensions\\");
        public static readonly string DataPath = Path.Combine(DefaultPath, "Data\\");
        public static readonly string LoggingPath = Path.Combine(DefaultPath, "Logs\\");
        public static readonly string AssemblePath = Path.GetDirectoryName(assembly.Location);

        #endregion Paths

        public static ILogger Logger;

        async void Start()
        {
            try
            {
                if (!Directory.Exists(DefaultPath)) Directory.CreateDirectory(DefaultPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);


               // await Permissions.Load();
             //   await Groups.Load();

                ExtensionLoader.Load();
                SQLiteConnector.GetInstance();
                DataStore.GetInstance();               
                PluginCollector.GetCollector();
               

                Logger = DependencyContainer.Resolve<ILogger>();

            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[Error] Failed to load Redox, Error: {0}", ex));
            }
           
        }

        public static async void Disable()
        {
            //   await Permissions.Save();
            //await Groups.Save();

            Logger.Log("[Redox] Preparing to shutdown..");

            PluginEngines.UnloadAll();
            DataStore.GetInstance().Save();

        }


    }
}

