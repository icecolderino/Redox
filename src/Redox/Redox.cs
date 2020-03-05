
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

using Redox.Core.PluginEngines;

using Redox.API;
using Redox.API.Helpers;
using Redox.API.Libraries;
using Redox.API.Collections;
using Redox.API.Permissions;
using Redox.API.DependencyInjection;
using Redox.API.Plugins;
using Redox.API.Plugins.CSharp;
using Redox.API.Plugins.Lua;
using Redox.API.Plugins.Extension;

namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private readonly static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static string DefaultPath { get; private set; } = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static string PluginPath { get; private set; } = Path.Combine(DefaultPath, "Plugins\\");
        public static string ExtensionPath { get; private set; } = Path.Combine(DefaultPath, "Extensions\\");
        public static string DependencyPath { get; private set; } = Path.Combine(DefaultPath, "Dependencies\\");
        public static string LibrariesPath { get; private set; } = Path.Combine(DefaultPath, "Libs\\");
        public static string DataPath { get; private set; } = Path.Combine(DefaultPath, "Data\\");
        public static string LoggingPath { get; private set; } = Path.Combine(DefaultPath, "Logs\\");
        public static string AssemblePath { get; private set; } = Path.GetDirectoryName(assembly.Location);

        #endregion Paths

        public static ILogger Logger;
        public static RedoxConfig config = new RedoxConfig();

        private readonly List<Assembly> dependencies = new List<Assembly>();

        /// <summary>
        /// For interpreter engines only
        /// </summary>
        public static readonly Dictionary<string, object> InterpreterVariables = new Dictionary<string, object>();

        public static readonly List<Assembly> InterpreterAssemblies = new List<Assembly>();

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
                    LibrariesPath = Path.Combine(DefaultPath, "Libs\\");
                    DataPath = Path.Combine(DefaultPath, "Data\\");
                    LoggingPath = Path.Combine(DefaultPath, "Logs\\");
                }
                

                if (!Directory.Exists(DefaultPath)) Directory.CreateDirectory(DefaultPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(DependencyPath)) Directory.CreateDirectory(DependencyPath);
                if (!Directory.Exists(LibrariesPath)) Directory.CreateDirectory(LibrariesPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

           
                string path = Path.Combine(DefaultPath, "Redox.yml");
                if (File.Exists(path))
                    config = YAMLHelper.FromFile<RedoxConfig>(path);
                else
                    YAMLHelper.ToFile(path, config.Init());

                InterpreterAssemblies.Add(typeof(GameObject).Assembly);

                await this.LoadDependencies();

                ExtensionLoader.Load();

                Logger = DependencyContainer.Resolve<ILogger>();

                PluginEngines.Register<CSPluginEngine>();
                PluginEngines.Register<LuaEngine>();
                Logger.LogInfo("[Redox] Loading data...");

                await PermissionManager.Initialize();
                PermissionManager.CreateGroup("default");
                PermissionManager.CreateGroup("admin");

                Logger.LogInfo("[Redox] Loading standard library..");
                SQLiteConnector.GetInstance();
                LocalStorage.GetStorage();              
                PluginCollector.GetCollector();

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

            await LocalStorage.GetStorage().Save();
            PluginEngines.UnloadAll();
          
        }

        public class RedoxConfig
        {
            public string UnknownCommand;

            public bool PluginSecurity;
            public bool LoadIncompitablePlugins;
            public string[] WhitelistedAssemblyNames;

            public HashMap<string, object> Rest;

            public RedoxConfig Init()
            {
                UnknownCommand = "Unknown Command!";
                PluginSecurity = true;
                LoadIncompitablePlugins = false;
                WhitelistedAssemblyNames = new string[] { };

                Rest = new HashMap<string, object>
                {
                    {"URL", "https://exampledomain.com"  },
                    {"Username", "username" },
                    {"password", "password" }
                };
                    
                return this;
            }
        }
    }
}

