
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

using Newtonsoft.Json;

using Redox.API;
using Redox.API.Libraries;
using Redox.API.Permissions;
using Redox.API.Plugins;
using Redox.API.Plugins.CSharp;
using Redox.API.Plugins.Extension;

using Redox.Core.Plugins;
using Autofac;
using Redox.Core.Permissions;
using Redox.Core.Commands;
using Redox.API.Commands;

namespace Redox
{
    public sealed class Redox
    {
        private readonly static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version Version = assembly.GetName().Version;

        private readonly List<Assembly> dependencies = new List<Assembly>();
        private Stopwatch LifeTimeWatch;
        private Timer WebRequestTimer;
        private string CustomPath;

        #region Directories
        public string RootPath { get; private set; }
        public string PluginPath { get; private set; } 
        public string ExtensionPath { get; private set; }
        public string DependencyPath { get; private set; } 
        public string DataPath { get; private set; } 
        public string LoggingPath { get; private set; } 
        public string AssemblePath { get; private set; }

        #endregion  Directories

        public API.ILogger Logger { get; private set; }
        public IRoleProvider RoleManager { get; private set; }
        public IGroupProvider GroupManager { get; private set; }
        public IPermissionProvider PermissionManager { get; private set; }
        public RedoxConfig Config { get; private set; }
        public IContainer Container { get; private set; }
        public static Redox Mod
        {
            get
            {
                return Bootstrap.RedoxMod;
            }
        }

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

        public Redox(string customPath = "")
        {
            CustomPath = customPath;         
        }
        public async void Initialize()
        {
            
            try
            {
                
                if (!string.IsNullOrEmpty(CustomPath))
                    RootPath = CustomPath;
                else
                    RootPath = Directory.GetCurrentDirectory() + "\\Redox\\";

                PluginPath = Path.Combine(RootPath, "Plugins\\");
                ExtensionPath = Path.Combine(RootPath, "Extensions\\");
                DependencyPath = Path.Combine(RootPath, "Dependencies\\");
                DataPath = Path.Combine(RootPath, "Data\\");
                LoggingPath = Path.Combine(RootPath, "Logs\\");
                AssemblePath = Path.GetDirectoryName(assembly.Location);



                if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(DependencyPath)) Directory.CreateDirectory(DependencyPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

                Config = new RedoxConfig();
                string path = Path.Combine(RootPath, "Redox.json");
                if (File.Exists(path))
                    Config = Utility.Json.FromFile<RedoxConfig>(path);
                else
                    Utility.Json.ToFile(path, Config.Init());

                this.EnableCertificates();
                this.LoadDependencies();    
                ExtensionLoader.Load();
                this.Container = ContainerConfig.Configure();
                this.BuildContainer();
                Logger = Container.Resolve<ILogger>();
                Logger.LogInfo("[Redox] Initializing RedoxMod..");
                PluginEngineManager.Register<CSPluginEngine>();
                Logger.LogInfo("[Redox] Loading data...");

                await PermissionManager.LoadAsync();
                await GroupManager.LoadAsync();
                await RoleManager.LoadAsync();
                await RoleManager.CreateRoleAsync(new Role("default", "default role", 0, false));
                await GroupManager.CreateGroupAsync(new Group("default", "default group for players.", "default", 0, true, false));

                await RoleManager.AddGroupAsync("default", "default");

                Logger.LogInfo("[Redox] Loading standard library..");
                LocalStorage.Load();             
                PluginCollector.GetCollector();
                Logger.LogInfo($"[Redox] RedoxMod V{Version} has been initialized.");

            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[Error] Failed to load Redox, Error: {0}", ex));
            }
            LifeTimeWatch = Stopwatch.StartNew();
            WebRequestTimer = Timers.Create(5, TimerType.Repeat, WebRequestUpdate);
        }

       

        private void EnableCertificates()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        private bool AcceptAllCertifications(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
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

        public async void DeInitialize()
        {            
            Logger.Log("[Redox] Preparing to shutdown..");

            PluginEngineManager.UnloadAll();
            LocalStorage.Save();
            await PermissionManager.SaveAsync();
        }
        private void BuildContainer()
        {
           
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

