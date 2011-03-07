using System.Net;

namespace HttPardon.Details
{
    internal class HttpWebRequestBuilder
    {
        static HttpWebRequestBuilder()
        {
            // http://blogs.msdn.com/b/shitals/archive/2008/12/27/9254245.aspx
            ServicePointManager.Expect100Continue = false;
        }

        public HttpWebRequest Build(HttpOptions options)
        {
            var result = (HttpWebRequest) WebRequest.Create(options.BaseUri);
            options.Configure(result);
            return result;
        }
    }
}