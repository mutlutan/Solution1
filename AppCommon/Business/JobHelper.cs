using AppCommon.DataLayer.DataMain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Business
{
    public class JobHelper: IDisposable
    {
        public readonly MainDataContext dataContext;

        public JobHelper(string mainConnection) {
            this.dataContext = new();
            this.dataContext.SetConnectionString(mainConnection);
        }

        public void LocalWebRequest()
        {
            try
            {
                string siteAddress = this.dataContext.Parameter.FirstOrDefault()?.SiteAddress ?? "";
                //Bu işlem ile application stop olmasını engellemek için doğal bir yöntem olarak kullanılabilir.
                using var client = new HttpClient() { BaseAddress = new Uri(siteAddress) };
                var response = client.GetAsync("").Result;
            }
            catch (Exception ex)
            {
                //WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }


        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
