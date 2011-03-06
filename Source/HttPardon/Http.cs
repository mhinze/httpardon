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
        public static Response get(string url)
        {
            return _requestor.Get(url);
        }

        internal static Response get(HttpOptions httpOptions)
        {
            return _requestor.Get(httpOptions);
        }

        internal static Response post(HttpOptions httpOptions)
        {
            return _requestor.Post(httpOptions);
        }
    }
}