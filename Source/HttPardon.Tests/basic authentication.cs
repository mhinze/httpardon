using Machine.Specifications;
using Wut;

namespace HttPardon.Specifications
{
    [Subject("HttpOptions authentication")]
    public class when_supplying_basic_authentication_credentials
    {
        Establish context = () => Listen.OnLocalhost().Authenticate(x => x.Basic()).Respond(x => x.Default());

        Cleanup after = Listen.Stop;

        Because of = () => Http.get(Listen.Url, ":basic_auth => {:username => 'uname', :password => 'pass'}");

        It should_authenticate =
            () => Listen.Assert(x =>
            {
                x.Username.ShouldEqual("uname");
                x.Password.ShouldEqual("pass");
            });
    }
}