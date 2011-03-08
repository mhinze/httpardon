using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HttPardon.Hashie;
using HttPardon.Util;

namespace HttPardon
{
    // TODO - I have the inclination to separate the semantic model from the configuration code    
    internal class HttpOptions : IHttp
    {
        public HttpOptions()
        {
            //defaults
            Method = "get";
            AdditionalOptions = new DynamicHash();
        }

        static readonly HttpOptionsCache _cache = new HttpOptionsCache();

        readonly IList<Action<HttpWebRequest>> _configurations = 
            new List<Action<HttpWebRequest>>();

        public string BaseUri { get; set; }
        public dynamic AdditionalOptions { get; set; }

        internal static HttpOptionsCache Cache
        {
            get { return _cache; }
        }

        string _method;
        public string Method
        {
            get { return _method.ToLower(); }
            set { _method = value; }
        }

        void IHttp.BaseUri(string uri)
        {
            if (!uri.StartsWith("http"))
                uri = "http://" + uri;
            BaseUri = uri;
        }

        public void BasicAuth(string username, string password)
        {
            _configurations.Add(x =>
            {
                x.Credentials = new NetworkCredential(username, password);
            });
            // this is how Net::HTTP implements it - the standard behavior of 
            // HttpWebRequest is different 
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