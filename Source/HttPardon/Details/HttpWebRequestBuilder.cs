using System.Net;

namespace HttPardon.Details
{
    internal class HttpWebRequestBuilder
    {
        public HttpWebRequest Build(HttpOptions options)
        {
            var result = (HttpWebRequest) WebRequest.Create(options.BaseUri);
            return result;
        }
    }
}