using System;

using Redox.API.Helpers;

namespace Redox.API.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJson(this object ob)
        {
            return JSONHelper.ToJson(ob);
        }
    }
}
