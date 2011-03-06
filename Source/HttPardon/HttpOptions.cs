using System;
using System.Net;

namespace HttPardon
{
    internal class HttpOptions : IHttp
    {
        static readonly HttpOptionsCache _cache = new HttpOptionsCache();

        public string base_uri { get; set; }

        internal static HttpOptionsCache Cache
        {
            get { return _cache; }
        }

        void IHttp.base_uri(string uri)
        {
            base_uri = uri;
        }

        public HttpWebRequest CreateWebRequest()
        {
            var result = (HttpWebRequest) WebRequest.Create(base_uri);
            return result;
        }
    }
}