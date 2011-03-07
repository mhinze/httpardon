using Machine.Specifications;
using Wut;

namespace HttPardon.Specifications.IHttp_Post
{
    public class Twitter
    {
        public Twitter()
        {
            this.http(h =>
            {
                h.BaseUri("http://localhost");
                h.BasicAuth("username", "password");
            });
        }
    }

    [Subject(typeof (IHttp), "post")]
    public class when_using_the_composition_hook_to_issue_a_post
    {
        Establish context = () => Listen.OnLocalhost().Respond(x => x.Default());

        Cleanup after = Listen.Stop;

        Because of = () => new Twitter()
            .post("/statuses/update.json",
                  @"{:query => {:status => 'It\'s an HTTParty and everyone is invited!'}}");

        It should_post_query = () =>
            Listen.Assert(
                x => x.RequestBody.ShouldEqual(@"{""status"":""It\u0027s an HTTParty and everyone is invited!""}"));

        It should_post_to_complete_url =
            () => Listen.Assert(x => x.Url.ShouldEqual("http://localhost/statuses/update.json"));

        It should_post_to_complete_url = () => Listen.Assert(x => x.Url.ShouldEqual("http://localhost/statuses/update.json"));
    }
}