using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HttPardon.Util;

namespace HttPardon
{
    internal class HttpOptions : IHttp
    {
        static readonly HttpOptionsCache _cache = new HttpOptionsCache();

        readonly IList<Action<HttpWebRequest>> _configurations = new List<Action<HttpWebRequest>>();

        public string BaseUri { get; set; }
        public dynamic AdditionalOptions { get; set; }

        internal static HttpOptionsCache Cache
        {
            get { return _cache; }
        }

        void IHttp.BaseUri(string uri)
        {
            if (!uri.StartsWith("http"))
                uri = "http://" + uri;
            BaseUri = uri;
        }

        public void BasicAuth(string username, string password)
        {
            _configurations.Add(x => { x.Credentials = new NetworkCredential(username, password); });
            _configurations.Add(req =>
            {
                var authInfo = username + ":" + password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                req.Headers["Authorization"] = "Basic " + authInfo;
            });
        }

        public void Configure(HttpWebRequest request)
        {
            foreach (var action in _configurations) action(request);
        }
    }
}