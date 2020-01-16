
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

using UnityEngine;

using Redox.API.Libraries;
using Redox.API.DependencyInjection;
using Redox.Core.Extension;
using Redox.API;
using Redox.Core.PluginEngines;
using System.Threading.Tasks;
using Redox.API.Permissions;

namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static string DefaultPath { get; private set; } = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static string PluginPath { get; private set; } = Path.Combine(DefaultPath, "Plugins\\");
        public static string ExtensionPath { get; private set; } = Path.Combine(DefaultPath, "Extensions\\");
        public static string DependencyPath { get; private set; } = Path.Combine(DefaultPath, "Dependencies\\");
        public static string DataPath { get; private set; } = Path.Combine(DefaultPath, "Data\\");
        public static string LoggingPath { get; private set; } = Path.Combine(DefaultPath, "Logs\\");
        public static string AssemblePath { get; private set; } = Path.GetDirectoryName(assembly.Location);

        #endregion Paths

        public static ILogger Logger;
        public static RedoxConfig config;

        private List<Assembly> dependencies = new List<Assembly>();

        public async void Initialize(string customPath)
        {
            try
            {

                if(!string.IsNullOrEmpty(customPath))
                {
                    DefaultPath = customPath;
                    PluginPath = Path.Combine(DefaultPath, "Plugins\\");
                    ExtensionPath = Path.Combine(DefaultPath, "Extensions\\");
                    DependencyPath = Path.Combine(DefaultPath, "Dependencies\\");
                    DataPath = Path.Combine(DefaultPath, "Data\\");
                    LoggingPath = Path.Combine(DefaultPath, "Logs\\");
                }


                if (!Directory.Exists(DefaultPath)) Directory.CreateDirectory(DefaultPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(DependencyPath)) Directory.CreateDirectory(DependencyPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);


                await this.LoadDependencies();

                string path = Path.Combine(DefaultPath, "Redox.json");
                if (File.Exists(path))
                    config = JSONHelper.FromFile<RedoxConfig>(path);
                else
                    JSONHelper.ToFile<RedoxConfig>(path, config.Init());

                ExtensionLoader.Load();

                Logger = DependencyContainer.Resolve<ILogger>();
                Logger.LogInfo("[Redox] Loading data...");

                await PermissionManager.Initialize();
                PermissionManager.CreateGroup("default");

                Logger.LogInfo("[Redox] Loading standard library...");
                SQLiteConnector.GetInstance();
                DataStore.GetInstance();               
                PluginCollector.GetCollector();

                //if MySQL enabled in settings, load user/pass etc then start MySQL
                Logger.LogInfo("[Redox] Starting MySQL...");
                MySQL.GetInstance().SetupNewConnection();
                if (MySQL.GetInstance().OpenConnection())//Test connection to MySQL
                    MySQL.GetInstance().CloseConnection();//Does it close?

            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[Error] Failed to load Redox, Error: {0}", ex));
            }
           
        }    
        private async Task LoadDependencies()
        {
            await Task.Run(() =>
            {
                foreach(var file in Directory.GetFiles(DependencyPath, "*.dll"))
                {
                    var assembly = Assembly.LoadFrom(file);
                    dependencies.Add(assembly);
                }
            });


        }

        public static async void Disable()
        {
            //   await Permissions.Save();
            //await Groups.Save();

            Logger.Log("[Redox] Preparing to shutdown..");

            PluginEngines.UnloadAll();
            await DataStore.GetInstance().Save();

        }

        public struct RedoxConfig
        {
            public string UnknownCommand;

            public bool PluginSecurity;

            public string[] WhitelistedAssemblyNames;

            public RedoxConfig Init()
            {
                UnknownCommand = "Unknown Command!";
                PluginSecurity = true;
                WhitelistedAssemblyNames = new string[] { };
                return this;
            }
        }
    }
}

