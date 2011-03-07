using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using HttPardon.Hashie;
using HttPardon.Util;

namespace HttPardon
{
    public class JsonSerializer
    {
        public string ToJson(object data)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.RegisterConverters(new[] {new DynamicHashConverter()});
            var serialize = javaScriptSerializer.Serialize(data);
            return serialize;
        }

        public dynamic FromJson(string json)
        {
            return Json.Decode(json);
        }
    }

    public class DynamicHashConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new[] {typeof (DynamicHash)})); }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            return ((DynamicHash) obj).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}