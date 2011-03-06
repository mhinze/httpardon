using System;
using System.Collections.Generic;

namespace HttPardon.Util
{
    internal class HttpOptionsCache
    {
        readonly Cache<WeakReference, HttpOptions> _cache = new Cache<WeakReference, HttpOptions>();

        public HttpOptions this[object index]
        {
            get
            {
                var weakReference = findCached(index);
                if (weakReference == null)
                    throw new KeyNotFoundException("There is no cached WeakReference for the provided index");
                return _cache[weakReference];
            }
            set
            {
                var weakReference = findCached(index);
                _cache[weakReference ?? new WeakReference(index)] = value;
            }
        }

        WeakReference findCached(object index)
        {
            foreach (var wr in _cache.GetAllKeys())
            {
                if (!wr.IsAlive)
                {
                    _cache.Remove(wr);
                    continue;
                }
                if (wr.IsAlive && ReferenceEquals(wr.Target, index))
                    return wr;
            }
            return null;
        }
    }
}