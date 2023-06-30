using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppPushNotification.Firesbase
{
    public record FirebaseSettings(
        [property: JsonPropertyName("project_id")] string ProjectId,
        [property: JsonPropertyName("private_key")] string PrivateKey,
        [property: JsonPropertyName("client_email")] string ClientEmail,
        [property: JsonPropertyName("token_uri")] string TokenUri
    );
}
