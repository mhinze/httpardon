using System.Net;
using Machine.Specifications;
using Wut;

namespace HttPardon.Specifications
{
    [Subject(typeof (Http))]
    public class when_requesting_a_document_over_http
    {
        static Response response;

        Establish context = () => Listen.OnLocalhost().Respond(x => x.Body("foo"));

        Cleanup listener = Listen.Stop;

        Because of = () => { response = Http.get(Listen.Url); };

        It should_include_the_raw_body_in_the_response = () =>
            (response.Raw).ShouldEqual("foo");

        It should_include_the_httpwebresponse_object_in_the_response = () =>
            ((HttpWebResponse) response).StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}