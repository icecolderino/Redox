using System;

namespace Redox
{
    public static class Bootstrap
    {
        /// <summary>
        /// RedoxMod instance
        /// </summary>
        public static Redox RedoxMod { get; private set; }

        public static void Init(string customPath = "")
        {
            if (RedoxMod != null)
                return;

            RedoxMod = new Redox(customPath);
            RedoxMod.Initialize();
        }
    }
}
