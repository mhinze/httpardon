using System;
using System.Net;

namespace Wut
{
    internal class Listener
    {
        HttpListener daemon;

        public void StartAsync(ListeningScenario listening)
        {
            daemon = new HttpListener();
            daemon.Prefixes.Add(listening.Prefix);
            daemon.Start();

            var listenerState = new ListenerState(daemon, listening);

            daemon.BeginGetContext(Callback, listenerState);
        }

        void Callback(IAsyncResult ar)
        {
            var state = ((ListenerState) ar.AsyncState);
            var context = state.Daemon.EndGetContext(ar);
            
            var response = context.Response;

            response.ContentType = state.Scenario.Response.ContentType;
            response.WriteOutput(state.Scenario.Response.Body);

            response.OutputStream.Close();

            Request = context.Request;
        }

        public HttpListenerRequest Request { get; set; }


        public void Stop()
        {
            daemon.Abort();
            daemon.Close();
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