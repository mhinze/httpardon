using System;
using System.ComponentModel;

namespace HttPardon
{
    public interface IGrammar
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();
    }


    public static class HttpCompositionExtensions
    {
        public static void http(this object extended, Action<IHttp> configuration)
        {
            if (configuration == null) return;

            var http = new HttpOptions();

            configuration(http);

            HttpOptions.Cache[extended] = http;
        }

        public static Response get(this object extended)
        {
            var httpOptions = HttpOptions.Cache[extended];
            return Http.get(httpOptions);
        }
    }


    public interface IHttp : IGrammar
    {
        void base_uri(string uri);
    }
}