namespace HttPardon.Hashie
{
    public interface IRubyHasher
    {
        dynamic Parse(string hash);
    }
}