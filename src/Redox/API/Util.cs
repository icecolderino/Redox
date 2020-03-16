using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

namespace Redox.API
{
    public class Util
    {

        public Array CreateEmptyArray()
        {
            return Array.Empty<object>();
        }

        public Vector3 CreateEmptyVector3()
        {
            return new Vector3();
        }
        public Vector3 CreateVector3(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }

        private readonly DateTime epoch = new DateTime(1970, 1, 1);
        public uint GetTimeStamp()
        {
            DateTime utc = DateTime.UtcNow;
            return (uint)(epoch - utc).TotalSeconds;
        }
    }
}
