using IronRuby;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace HttPardon.Hashie
{
    public class RubyHasher : IRubyHasher
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
            if (hash == null)
                return new DynamicHash();

            hash = reformatHash_addingOutsideBracesIfNecessary(hash);

            dynamic rubyHash = compileHash(hash);

            return new DynamicHash(rubyHash);
        }

        static object compileHash(string hash)
        {
            var source = engine.CreateScriptSourceFromString(hash, SourceCodeKind.Expression);
            return source.Execute();
        }

        static string reformatHash_addingOutsideBracesIfNecessary(string hash)
        {
            var trimmed = hash.Trim();
            if (trimmed.StartsWith("{") && trimmed.EndsWith("}")) return hash;
            return string.Format("{{{0}}}", hash);
        }
    }
}