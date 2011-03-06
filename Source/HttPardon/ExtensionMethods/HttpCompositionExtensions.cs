using System;
using System.Collections.Generic;
using HttPardon.FluentInterface;

namespace HttPardon
{
    public static class HttpCompositionExtensions
    {
        public static void http(this object extended, Action<IHttp> configuration)
        {
            if (configuration == null) return;

            var http = new HttpOptions();

            configuration(http);

            HttpOptions.Cache[extended] = http;
        }

        public static Response get(this object extended)
        {
            var httpOptions = HttpOptions.Cache[extended];
            return Http.get(httpOptions);
        }

        public static Response post(this object extended, string path, dynamic options)
        {
            var httpOptions = HttpOptions.Cache[extended];
            return Http.post(httpOptions);
        }
    }
}