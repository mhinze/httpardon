using System.IO;
using System.Net;
using System.Text;

namespace Wut
{
    public static class HttpListenerExtensions
    {
        public static void WriteOutput(this HttpListenerResponse response, string output)
        {
            var buffer = Encoding.UTF8.GetBytes(output);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        public static string Body(this HttpListenerRequest request)
        {
            if (!request.HasEntityBody) return null;
            var encoding = request.ContentEncoding;
            using (var body = request.InputStream)
            using (var reader = new StreamReader(body, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}