using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Business
{
    public class JobHelper: IDisposable
    {

        public JobHelper() {
        
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
