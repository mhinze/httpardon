using IronRuby;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace HttPardon.Util
{
    public class RubyHasher
    {
        static readonly ScriptRuntime runtime;

        static readonly ScriptEngine engine;

        static RubyHasher()
        {
            runtime = Ruby.CreateRuntime();
            engine = Ruby.GetEngine(runtime);
        }

        public dynamic Parse(string hash)
        {
            hash = fixHash(hash);

            var source = engine.CreateScriptSourceFromString(hash, SourceCodeKind.Expression);

            var rubyHash = source.Execute();

            return new DynamicHash(rubyHash);
        }

        string fixHash(string hash)
        {
            var trimmed = hash.Trim();
            if (trimmed.StartsWith("{") && trimmed.EndsWith("}"))
            {
                return hash;
            }

            return string.Format("{{{0}}}", hash);
        }
    }
}