using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Redox.API.Plugins.Lua;
using UnityEngine;

namespace Redox.API
{
    public class Util
    {
        //private readonly NLua.Lua Engine;

        #region Lua functions

            /*
        /// <summary>
        /// Constructor for lua plugins
        /// </summary>
        /// <param name="engine"></param>
        public Util(ref NLua.Lua engine)
        {
            this.Engine = engine;
        }
        public Type GetType(string assemblyName, string typeName)
        {
            if (assemblyName.ToLower().Contains("system")) return null; //Lets block acces to the system namespace

            return Type.GetType($"{typeName},{assemblyName}");
        }

        public object GetPropertyGetter(Type type, string propertyName, BindingFlags flags, object instance)
        {
            try
            {
                if(type != null)
                {
                    var property = type.GetProperty(propertyName, flags);

                    if(property != null)
                    {
                        if(property.CanRead)
                        {
                            return property.GetValue(instance); 
                        }
                        return null;
                    }
                    return null;
                }
                return null;
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] Failed to GetPropertyGetter at {0} because of Error: {1}", propertyName, ex.Message));
                return null;
            }
        }
        public bool SetPropertySetter(Type type, string propertyName, BindingFlags flags, object instance, object value)
        {
            try
            {
                if(type != null)
                {
                    var property = type.GetProperty(propertyName, flags);

                    if (property != null)
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(instance, value);
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] Failed to SetPropertySetter at {0} because of Error: {1}", propertyName, ex.Message));
                return false;
            }

        }
        public object GetField(Type type, string fieldName, BindingFlags flags, object instance)
        {
            try
            {
                if(type != null)
                {
                    var field = type.GetField(fieldName, flags);

                    if(field != null)
                    {
                        return field.GetValue(instance);
                    }
                    return null;
                }
                return null;
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] Failed to GetField at {0} because of Error: {1}", fieldName, ex.Message));
                return null;
            }
        }
        public bool SetField(Type type, string fieldName, BindingFlags flags, object instance, object value)
        {
            try
            {
                if(type != null)
                {
                    var field = type.GetField(fieldName, flags);

                    if(field != null)
                    {
                        field.SetValue(instance, value);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] Failed to SetField at {0} because of Error: {1}", fieldName, ex.Message));
                return false;
            }
        }
        public MethodInfo FindMethod(Type type, string methodName, BindingFlags flags)
        {
            if (type != null)
            {
                if (methodName != "cctor")
                {
                    return type.GetMethod(methodName, flags);
                }
                return null;
            }
            return null;
           
        }
        public MethodInfo FindOverLoadMethod(Type type, string methodName, BindingFlags flags, string[] args)
        {
            try
            {
                if (type != null)
                {
                    if (methodName != "cctor")
                    {
                        var methods = type.GetMethods(flags);

                        if (methods.Length == 0)
                            return null;

                        MethodInfo methodInfo = null;
                        foreach (var method in methods)
                        {
                            if (method.Name == methodName)
                            {
                                var parameters = method.GetParameters();

                                if (parameters.Length != args.Length)
                                    return null;

                                bool[] matches = new bool[args.Length];

                                for (int i = 0; i < args.Length; i++)
                                {
                                    var para = parameters[i];
                                    var arg = args[i];

                                    //If the type of the parameters is the same as the  giving argument then lets set it to true
                                    if (para.GetType().FullName == arg)
                                        matches[i] = true;
                                    else
                                        matches[i] = false;
                                }
                                //If every element is true that means we found the correct method
                                if (matches.All(x => x))
                                {
                                    methodInfo = method;
                                    break;
                                }
                            }
                        }
                        return methodInfo;
                    }
                    return null;
                }
                return null;
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] Failed to  FindOverLoadMethod at {0} because of Error: {1}", methodName, ex.Message));
                return null;
            }
        }

        public LuaTable GetEnum(Type type)
        {
            Engine.NewTable("tempTable");
            LuaTable table = Engine["tempTable"] as LuaTable;
            if (table == null) return null;
            Engine["tempTable"] = null;
            Array values = Enum.GetValues(type);
            if (values == null) return null;

            for(int i = 0; i < values.Length; i++)
            {
                var val = values.GetValue(i);
                table[val.ToString()] = val;
            }
            return table;
        }

        /// <summary>
        /// Converts a LuaTable to .NET Dictionary
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Dictionary<string, object> TableToDictionary(LuaTable table)
        {
            try
            {          
                if (table.Keys.Count > 0 && table.Values.Count > 0)
                {
                    var dict = new Dictionary<string, object>(); 
                    foreach(var k in table.Keys)
                    {                       
                        string key = k.ToString();
                        if (dict.ContainsKey(key)) continue;
                        object value = table[k];
                                    
                        if (value is LuaTable)
                        {                             
                            var dict2 = TableToDictionary((LuaTable)value);
                            if (dict2 != null)
                            {                    
                                dict.Add(key.ToString(), ArrayFromTable((LuaTable)value));
                                continue;
                            }
                            dict.Add(key.ToString(), dict2); //This should never happen
                        }
                        else
                            dict.Add(key.ToString(), value);
                    }
                    return dict;                  
                }                                       
                return null;
                
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError("[Redox] Failed to convert luatable to dict, Error: " + ex);
                return null;
            }
        }

        /// <summary>
        /// Converts a LuaTable to .NET array
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Array ArrayFromTable(LuaTable table)
        {
            object[] array = new object[table.Keys.Count];

            for(int i = 0; i < table.Keys.Count; i++)
            {
                array[i] = table[i];
            }
            
            return array;
        }

        /// <summary>
        /// Converts a .NET array to luatable
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public LuaTable TableFromArray(Array array)
        {
            Engine.NewTable("_tempTable");
            LuaTable table = Engine["_tempTable"] as LuaTable;
            if (table == null) return null;
            Engine["_tempTable"] = null;

            for (int i = 0; i < array.Length; i++)
                table[i + 1] = array.GetValue(i);
            return table;
        }
        /// <summary>
        /// Converts a .NET Dictionary to luatable
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public LuaTable TableFromDictionary(object ob)
        {
            var dict = ob as Dictionary<string, object>;
            if(dict != null)
            {
                Engine.NewTable("_tempTable");
                LuaTable table = Engine["_tempTable"] as LuaTable;
                if (table == null) return null;
                Engine["_tempTable"] = null;

                foreach (var pair in dict)
                {
                    if(pair.Value is Dictionary<string, object>)
                    {
                        table[pair.Key] = TableFromDictionary(pair.Value);
                        continue;
                    }
                    table[pair.Key] = pair.Value;
                }
                    
                return table;
            }
            return null;
        }
        */
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

        #endregion
    }
}
