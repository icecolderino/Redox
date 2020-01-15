using System;

using Redox.API.Plugins;

namespace Redox
{
    public class AmazingPlugin : RedoxPlugin
    {
        private string Message1;
        private string Message2;
        private int Number;

        public override void Load()
        {
            if(!DefaultConfig.Exists())
            {
                DefaultConfig.AddSetting("Message1", "Amazing Message1");
                DefaultConfig.AddSetting("Message2", "Amazing Message2");
                DefaultConfig.AddSetting("Number", 600);
                DefaultConfig.Save();
            }
            else
            {
                DefaultConfig.LoadConfig();

                Message1 = (string)DefaultConfig.GetSetting("Message1");
                Message2 = (string)DefaultConfig.GetSetting("Message2");
                Number = (int)DefaultConfig.GetSetting("Number");
            }
        }

        public override void Unload()
        {
            //Nothing happend here, Yet
        }
    }
}
