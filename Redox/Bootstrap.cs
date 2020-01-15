using System;
using UnityEngine;

namespace Redox
{
    public class Bootstrap
    {
        private static GameObject _mod;

        public static void Init(string customPath)
        {
            _mod = new GameObject("Redox");
            _mod.AddComponent<Redox>().Initialize(customPath);
            UnityEngine.Object.DontDestroyOnLoad(_mod);
         
        }
    }
}
