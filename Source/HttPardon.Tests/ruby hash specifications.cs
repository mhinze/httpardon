using HttPardon.Util;
using Machine.Specifications;

namespace HttPardon.Specifications.Hash
{
    public abstract class parsing_a_hash
    {
        protected static RubyHasher hasher;
        protected static dynamic parsed;
        Because hashed = () => parsed = new RubyHasher().Parse(hash);
        protected static string hash { get; set; }
    }

    [Subject(typeof (RubyHasher))]
    public class standard_hash_parsing : parsing_a_hash
    {
        It should_be_available_via_indexer = () =>
            ((string) parsed["query"]["status"]).ShouldEqual("It\'s an HTTParty and everyone is invited!");

        It should_parse_the_nested_hash = () =>
            ((string) parsed.query.status).ShouldEqual("It\'s an HTTParty and everyone is invited!");

        Establish that = () => hash = @"{ :query => {:status => 'It\'s an HTTParty and everyone is invited!'} }";
    }

    [Subject(typeof (RubyHasher))]
    public class hash_parsing_without_surrounding_braces : parsing_a_hash
    {
        It should_be_available_via_indexer = () =>
            ((string) parsed["query"]["status"]).ShouldEqual("It\'s an HTTParty and everyone is invited!");

        It should_parse_the_nested_hash = () =>
            ((string) parsed.query.status).ShouldEqual("It\'s an HTTParty and everyone is invited!");

        Establish that = () => hash = @":query => {:status => 'It\'s an HTTParty and everyone is invited!'}";
    }

    [Subject(typeof (RubyHasher))]
    public class standard_hash_parsing_with_integers : parsing_a_hash
    {
        It should_parse_the_nested_hash = () =>
            ((int) parsed.i).ShouldEqual(42);

        Establish that = () => hash = @"{'i' => 42}";
    }

    [Subject(typeof (RubyHasher))]
    public class standard_hash_parsing_with_array : parsing_a_hash
    {
        It should_parse_the_nested_hash = () =>
            ((object[]) parsed.i).ShouldContainOnly(42, 43, 44, 45);

        Establish that = () => hash = @"{'i' => [42,43,44,45]}";
    }
}