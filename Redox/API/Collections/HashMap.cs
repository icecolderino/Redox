using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Redox.API.Helpers;

namespace Redox.API.Collections
{
    /// <summary>
    /// Provides a serializable dictionary with extra features
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class HashMap<TKey,TValue> : Dictionary<TKey, TValue>
    {
        public HashMap() : base() { }

        public HashMap(SerializationInfo info, StreamingContext context) : base(info, context) {  }
      
        /// <summary>
        /// Returns true or false when the map is empty
        /// </summary>
        public bool IsEmpty
        {
            get => base.Count == 0;
        }

        public new TValue this[TKey key]
        {
            get
            {
                TValue value;

                if(base.TryGetValue(key, out value))
                {
                    return value;
                }
                return default(TValue);
                
            }
            set
            {
                if (base.ContainsKey(key))
                    base[key] = value;
                else
                    Add(key, value);
            }
        }
        


        /// <summary>
        /// Adds the specific key and value to the hashmap
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            Type keyType = key.GetType();
            Type valueType = value.GetType();

            if(!base.ContainsKey(key))
            {
                if (keyType.IsSerializable && valueType.IsSerializable)
                {
                    base.Add(key, value);
                }
            }

          
        }

        public void ReadJson(string path)
        {
           var dict = JSONHelper.FromFile<Dictionary<TKey, TValue>>(path);

            base.Clear();
            foreach(KeyValuePair<TKey, TValue> pair in dict)
            {
                this.Add(pair.Key, pair.Value);
            }
            dict.Clear();
        }

        public void WriteJson(string path)
        {
            JSONHelper.ToFile(path, this);
        }
       
        public override string ToString()
        {
            return base.Count.ToString();
        }
    }
}
