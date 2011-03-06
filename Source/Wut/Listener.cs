using System;
using System.Net;

namespace Wut
{
    internal class Listener
    {
        readonly ListenerAssertion _assertion = new ListenerAssertion();

        HttpListener daemon;

        public void StartAsync(ListeningScenario listening)
        {
            daemon = new HttpListener();
            daemon.Prefixes.Add(listening.Prefix);
            daemon.Start();

            var listenerState = new ListenerState(daemon, listening);

            var asyncResult = daemon.BeginGetContext(Callback, listenerState);

            listening.Execution(asyncResult);
        }

        void Callback(IAsyncResult ar)
        {
            var state = ((ListenerState) ar.AsyncState);
            var context = state.Daemon.EndGetContext(ar);

            StoreRequestInfoForAssertion(context.Request, _assertion);

            var response = context.Response;

            response.ContentType = state.Scenario.Response.ContentType;
            response.WriteOutput(state.Scenario.Response.Body);

            response.OutputStream.Close();
        }

        static void StoreRequestInfoForAssertion(HttpListenerRequest request, ListenerAssertion assertData)
        {
            assertData.Url = request.Url.AbsoluteUri;
            assertData.RequestBody = request.Body();
        }

        public void Stop()
        {
            daemon.Abort();
            daemon.Close();
        }

        public void Assert(Action<ListenerAssertion> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");
            if (_assertion == null) throw new InvalidOperationException("Assertion Data is null");
            assertion(_assertion);
        }

        class ListenerState
        {
            public ListenerState(HttpListener daemon, ListeningScenario listeningScenario)
            {
                Daemon = daemon;
                Scenario = listeningScenario;
            }

            public HttpListener Daemon { get; set; }
            public ListeningScenario Scenario { get; set; }
        }
    }
}