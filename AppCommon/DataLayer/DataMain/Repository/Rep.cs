using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCommon.DataLayer.DataMain.Models;
using AppCommon.DataLayer.DataMain.Repository.Dmo;

#nullable disable

namespace AppCommon.DataLayer.DataMain.Repository
{
    public class Repository : IDisposable
    {
        public readonly MainDataContext dataContext;
       
        public DmoRole RepRole { get; private set; }
        public DmoParameter RepParameter { get; private set; }
        public DmoUser RepUser { get; private set; }
        
        public DmoEmailLetterhead RepEmailLetterhead { get; private set; }
        public DmoEmailTemplate RepEmailTemplate { get; private set; }
        public DmoVwAuditLog RepVwAuditLog { get; private set; }
        public DmoVwSystemLog RepVwSystemLog { get; private set; }
        public DmoVwUserLog RepVwUserLog { get; private set; }
        public DmoEmailPool RepEmailPool { get; private set; }
        public DmoUye RepUye { get; private set; }
        public DmoUyeGrup RepUyeGrup { get; private set; }
        
        public DmoCountry RepCountry { get; private set; }

        public DmoCurrency RepCurrency { get; private set; }
        /*code_definition_end*/

        public Repository(MainDataContext context)
        {
            this.dataContext = context;

            this.RepRole = new DmoRole(this.dataContext);
            this.RepParameter = new DmoParameter(this.dataContext);
            this.RepUser = new DmoUser(this.dataContext);
            this.RepEmailLetterhead = new DmoEmailLetterhead(this.dataContext);
            this.RepEmailTemplate = new DmoEmailTemplate(this.dataContext);
            this.RepVwAuditLog = new DmoVwAuditLog(this.dataContext);
            this.RepVwSystemLog = new DmoVwSystemLog(this.dataContext);
            this.RepVwUserLog = new DmoVwUserLog(this.dataContext);
            this.RepEmailPool = new DmoEmailPool(this.dataContext);
            this.RepUye = new DmoUye(this.dataContext);
            this.RepUyeGrup = new DmoUyeGrup(this.dataContext);
            this.RepCountry = new DmoCountry(this.dataContext);

            this.RepCurrency = new DmoCurrency(this.dataContext);
            /*code_constructor_end*/
        }

        public int SaveChanges()
        {
            return this.dataContext.SaveChanges();
        }

        public void Dispose()
        {
            this.RepRole = null;
            this.RepParameter = null;
            this.RepUser = null;
            /* bunu generator ��renmelei null setlerini yazmal�*/

            GC.SuppressFinalize(this);
        }
    }
}
