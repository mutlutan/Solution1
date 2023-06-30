using AppPushNotification.Firesbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPushNotification.Interfaces
{
    public interface IFirebaseSender
    {
        Task<FirebaseResponse> SendAsync(object payload, CancellationToken cancellationToken = default);
    }
}
