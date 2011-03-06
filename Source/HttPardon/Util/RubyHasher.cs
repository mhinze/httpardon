using System.Dynamic;
using IronRuby;
using IronRuby.Builtins;
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

            var rubyHash = (Hash) source.Execute();

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

    public class DynamicHash : DynamicObject
    {
        readonly Hash _hash;

        public DynamicHash(Hash hash)
        {
            _hash = hash;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return getFromHash(binder.Name, out result);
        }

        bool getFromHash(string name, out object result)
        {
            if (_hash.TryGetValue(MutableString.Create(name), out result))
            {
                var value_hash = result as Hash;
                if (value_hash != null)
                {
                    result = new DynamicHash(value_hash);
                }
                var value_array = result as RubyArray;
                if (value_array != null)
                {
                    result = value_array.ToArray();
                }
                return true;
            }
            return false;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            return getFromHash(index.ToString(), out result);
        }
    }
}