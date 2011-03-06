using HttPardon.FluentInterface;

namespace HttPardon
{
    public interface IHttp : IGrammar
    {
        void BaseUri(string uri);
        void BasicAuth(string username, string password);
    }
}