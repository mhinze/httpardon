namespace Wut
{
    public static class Listen
    {
        static Listener _listener;
        public static string Url { get; private set; }

        public static IListeningScenario OnLocalhost()
        {
            _listener = new Listener();

            var listeningScenario = new ListeningScenario(_listener)
            {
                Prefix = Url = "http://localhost:80/"
            };
            return listeningScenario;
        }

        public static void Stop()
        {
            _listener.Stop();
        }
    }
}