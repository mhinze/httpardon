using HttPardon.Hashie;
using Machine.Specifications;

namespace HttPardon.Specifications.Hashie
{
    public abstract class parsing_a_hash
    {
        protected static RubyHasher hasher;
        protected static dynamic parsed;
        Because of = () => parsed = new RubyHasher().Parse(hash);
        protected static string hash { get; set; }
    }

    [Subject(typeof (RubyHasher))]
    public class when_obtaining_a_ruby_hash : parsing_a_hash
    {
        Establish context = () => hash = @"{ :query => {:status => 'It\'s an HTTParty and everyone is invited!'} }";

        It should_be_available_via_indexer = () =>
            ((string) parsed["query"]["status"]).ShouldEqual("It\'s an HTTParty and everyone is invited!");

        It should_parse_the_nested_hash = () =>
            ((string) parsed.query.status).ShouldEqual("It\'s an HTTParty and everyone is invited!");
    }

    [Subject(typeof (RubyHasher), "shortcut")]
    public class when_obtaining_a_ruby_hash_without_the_surrounding_braces : parsing_a_hash
    {
        Establish context = () => hash = @":query => {:status => 'It\'s an HTTParty and everyone is invited!'}";

        It should_be_available_via_indexer = () =>
            ((string) parsed["query"]["status"]).ShouldEqual("It\'s an HTTParty and everyone is invited!");

        It should_parse_the_nested_hash = () =>
            ((string) parsed.query.status).ShouldEqual("It\'s an HTTParty and everyone is invited!");
    }

    [Subject(typeof (RubyHasher))]
    public class when_obtaining_a_ruby_hash_with_integer_value : parsing_a_hash
    {
        Establish context = () => hash = @"{'i' => 42}";

        It should_parse_the_nested_hash = () =>
            ((int) parsed.i).ShouldEqual(42);
    }

    [Subject(typeof (RubyHasher))]
    public class when_obtaining_a_ruby_hash_with_array_value : parsing_a_hash
    {
        Establish context = () => hash = @"{'i' => [42,43,44,45]}";

        It should_parse_the_nested_hash = () =>
            ((object[]) parsed.i).ShouldContainOnly(42, 43, 44, 45);
    }


    [Subject(typeof (RubyHasher))]
    public class when_obtaining_a_deep_ruby_hash : parsing_a_hash
    {
        Establish context =
            () => hash = @"{:a => 1, :b => {:c => 2, :j => 3, :d => 4, :f => {:g => 6, :h => 7, :i => 8} } }";

        It should_be_available_via_indexer = () =>
            ((int) parsed["b"]["f"]["g"]).ShouldEqual(6);

        It should_parse_the_nested_hash = () =>
            ((int) parsed.b.f.g).ShouldEqual(6);
    }

    [Subject(typeof (DynamicHash))]
    public class when_accessing_a_non_existant_member : parsing_a_hash
    {
        Establish context = () => hash = string.Empty;

        It member_access_should_return_return_null = () =>
        {
            object foo = parsed.non_existant;
            foo.ShouldBeNull();
        };

        It indexer_access_should_return_return_null = () =>
        {
            object foo = parsed["non_existant"];
            foo.ShouldBeNull();
        };
    }
}