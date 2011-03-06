using System.Net;
using System.Web.Helpers;

namespace HttPardon.Details
{
    public class ResponseBuilder
    {
        public Response Build(HttpWebResponse httpWebResponse)
        {
            var raw = httpWebResponse.Body();
            var body = httpWebResponse.ContentType.Contains("json") ? Json.Decode(raw) : raw;
            return new Response
            {
                Raw = raw,
                HttpWebResponse = httpWebResponse,
                Body = body
            };
        }
    }
}