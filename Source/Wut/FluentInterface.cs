using System;
using System.ComponentModel;

namespace Wut
{
    public interface IGrammar
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();
    }

    public interface IListeningScenario : IGrammar
    {
        void Respond(Action<IRespondingScenario> response);
    }

    public interface IRespondingScenario : IGrammar
    {
        void Default();
        void Body(string body);
        void ContentType(string contentType);
        void Json(string json);
    }
}