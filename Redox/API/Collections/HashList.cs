using System;
using System.Linq;
using System.Collections.Generic;

using Redox.API.Libraries;

namespace Redox.API.Collections
{
    /// <summary>
    /// Provides a serializable dictionary with extra features
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class HashList<TKey> : List<TKey>
    {
        public HashList() : base() { }


        /// <summary>
        /// Returns true or false when the map is empty
        /// </summary>
        public bool IsEmpty
        {
            get => base.Count == 0;
        }

        public new TKey this[int index]
        {
            get
            {
                if (index > (Count - 1))
                    return default(TKey);
                return base[index];
            }
            set
            {
                if (index <= (Count - 1))
                    base[index] = value;
            }
        }

        /// <summary>
        /// Adds the specific key to the hashlist
        /// </summary>
        /// <param name="key"></param>
        public new void Add(TKey key)
        {
            Type keyType = key.GetType();
            if (!base.Contains(key))
            {
                if (keyType.IsSerializable)
                {
                    base.Add(key);
                }
            }
        }
  
        public void ReadJson(string path)
        {
            var list = JSONHelper.FromFile<List<TKey>>(path);

            base.Clear();
            foreach (var x in list)
                this.Add(x);
            list.Clear();
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
