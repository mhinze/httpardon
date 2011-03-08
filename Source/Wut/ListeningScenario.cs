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

        public RespondingScenario Response { get; private set; }
        public AuthenticationScenario Authentication { get; set; }

        public IRespondExpression Authenticate(Action<IAuthenticationScenario> auth)
        {
            Authentication = new AuthenticationScenario();
            if (auth != null)
                auth(Authentication);
            _listener.Authentication(this);
            return this;
        }

        public IAuthenticateExpression Respond(Action<IRespondingScenario> response)
        {
            Response = new RespondingScenario();
            if (response != null) response(Response);
            _listener.StartAsync(this);
            return this;
        }
    }
}