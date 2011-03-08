using System;

namespace HttPardon
{
    /// <summary>
    ///     The starting point for issuing HTTP requests
    /// </summary>
    public static class Http
    {
        static readonly Requestor _requestor = new Requestor();

        /// <summary>
        ///     Performs an HTTP GET
        /// </summary>
        /// <param name = "url"></param>
        /// <returns></returns>
        public static Response get(string url, string optionsHash = null)
        {
            return _requestor.Get(url, optionsHash);
        }

        internal static Response get(HttpOptions httpOptions)
        {
            return _requestor.Get(httpOptions);
        }

        public static Response post(string url, string optionsHash = null)
        {
            return _requestor.Post(url, optionsHash);
        }

        internal static Response post(HttpOptions options)
        {
            return _requestor.Post(options);
        }

    }
}