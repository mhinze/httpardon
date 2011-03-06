using HttPardon.Util;
using Machine.Specifications;

namespace HttPardon.Specifications
{
    public class when_crafting_json_post_data_from_hash
    {
        const string hash = @"{:query => {:status => 'It\'s an HTTParty and everyone is invited!'}}";

        static dynamic parsed;
        static string json;
        Because of = () => { json = new JsonSerializer().ToJson(parsed.query); };

        It should_be_correct_json =
            () => json.ShouldEqual(@"{""status"":""It\u0027s an HTTParty and everyone is invited!""}");

        It should_parse = () => ((string) parsed.query.status).ShouldEqual("It\'s an HTTParty and everyone is invited!");
        Establish that = () => { parsed = new RubyHasher().Parse(hash); };
    }
}