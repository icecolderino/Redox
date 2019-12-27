
using System;
using System.IO;
using System.Reflection;

using UnityEngine;

using Redox.API.Libraries;
using Redox.Core.Plugin;


namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        private static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static readonly string DefaultPath = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static readonly string PluginPath = Path.Combine(DefaultPath, "Plugins\\");
        public static readonly string AssemblePath = Path.GetDirectoryName(assembly.Location);

        #endregion Paths

        void Start()
        {
            if (!Directory.Exists(DefaultPath)) Directory.CreateDirectory(DefaultPath);
            if (!Directory.Exists(PluginPath)) Directory.CreateDirectory(PluginPath);          
           
            DataStore.GetInstance().Save();
            PluginCollector.GetCollector();

            Logger.LogInfo("Loading Plugins..");
            PluginLoader.LoadPlugins();
        }


    }
}

