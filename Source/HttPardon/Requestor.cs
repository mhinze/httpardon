using System.Net;
using HttPardon.Details;
using HttPardon.Hashie;

namespace HttPardon
{
    /// <summary>
    ///     HttPardon's main interface<para />
    ///     responsible for issuing HTTP requests
    /// </summary>
    public class Requestor
    {
        readonly RubyHasher _hasher = new RubyHasher();

        readonly HttpWebRequestBuilder _requestBuilder = new HttpWebRequestBuilder();
        readonly ResponseBuilder _responseBuilder = new ResponseBuilder();

        internal Response Get(HttpOptions options)
        {
            options.Method = "get";
            return makeRequest(options);
        }

        public Response Get(string url, string optionsHash)
        {
            return makeRequest(new HttpOptions
            {
                Method = "get",
                BaseUri = url,
                AdditionalOptions = _hasher.Parse(optionsHash)
            });
        }



        internal Response Post(HttpOptions httpOptions)
        {
            httpOptions.Method = "post";
            return makeRequest(httpOptions);
        }

        public Response Post(string url, string optionsHash)
        {
            return makeRequest(new HttpOptions
            {
                Method = "post",
                AdditionalOptions = _hasher.Parse(optionsHash),
                BaseUri = url
            });
        }



        Response makeRequest(HttpOptions httpOptions)
        {
            var request = _requestBuilder.Build(httpOptions);

            var response = (HttpWebResponse) request.GetResponse();

            return _responseBuilder.Build(response);
        }
    }
}