using System.Dynamic;
using System.Net;
using System.Web.Helpers;

namespace HttPardon
{
    public class Http
    {
        public static Response get(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            var response = (HttpWebResponse) request.GetResponse();

            return new Response(response);
        }
    }

    public class Response : DynamicObject
    {
        public Response(HttpWebResponse response)
        {
            HttpWebResponse = response;
            Raw = response.Body();
            Body = response.ContentType.Contains("json") ? Json.Decode(Raw) : Raw;
        }

        public string Raw { get; private set; }

        public HttpWebResponse HttpWebResponse { get; private set; }

        public dynamic Body { get; private set; }
    }
}