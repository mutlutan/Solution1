using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPushNotification.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        TObject Deserialize<TObject>(string json);
    }
}
