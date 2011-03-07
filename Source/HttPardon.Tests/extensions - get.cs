using Machine.Specifications;
using Wut;

namespace HttPardon.Specifications.IHttp_Post
{
    public class Partay
    {
        public const string Url = "http://localhost:8080/";

        public Partay()
        {
            this.http(h => h.BaseUri(Url));
        }
    }

    [Subject(typeof (IHttp), "get")]
    public class when_using_the_extensions_to_issue_a_GET_request
    {
        static Partay partay;

        Establish context = () =>
        {
            Listen.On(Partay.Url).Respond(x => x.Default());
            partay = new Partay();
        };

        Cleanup after = Listen.Stop;

        Because of = () => { var response = partay.get(); };

        It should_issue_the_request_at_the_configured_base_uri =
            () => Listen.Assert(x => x.Url.ShouldEqual(Partay.Url));
    }
}