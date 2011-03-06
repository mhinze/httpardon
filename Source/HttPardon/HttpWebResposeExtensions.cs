using System;
using System.IO;
using System.Net;

namespace HttPardon
{
    public static class HttpWebResposeExtensions
    {
        public static string Body(this HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
            throw new InvalidOperationException("The response stream was null.");
        }
    }
}