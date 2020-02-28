using System;

using UnityEngine;

namespace Redox.API.Extensions
{
    public static class VectorExtension
    {
        public static Vector3 FromVector3String(this string str)
        {
            try
            {
                if (str.StartsWith("(") && str.EndsWith(")"))
                {
                    string[] arr = str.Substring(1, str.Length - 2).Split(',');

                    if(arr.Length == 3)
                    {
                        float x = float.Parse(arr[0]);
                        float y = float.Parse(arr[1]);
                        float z = float.Parse(arr[2]);

                        return new Vector3(x, y, z);
                    }
                    return Vector3.zero;
                }
                return Vector3.zero;
            }
            catch
            {
                return Vector3.zero;
            }
        }

        public static Vector2 FromVector2String(this string str)
        {
            try
            {
                if (str.StartsWith("(") && str.EndsWith(")"))
                {
                    string[] arr = str.Substring(1, str.Length - 2).Split(',');

                    if (arr.Length == 2)
                    {
                        float x = float.Parse(arr[0]);
                        float y = float.Parse(arr[1]);

                        return new Vector2(x, y);
                    }
                    return Vector2.zero;
                }
                return Vector2.zero;
            }
            catch
            {
                return Vector2.zero;
            }
        }
    }
}
