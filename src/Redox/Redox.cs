
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
using Redox.API.Permissions;
using Redox.API.DependencyInjection;
using Redox.API.Plugins;
using Redox.API.Plugins.CSharp;
using Redox.API.Plugins.Javascript;
using Redox.API.Plugins.Extension;

namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private readonly static Assembly assembly = Assembly.GetExecutingAssembly();
        public static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static string RootPath { get; private set; } = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static string PluginPath { get; private set; } = Path.Combine(RootPath, "Plugins\\");
        public static string ExtensionPath { get; private set; } = Path.Combine(RootPath, "Extensions\\");
        public static string DependencyPath { get; private set; } = Path.Combine(RootPath, "Dependencies\\");
        public static string LibrariesPath { get; private set; } = Path.Combine(RootPath, "Libs\\");
        public static string DataPath { get; private set; } = Path.Combine(RootPath, "Data\\");
        public static string LoggingPath { get; private set; } = Path.Combine(RootPath, "Logs\\");
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
                    RootPath = customPath;
                    PluginPath = Path.Combine(RootPath, "Plugins\\");
                    ExtensionPath = Path.Combine(RootPath, "Extensions\\");
                    DependencyPath = Path.Combine(RootPath, "Dependencies\\");
                    LibrariesPath = Path.Combine(RootPath, "Libs\\");
                    DataPath = Path.Combine(RootPath, "Data\\");
                    LoggingPath = Path.Combine(RootPath, "Logs\\");
                }
                

                if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
                if (!Directory.Exists(LoggingPath)) Directory.CreateDirectory(LoggingPath);
                if (!Directory.Exists(ExtensionPath)) Directory.CreateDirectory(ExtensionPath);
                if (!Directory.Exists(DependencyPath)) Directory.CreateDirectory(DependencyPath);
                if (!Directory.Exists(LibrariesPath)) Directory.CreateDirectory(LibrariesPath);
                if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);
                if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

           
                string path = Path.Combine(RootPath, "Redox.json");
                if (File.Exists(path))
                    config = JSONHelper.FromFile<RedoxConfig>(path);
                else
                    JSONHelper.ToFile(path, config.Init());

                InterpreterAssemblies.Add(Assembly.GetAssembly(typeof(GameObject)));

               this.LoadDependencies();

                ExtensionLoader.Load();

                Logger = DependencyContainer.Resolve<ILogger>();

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
           
        }

        public void Update()
        {
            if(Web.Requests.Count <= 2)
            {
                if (Web.RequestsQueue.Count != 0 && Web.RequestsQueue.Peek() != null) 
                    Web.Requests.Add(Web.RequestsQueue.Dequeue());
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

        public static void Disable()
        {
            PermissionManager.Save();
            Logger.Log("[Redox] Preparing to shutdown..");

            LocalStorage.Save();
            PluginEngines.UnloadAll();
          
        }

        public class RedoxConfig
        {
            public string UnknownCommand;

            public bool PluginSecurity;
            public bool LoadIncompitablePlugins;
            public string[] WhitelistedAssemblyNames;

            public RedoxConfig Init()
            {
                UnknownCommand = "Unknown Command!";
                PluginSecurity = true;
                LoadIncompitablePlugins = false;
                WhitelistedAssemblyNames = new string[] { };                           
                return this;
            }
        }
    }
}

