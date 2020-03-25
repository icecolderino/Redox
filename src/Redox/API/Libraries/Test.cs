using Redox.API.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    public class Test : Schedule
    {
        public override void Execute()
        {
        }

        public override string GetName()
        {
            return "Test schedule";
        }

        public override TimeSpan IntervalTime()
        {
            return new TimeSpan(10, 40, 10);
        }

        public override bool Repeatable()
        {
            return false;
        }
    }
}
