using HttPardon.Hashie;
using HttPardon.Util;

namespace HttPardon.Performance
{
    public class Program
    {
        public static void Main()
        {
            for (int i = 0; i < 100000; i++)
            {
                var hasher = new RubyHasher();
                var o = hasher.Parse(@"{:foo => ""bar"" }");
            }
        }
    }
}