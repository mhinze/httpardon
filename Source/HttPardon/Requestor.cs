using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
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
        readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public Response Get(string url)
        {
            var httpOptions = new HttpOptions {BaseUri = url};
            return GetResponse(httpOptions);
        }

        internal Response Get(HttpOptions options)
        {
            return GetResponse(options);
        }

        Response GetResponse(HttpOptions httpOptions, Action<HttpWebRequest> action = null)
        {
            var request = _requestBuilder.Build(httpOptions);

            if (action != null)
                action(request);

            var response = (HttpWebResponse) request.GetResponse();

            return _responseBuilder.Build(response);
        }

        internal Response Post(HttpOptions httpOptions)
        {
            return GetResponse(httpOptions, r => Action(r, httpOptions));
        }

        void Action(HttpWebRequest request, HttpOptions options)
        {
            request.Method = "post";

            using (TextWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                var json = _jsonSerializer.ToJson(options.AdditionalOptions.query);
                writer.Write(json);
            }
        }
    }
}