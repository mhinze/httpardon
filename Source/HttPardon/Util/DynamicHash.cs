using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using IronRuby.Builtins;

namespace HttPardon.Util
{
    public class DynamicHash : DynamicObject, IDictionary<string, object>
    {
        readonly Hash _hash;
        readonly IDictionary<string, object> _dict = new Dictionary<string, object>();

        public DynamicHash(Hash hash)
        {
            _hash = hash;
            _dict = _hash.ToDictionary(kvp => kvp.Key.ToString(), kvp => GetResult(kvp.Value));
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return getFromHash(binder.Name, out result);
        }

        bool getFromHash(string name, out object result)
        {
            if (_hash.TryGetValue(MutableString.Create(name), out result))
            {
                result = (dynamic) GetResult(result);

                return true;
            }
            return false;
        }

        static object GetResult(object result)
        {
            var value_hash = result as Hash;
            if (value_hash != null)
            {
                if (value_hash.Count > 1)
                {
                    result = new DynamicHash(value_hash);
                }
                else
                {
                    var pair = value_hash.First();
                    IDictionary<string, object> r = new ExpandoObject();
                    r.Add(pair.Key.ToString(), pair.Value);
                    result = (dynamic) r;
                }
            }

            var value_array = result as RubyArray;
            if (value_array != null)
            {
                result = value_array.ToArray();
            }
            var value_string = result as MutableString;
            if (value_string != null)
            {
                result = value_string.ToString();
            }
            return result;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            return getFromHash(index.ToString(), out result);
        }

        #region Implementation of IEnumerable

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<string,object>>

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

        #endregion

        #region Implementation of IDictionary<string,object>

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

        #endregion
    }
}