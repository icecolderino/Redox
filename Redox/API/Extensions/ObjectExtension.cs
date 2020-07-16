using System;


namespace Redox.API.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJson(this object ob)
        {
            return Utility.Json.ToJson(ob);
        }
    }
}
