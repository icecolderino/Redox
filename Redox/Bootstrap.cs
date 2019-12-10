using System;
using UnityEngine;

namespace Redox
{
    public class Bootstrap
    {
        private static GameObject _mod;

        public static void Init()
        {
            _mod = new GameObject("Redox");
            _mod.AddComponent<Redox>();
            UnityEngine.Object.DontDestroyOnLoad(_mod);
         
        }
    }
}
