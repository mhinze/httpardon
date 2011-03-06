using System;

namespace Wut
{
    internal class ListeningScenario : IListeningScenario
    {
        readonly Listener _listener;

        public ListeningScenario(Listener listener)
        {
            _listener = listener;
        }

        public string Prefix { get; set; }

        public RespondingScenario Response { get; set; }

        void IListeningScenario.Respond(Action<IRespondingScenario> response)
        {
            Response = new RespondingScenario();
            if (response != null) response(Response);
            _listener.StartAsync(this);
        }
    }
}