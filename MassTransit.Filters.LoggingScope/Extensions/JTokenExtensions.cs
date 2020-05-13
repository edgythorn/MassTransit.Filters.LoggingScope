using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MassTransit.Filters.LoggingScope
{
    internal static class JTokenExtensions
    {
        public static object ToObject(this JToken token) => token?.Type switch
        {
            null => throw new ArgumentNullException(nameof(token)),

            JTokenType.Object => token.TryGetKeyValuePair(out var pair)
                ? pair as object
                : token.Children<JProperty>().ToDictionary(prop => prop.Name, prop => ToObject(prop.Value)),

            JTokenType.Array => token.Select(ToObject).ToList(),

            _ => ((JValue)token).Value,
        };

        private static bool TryGetKeyValuePair(this JToken token, out KeyValuePair<string, object> pair)
        {
            var obj = token as JObject;
            if (obj == null)
            {
                return false;
            }

            if(obj.Count == 2 
                && obj.TryGetValue("key", StringComparison.OrdinalIgnoreCase, out var key)
                && key.Type == JTokenType.String
                && obj.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out var val))
            {
                pair = new KeyValuePair<string, object>(key.Value<string>(), ((JValue)val).Value);
                return true;
            }

            return false;
        }
    }
}
