using System;
using System.Collections.Generic;
using System.Dynamic;
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
                h.BaseUri("http://localhost:80/");
                h.BasicAuth("username", "password");
            });
        }
    }

    [Subject(typeof (IHttp), "post")]
    public class when_using_the_composition_hook_to_issue_a_post
    {
        Establish listener = () => Listen.OnLocalhost().Respond(x => x.Default());
//
//        Because of = () => new Twitter()
//            .post("/statuses/update.json", new hash{foo = bar});
    }

    
}