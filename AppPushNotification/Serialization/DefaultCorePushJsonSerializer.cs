using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppPushNotification.Serialization
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultCorePushJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            var options = GetJsonSerializerOptions();
            var json = JsonSerializer.Serialize(obj, options);

            return json;
        }

        public TObject Deserialize<TObject>(string json)
        {
            var options = GetJsonSerializerOptions();
            var obj = JsonSerializer.Deserialize<TObject>(json, options);

            return obj;
        }

        protected virtual JsonSerializerOptions GetJsonSerializerOptions()
        {
            var settings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return settings;
        }
    }
}
