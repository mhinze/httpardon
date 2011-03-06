using System;
using System.Net;
using HttPardon.Details;

namespace HttPardon
{
    /// <summary>
    ///     HttPardon's main interface<para />
    ///     responsible for issuing HTTP requests
    /// </summary>
    public class Requestor
    {
        readonly HttpWebRequestBuilder _requestBuilder = new HttpWebRequestBuilder();
        readonly ResponseBuilder _responseBuilder = new ResponseBuilder();

        public Response Get(string url)
        {
            var httpOptions = new HttpOptions {BaseUri = url};
            return GetResponse(httpOptions);
        }

        internal Response Get(HttpOptions options)
        {
            return GetResponse(options);
        }

        Response GetResponse(HttpOptions httpOptions)
        {
            var request = _requestBuilder.Build(httpOptions);

            var response = (HttpWebResponse) request.GetResponse();

            return _responseBuilder.Build(response);
        }

        internal Response Post(HttpOptions httpOptions)
        {
            throw new NotImplementedException();
        }
    }
}