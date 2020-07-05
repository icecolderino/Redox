
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Newtonsoft.Json;

using Redox.Core.PluginEngines;

using Redox.API;
using Redox.API.Libraries;
using Redox.API.Permissions;
using Redox.API.DependencyInjection;
using Redox.API.Plugins;
using Redox.API.Plugins.CSharp;
using Redox.API.Plugins.Javascript;
using Redox.API.Plugins.Extension;

namespace Redox
{
    public sealed class Redox
    {
        private readonly static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version version = assembly.GetName().Version;

        private readonly List<Assembly> dependencies = new List<Assembly>();
        private Stopwatch LifeTimeWatch;
        private Timer WebRequestTimer;

        #region Directories
        public string RootPath { get; private set; }
        public string PluginPath { get; private set; } 
        public string ExtensionPath { get; private set; }
        public string DependencyPath { get; private set; } 
        public string LibrariesPath { get; private set; } 
        public string DataPath { get; private set; } 
        public string LoggingPath { get; private set; } 
        public string AssemblePath { get; private set; }

        #endregion  Directories

        public API.ILogger Logger { get; private set; }
        public RedoxConfig Config { get; private set; }

        /// <summary>
        /// Gets the number of seconds since Redox got initialized (Usefull for time measurements).
        /// </summary>
        public float LifeTime
        {
            get
            {
                return (float)LifeTimeWatch.Elapsed.TotalSeconds;
            }
        }

       
        /// <summary>
        /// For interpreter engines only
        /// </summary>
        public static readonly Dictionary<string, object> InterpreterVariables = new Dictionary<string, object>();

        public static readonly List<Assembly> InterpreterAssemblies = new List<Assembly>();

     
        public async void Initialize(string customPath = "")
        {
            
            try
            {
                
                if (!string.IsNullOrEmpty(customPath))
                    RootPath = customPath;
                else
                    RootPath = Directory.GetCurrentDirectory() + "\\Redox\\";

                PluginPath = Path.Combine(RootPath, "Plugins\\");
                ExtensionPath = Path.Combine(RootPath, "Extensions\\");
                DependencyPath = Path.Combine(RootPath, "Dependencies\\");
                LibrariesPath = Path.Combine(RootPath, "Libs\\");
                DataPath = Path.Combine(RootPath, "Data\\");
                LoggingPath = Path.Combine(RootPath, "Logs\\");
                AssemblePath = Path.GetDirectoryName(assembly.Location);


                if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(DependencyPath)) Directory.CreateDirectory(DependencyPath);
                if (!Directory.Exists(LibrariesPath)) Directory.CreateDirectory(LibrariesPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

                Config = new RedoxConfig();
                string path = Path.Combine(RootPath, "Redox.json");
                if (File.Exists(path))
                    Config = Utility.Json.FromFile<RedoxConfig>(path);
                else
                    Utility.Json.ToFile(path, Config.Init());

               this.LoadDependencies();

                ExtensionLoader.Load();

                Logger = DependencyContainer.Resolve<API.ILogger>();

                PluginEngines.Register<CSPluginEngine>();
                PluginEngines.Register<JSEngine>();
                Logger.LogInfo("[Redox] Loading data...");

                await PermissionManager.Initialize();
                PermissionManager.CreateGroup("default");
                PermissionManager.CreateGroup("admin");

                Logger.LogInfo("[Redox] Loading standard library..");
                LocalStorage.Load();             
                PluginCollector.GetCollector();

            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[Error] Failed to load Redox, Error: {0}", ex));
            }
            LifeTimeWatch = new Stopwatch();
            LifeTimeWatch.Start();
            WebRequestTimer = Timers.Create(5, TimerType.Repeat, WebRequestUpdate);
        }
        
        public void WebRequestUpdate(Timer timer)
        {
            if(Web.Requests.Count <= 2)
            {
                if (Web.RequestsQueue.Count != 0 && Web.RequestsQueue.Peek() != null)
                {
                    var request = Web.RequestsQueue.Dequeue();
                    request.Create();
                    Web.Requests.Add(request);
                }
                    
            }

            if (Web.Requests.Count == 0)
                return;

            foreach(var request in Web.Requests)
            {
                if (request.Complete)
                    Web.Requests.Remove(request);
            }
        }


        private void LoadDependencies()
        {
            foreach (var file in Directory.GetFiles(DependencyPath, "*.dll"))
            { 
                 var assembly = Assembly.LoadFrom(file);
                 dependencies.Add(assembly);
            }
        }

        public void DeInitialize()
        {
            PermissionManager.Save();
            Logger.Log("[Redox] Preparing to shutdown..");

            LocalStorage.Save();
            PluginEngines.UnloadAll();
          
        }

        public class RedoxConfig
        {
            [JsonProperty("This message will be sent when an executed command doesn't exist.")]
            public string UnknownCommand { get; private set; }
            [JsonProperty("Plugin security prevents plugins from having resources.")]
            public bool PluginSecurity { get; private set; }
            [JsonProperty("Do you want outdated plugins to be loaded? (It's recommended to keep this disabled)")]
            public bool LoadIncompitablePlugins { get; private set; }

            [JsonProperty("Do you want Redox to log messages into the console?")]
            public bool Logging { get; private set; }

            [JsonProperty("Do you want to debug messages to show in the console? (This contains the load time of plugins)")]
            public bool DebugLogging { get; private set; }
            [JsonProperty("Enter here the plugin names you want to bypass the security check (Only works when PluginSecurity is enabled).")]
            public string[] WhitelistedAssemblyNames { get; private set; }

            public RedoxConfig Init()
            {
                UnknownCommand = "Unknown Command!";
                PluginSecurity = true;
                LoadIncompitablePlugins = false;
                Logging = true;
                DebugLogging = true;
                WhitelistedAssemblyNames = new string[] { };                           
                return this;
            }
        }
    }
}

