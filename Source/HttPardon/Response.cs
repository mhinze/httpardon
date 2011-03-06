using System.Net;

namespace HttPardon
{
    public class Response
    {
        public string Raw { get; internal set; }

        public HttpWebResponse HttpWebResponse { get; internal set; }

        public dynamic Body { get; internal set; }
    }
}