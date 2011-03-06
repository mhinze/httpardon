using System;
using System.Net;

namespace Wut
{
    public static class Listen
    {
        static Listener _listener;
        public static string Url { get; private set; }

        public static IListeningScenario OnLocalhost()
        {
            return StartAndSetUrl("http://localhost:80/");
        }

        public static IListeningScenario On(string url)
        {
            return StartAndSetUrl(url);
        }

        public static void Assert(Action<HttpListenerRequest> assertion)
        {
            if (_listener == null)
                throw new InvalidOperationException("The Listener has not been initialized");

            if (assertion == null)
                throw new ArgumentNullException("assertion");

            assertion(_listener.Request);
        }

        public static void Stop()
        {
            _listener.Stop();
        }

        static IListeningScenario StartAndSetUrl(string url)
        {
            _listener = new Listener();

            var listeningScenario = new ListeningScenario(_listener)
            {
                Prefix = Url = url
            };
            return listeningScenario;
        }
    }
}