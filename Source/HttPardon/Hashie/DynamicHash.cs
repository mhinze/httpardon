using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using IronRuby.Builtins;

namespace HttPardon.Hashie
{
    // TODO 
    public partial class DynamicHash : DynamicObject, IDictionary<string, object>
    {
        readonly IDictionary<string, object> _dict = new Dictionary<string, object>();
        readonly Hash _hash;

        public DynamicHash(Hash hash)
        {
            _hash = hash;
            _dict = _hash.ToDictionary(kvp => kvp.Key.ToString(), kvp => GetResult(kvp.Value));
        }

        public DynamicHash(IEnumerable<KeyValuePair<string, object>> dictionary)
        {
            _dict = dictionary.ToDictionary(kvp => kvp.Key, kvp => GetResult(kvp.Value));
        }

        public DynamicHash()
        {
            _dict = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return GetFromDictionary(binder.Name, out result);
        }

        bool GetFromDictionary(string name, out object result)
        {
            if (_dict.TryGetValue(name, out result))
            {
                result = (dynamic) result;

                return true;
            }
            result = null;
            return true;
        }

        static object GetResult(object result)
        {
            var value_string = result as MutableString;
            if (value_string != null)
            {
                result = value_string.ToString();
                return result;
            }

            var value_hash = result as Hash;
            if (value_hash != null)
            {
                return GetResultFromHash(result, value_hash);
            }

            var value_array = result as RubyArray;
            if (value_array != null)
            {
                result = value_array.ToArray();
                return result;
            }


            return result;
        }

        static object GetResultFromHash(object result, Hash value_hash)
        {
            if (value_hash.Count > 1)
            {
                result = new DynamicHash(value_hash);
            }
            else if (value_hash.Count == 1)
            {
                var pair = value_hash.Single();
                var dictionary = new Dictionary<string, object> {{pair.Key.ToString(), pair.Value}};
                result = new DynamicHash(dictionary);
            }
            else if (value_hash.Count < 1)
            {
                result = new Dictionary<string, object>();
            }
            return result;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            return GetFromDictionary(index.ToString(), out result);
        }
    }

    #region explicit interface implementations
    public partial class DynamicHash
    {
        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            _dict.Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            _dict.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return _dict.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return _dict.Remove(item);
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { return _dict.Count; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return _dict.IsReadOnly; }
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            _dict.Add(key, value);
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return _dict.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return _dict.TryGetValue(key, out value);
        }

        object IDictionary<string, object>.this[string key]
        {
            get { return _dict[key]; }
            set { _dict[key] = this; }
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { return _dict.Keys; }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { return _dict.Values; }
        }
    }
    #endregion

}