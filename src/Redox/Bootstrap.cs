using System;
using UnityEngine;

namespace Redox
{
    public class Bootstrap
    {
       public static GameObject Mod;

        public static void Init(string customPath)
        {
            Mod = new GameObject("Redox");
            Mod.AddComponent<Redox>().Initialize(customPath);
            UnityEngine.Object.DontDestroyOnLoad(Mod);
         
        }
    }
}
