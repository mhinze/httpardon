using System.IO;
using System.Net;
using System.Text;

namespace HttPardon.Details
{
    internal class HttpWebRequestBuilder
    {
        readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        static HttpWebRequestBuilder()
        {
            // http://blogs.msdn.com/b/shitals/archive/2008/12/27/9254245.aspx
            ServicePointManager.Expect100Continue = false;
        }

        // TODO hrm
        public HttpWebRequest Build(HttpOptions options)
        {
            var request = (HttpWebRequest) WebRequest.Create(options.BaseUri);

            request.Method = options.Method;

            PrepareRequestStream(options, request);

            PrepareAuthentication(options, request);

            options.Configure(request);

            return request;
        }

        static void PrepareAuthentication(HttpOptions options, HttpWebRequest request)
        {
            var basic_auth = options.AdditionalOptions.basic_auth;
            if (basic_auth != null)
            {
                options.BasicAuth(basic_auth.username, basic_auth.password);
            }
        }

        void PrepareRequestStream(HttpOptions options, HttpWebRequest result)
        {
            if (options.Method == "post")
            {
                var query = options.AdditionalOptions.query;

                if (query != null)
                {
                    string json = _jsonSerializer.ToJson(query);
                    result.ContentLength = Encoding.UTF8.GetBytes(json).Length;
                    using (TextWriter writer = new StreamWriter(result.GetRequestStream()))
                    {
                        writer.Write(json);
                    }
                }

                else
                {
                    result.ContentLength = 0;
                }
            }
        }
    }
}