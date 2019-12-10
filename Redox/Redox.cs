using System;
using System.IO;
using System.Reflection;

using UnityEngine;

namespace Redox
{
    public sealed class Redox  : MonoBehaviour
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        private static readonly Version version = assembly.GetName().Version;

        #region Paths
        public static readonly string DefaultPath = Directory.GetCurrentDirectory() + "\\Redox\\";
        public static readonly string PluginPath = DefaultPath + "Plugins\\";

        #endregion Paths

        void Start()
        {
            //TODO: Add all codes here that should be executed at Initialize
        }


    }
}

