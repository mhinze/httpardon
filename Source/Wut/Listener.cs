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
            NewDaemon();

            daemon.Prefixes.Add(listening.Prefix);

            daemon.Start();

            var listenerState = new ListenerState(daemon, listening);

            daemon.BeginGetContext(Callback, listenerState);
        }

        void NewDaemon()
        {
            daemon = daemon ?? new HttpListener();
        }

        void Callback(IAsyncResult ar)
        {
            var state = ((ListenerState) ar.AsyncState);
            var context = state.Daemon.EndGetContext(ar);

            StoreRequestInfoForAssertion(context, _assertion);

            var response = context.Response;
            
            response.ContentType = state.Scenario.Response.ContentType;
            response.WriteOutput(state.Scenario.Response.Body);

            response.OutputStream.Close();
        }

        static void StoreRequestInfoForAssertion(HttpListenerContext context, ListenerAssertion assertData)
        {
            assertData.Url = context.Request.Url.AbsoluteUri;
            assertData.RequestBody = context.Request.Body();
            
            if (context.User != null)
            {
                var identity = context.User.Identity as HttpListenerBasicIdentity;
                if (identity != null)
                {
                    assertData.Username = identity.Name;
                    assertData.Password = identity.Password;
                }
            }


            
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

        public void Authentication(ListeningScenario listeningScenario)
        {
            NewDaemon();

            daemon.AuthenticationSchemes = listeningScenario.Authentication.Scheme;
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