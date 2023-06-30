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
#pragma warning disable CS8603 // Possible null reference return.
			return JsonSerializer.Deserialize<TObject>(json, options);
#pragma warning restore CS8603 // Possible null reference return.
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
