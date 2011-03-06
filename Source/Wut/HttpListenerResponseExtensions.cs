using System.Net;
using System.Text;

namespace Wut
{
    public static class HttpListenerResponseExtensions
    {
        public static void WriteOutput(this HttpListenerResponse response, string output)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(output);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}