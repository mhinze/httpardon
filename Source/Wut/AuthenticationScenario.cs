using System.Net;

namespace Wut
{
    internal class AuthenticationScenario : IAuthenticationScenario
    {
        public AuthenticationSchemes Scheme { get; set; }

        public void Basic()
        {
            Scheme = AuthenticationSchemes.Basic;
        }
    }
}