using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Redox.API.Plugins.Lua;

using NLua;

using UnityEngine;

namespace Redox.API
{
    public class Util
    {
        public readonly NLua.Lua Engine;

        #region Lua functions

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
        public MethodInfo FindOverLoadMethod(Type type, string methodName, BindingFlags flags, object[] args)
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
                                    if (para.GetType() == arg.GetType())
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

        public Dictionary<string, object> TableToDictionary(LuaTable table)
        {
           
            if (table.Keys.Count > 0 && table.Values.Count > 0)
            {
                var dict = new Dictionary<string, object>();
                foreach (var key in table.Keys)
                {
                    foreach(var value in table.Values)
                    {
                        if(value is LuaTable)
                        {
                            var arr = ArrayFromTable((LuaTable)value);
                            if(arr != null)
                            {
                                dict.Add(key.ToString(), arr);
                                continue;
                            }
                            else
                            {
                                var dict2 = TableToDictionary((LuaTable)value);
                                if(dict2 != null)
                                {
                                    dict.Add(key.ToString(), dict2);
                                    continue;
                                }
                            }
                            dict.Add(key.ToString(), null); //This should never happen
                        }
                        dict.Add(key.ToString(), value);
                    }
                }             
                return dict;
            }
            return null;
        }
        public object[] ArrayFromTable(LuaTable table)
        {
            if (table.Keys.Count > 0 && table.Values.Count == 0)
            {
                object[] arr = new object[table.Keys.Count];

                for(int i = 1; i < table.Keys.Count; i++)
                {
                    arr[i - 1] = table[i];
                }
                return arr;
            }
            return null;
        }
        public LuaTable TableFromArray(object[] array)
        {
            Engine.NewTable("_tempTable");
            LuaTable table = Engine["_tempTable"] as LuaTable;
            if (table == null) return null;
            Engine["_tempTable"] = null;

            for (int i = 0; i < array.Length; i++)
                table[i + 1] = array[i];
            return table;
        }
        public LuaTable TableFromDictionary(Dictionary<object, object> dict)
        {
            Engine.NewTable("_tempTable");
            LuaTable table = Engine["_tempTable"] as LuaTable;
            if (table == null) return null;
            Engine["_tempTable"] = null;

            foreach (var pair in dict)
                table[pair.Key] = pair.Value;
            return table;
        }
        #endregion
    }
}
