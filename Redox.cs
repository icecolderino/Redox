
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
using Redox.Core.Permissions;
using Redox.Core.Commands;
using Redox.API.Commands;
using Redox.API.Configuration;

using Autofac;

namespace Redox
{
    public sealed class Redox
    {
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        public static readonly Version Version = Assembly.GetName().Version;

        private readonly List<Assembly> _dependencies = new List<Assembly>();
        private Stopwatch _lifeTimeWatch;
        private Timer _webRequestTimer;
        private readonly string _customPath;

        #region Directories
        public string RootPath { get; private set; }
        public string PluginPath { get; private set; } 
        public string ExtensionPath { get; private set; }
        public string DependencyPath { get; private set; } 
        public string DataPath { get; private set; } 
        public string LoggingPath { get; private set; } 
        public string AssemblePath { get; private set; }

        #endregion  Directories

        #region Properties
        public RedoxConfig Config { get; private set; }
        
        public ContainerBuilder Builder { get; internal set; }
        public IContainer Container { get; internal set; }
        public API.ILogger Logger { get; internal set; }
        public IRoleProvider RoleManager { get; internal set; }
        public IPermissionProvider PermissionManager { get; internal set; }
        public PluginEngineManager EngineManager { get; internal set; }
        public PluginManager Plugins { get; internal set; }
        public DataStore Storage { get; internal set; }
        public Timers Timers { get; internal set; }
        #endregion
        public static Redox Mod
        {
            get
            {
                return Bootstrap.RedoxMod;
            }
        }

        /// <summary>
        /// Gets the number of seconds since Redox got initialized (Useful for time measurements).
        /// </summary>
        public float LifeTime
        {
            get
            {
                return (float)_lifeTimeWatch.Elapsed.TotalSeconds;
            }
        }

        public Redox(string customPath = "")
        {
            _customPath = customPath;         
        }
        public async void Initialize()
        {
            try
            {
                
                if (!string.IsNullOrEmpty(_customPath))
                    RootPath = _customPath;
                else
                    RootPath = Directory.GetCurrentDirectory() + "\\Redox\\";

                PluginPath = Path.Combine(RootPath, "Plugins\\");
                ExtensionPath = Path.Combine(RootPath, "Extensions\\");
                DependencyPath = Path.Combine(RootPath, "Dependencies\\");
                DataPath = Path.Combine(RootPath, "Data\\");
                LoggingPath = Path.Combine(RootPath, "Logs\\");
                AssemblePath = Path.GetDirectoryName(Assembly.Location);


                if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(DependencyPath)) Directory.CreateDirectory(DependencyPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
               // Utility.ExtractEssentials();
                Config = new RedoxConfig();
                string path = Path.Combine(RootPath, "Redox.json");
                if (File.Exists(path))
                    Config = Utility.Json.FromFile<RedoxConfig>(path);
                else
                    Utility.Json.ToFile(path, Config.Init());

                this.EnableCertificates();
                this.LoadDependencies();
                ContainerConfig.Configure();
                ExtensionLoader.Load();
                Container = Builder.Build();
                ContainerConfig.ResolveAll();
                Logger.LogInfo("[Redox] Initializing RedoxMod..");
                Logger.LogInfo("[Redox] Loading data...");
                await PermissionManager.LoadAsync();
                _lifeTimeWatch = Stopwatch.StartNew();
                _webRequestTimer = Timers.Create(5, TimerType.Repeat, WebRequestUpdate);
                Logger.LogInfo($"[Redox] RedoxMod V{Version} has been initialized.");
                EngineManager.StartAll();
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Failed to load Redox, Error: {ex}");
            }
         
        }

       

        private void EnableCertificates()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        private bool AcceptAllCertifications(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public async void WebRequestUpdate(Timer timer)
        {
            if(Web.Requests.Count <= 2)
            {
                if (Web.RequestsQueue.Count != 0 && Web.RequestsQueue.Peek() != null)
                {
                    var request = Web.RequestsQueue.Dequeue();
                    Web.Requests.Add(request);
                    await request.Create();
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
                 _dependencies.Add(assembly);
            }
        }

        public async void DeInitialize()
        {            
            Logger.Log("[Redox] Preparing to shutdown..");

            EngineManager.UnloadAll();
            await Storage.SaveAsync();
            await PermissionManager.SaveAsync();
            await RoleManager.SaveAsync();
        }
    }
}

