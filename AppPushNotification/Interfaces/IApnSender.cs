using AppPushNotification.Apple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPushNotification.Interfaces
{
    public interface IApnSender
    {
        Task<ApnsResponse> SendAsync(
            object notification,
            string deviceToken,
            string apnsId = null,
            int apnsExpiration = 0,
            int apnsPriority = 10,
            ApnPushType apnPushType = ApnPushType.Alert,
            CancellationToken cancellationToken = default);
    }
}
