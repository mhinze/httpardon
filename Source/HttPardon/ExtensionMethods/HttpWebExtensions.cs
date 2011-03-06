using System;
using System.IO;
using System.Net;

namespace HttPardon
{
    public static class HttpWebExtensions
    {
        public static string Body(this HttpWebResponse response)
        {
            return response.GetResponseStream().Body();
        }

        public static string Body(this Stream stream)
        {
            using (stream)
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
            throw new InvalidOperationException("The response stream was null.");
        }
    }
}