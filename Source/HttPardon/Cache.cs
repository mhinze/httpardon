/* Cache.cs borrowed from FubuMvc:
  
  Copyright 2011 Jeremy Miller, Chad Myers, Joshua Flanagan, Robert Greyling, Joshua Arnold, Brandon Behrens, and Tim Tyrrell
  Copyright 2009-2010 Chad Myers, Jeremy Miller, Joshua Flanagan, Mark Nijhof

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HttPardon
{
    [Serializable]
    public class Cache<TKey, TValue> : IEnumerable<TValue>
    {
        readonly object _locker = new object();
        readonly IDictionary<TKey, TValue> _values;

        Func<TValue, TKey> _getKey = delegate { throw new NotImplementedException(); };

        Action<TValue> _onAddition = x => { };

        Func<TKey, TValue> _onMissing = delegate(TKey key)
        {
            var message = string.Format("Key '{0}' could not be found", key);
            throw new KeyNotFoundException(message);
        };

        public Cache()
            : this(new Dictionary<TKey, TValue>()) {}

        public Cache(Func<TKey, TValue> onMissing)
            : this(new Dictionary<TKey, TValue>(), onMissing) {}

        public Cache(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> onMissing)
            : this(dictionary)
        {
            _onMissing = onMissing;
        }

        public Cache(IDictionary<TKey, TValue> dictionary)
        {
            _values = dictionary;
        }

        public Action<TValue> OnAddition
        {
            set { _onAddition = value; }
        }

        public Func<TKey, TValue> OnMissing
        {
            set { _onMissing = value; }
        }

        public Func<TValue, TKey> GetKey
        {
            get { return _getKey; }
            set { _getKey = value; }
        }

        public int Count
        {
            get { return _values.Count; }
        }

        public TValue First
        {
            get
            {
                foreach (var pair in _values)
                {
                    return pair.Value;
                }

                return default(TValue);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                FillDefault(key);

                return _values[key];
            }
            set
            {
                _onAddition(value);
                if (_values.ContainsKey(key))
                {
                    _values[key] = value;
                }
                else
                {
                    _values.Add(key, value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TValue>) this).GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }

        /// <summary>
        /// Guarantees that the Cache has the default value for a given key.
        /// If it does not already exist, it's created.
        /// </summary>
        /// <param name="key"></param>
        public void FillDefault(TKey key)
        {
            if (!_values.ContainsKey(key))
            {
                lock (_locker)
                {
                    if (!_values.ContainsKey(key))
                    {
                        var value = _onMissing(key);
                        _onAddition(value);
                        _values.Add(key, value);
                    }
                }
            }
        }

        public void Fill(TKey key, TValue value)
        {
            if (_values.ContainsKey(key))
            {
                return;
            }

            _values.Add(key, value);
        }

        public void Each(Action<TValue> action)
        {
            foreach (var pair in _values)
            {
                action(pair.Value);
            }
        }

        public void Each(Action<TKey, TValue> action)
        {
            foreach (var pair in _values)
            {
                action(pair.Key, pair.Value);
            }
        }

        public bool Has(TKey key)
        {
            return _values.ContainsKey(key);
        }

        public bool Exists(Predicate<TValue> predicate)
        {
            var returnValue = false;

            Each(delegate(TValue value) { returnValue |= predicate(value); });

            return returnValue;
        }

        public TValue Find(Predicate<TValue> predicate)
        {
            foreach (var pair in _values)
            {
                if (predicate(pair.Value))
                {
                    return pair.Value;
                }
            }

            return default(TValue);
        }

        public TKey[] GetAllKeys()
        {
            return _values.Keys.ToArray();
        }

        public TValue[] GetAll()
        {
            return _values.Values.ToArray();
        }

        public void Remove(TKey key)
        {
            if (_values.ContainsKey(key))
            {
                _values.Remove(key);
            }
        }

        public void ClearAll()
        {
            _values.Clear();
        }

        public bool WithValue(TKey key, Action<TValue> callback)
        {
            if (Has(key))
            {
                callback(this[key]);
                return true;
            }

            return false;
        }

        public IDictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(_values);
        }
    }
}