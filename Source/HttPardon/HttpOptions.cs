using HttPardon.Util;

namespace HttPardon
{
    internal class HttpOptions : IHttp
    {
        static readonly HttpOptionsCache _cache = new HttpOptionsCache();

        public string BaseUri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        internal static HttpOptionsCache Cache
        {
            get { return _cache; }
        }

        public dynamic AdditionalOptions { get; set; }

        void IHttp.BaseUri(string uri)
        {
            BaseUri = uri;
        }

        public void BasicAuth(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}