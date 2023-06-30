using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Main.Models;
using AppData.Main.Repository.Dmo;

#nullable disable

namespace AppData.Main.Repository
{
    public class Repository : IDisposable
    {
        public readonly DataContext dataContext;
       
        public DmoRole RepRole { get; private set; }
        public DmoParameter RepParameter { get; private set; }
        public DmoUser RepUser { get; private set; }
        
        public DmoEmailLetterhead RepEmailLetterhead { get; private set; }
        public DmoEmailTemplate RepEmailTemplate { get; private set; }
        public DmoBolge RepBolge { get; private set; }
        public DmoBolgeDetay RepBolgeDetay { get; private set; }
        public DmoSarjIstasyonu RepSarjIstasyonu { get; private set; }
        public DmoVwAuditLog RepVwAuditLog { get; private set; }
        public DmoVwSystemLog RepVwSystemLog { get; private set; }
        public DmoVwUserLog RepVwUserLog { get; private set; }
        public DmoEmailPool RepEmailPool { get; private set; }
        public DmoUye RepUye { get; private set; }
        public DmoUyeGrup RepUyeGrup { get; private set; }
        public DmoUyeKaraListe RepUyeKaraListe { get; private set; }
        public DmoMobilBildirim RepMobilBildirim { get; private set; }
        public DmoTarife RepTarife { get; private set; }
        public DmoFiyat RepFiyat { get; private set; }
        public DmoSarjIstasyonuHareket RepSarjIstasyonuHareket { get; private set; }
        public DmoMobilBildirimUye RepMobilBildirimUye { get; private set; }
        public DmoSmsBildirim RepSmsBildirim { get; private set; }
        public DmoSmsBildirimUye RepSmsBildirimUye { get; private set; }
        public DmoUyeCariHareket RepUyeCariHareket { get; private set; }
        public DmoUyeCuzdanHareket RepUyeCuzdanHareket { get; private set; }
        public DmoArac RepArac { get; private set; }
        public DmoAracHareket RepAracHareket { get; private set; }
        public DmoAracHareketDetay RepAracHareketDetay { get; private set; }
        public DmoVwAracStatuLog RepVwAracStatuLog { get; private set; }
        public DmoVwMobilBildirimLog RepVwMobilBildirimLog { get; private set; }
        public DmoVwSmsLog RepVwSmsLog { get; private set; }
        public DmoKampanya RepKampanya { get; private set; }
        /*code_definition_end*/

        public Repository(DataContext context)
        {
            this.dataContext = context;

            this.RepRole = new DmoRole(this.dataContext);
            this.RepParameter = new DmoParameter(this.dataContext);
            this.RepUser = new DmoUser(this.dataContext);
            
            this.RepEmailLetterhead = new DmoEmailLetterhead(this.dataContext);
            this.RepEmailTemplate = new DmoEmailTemplate(this.dataContext);
            this.RepBolge = new DmoBolge(this.dataContext);
            this.RepBolgeDetay = new DmoBolgeDetay(this.dataContext);
            this.RepSarjIstasyonu = new DmoSarjIstasyonu(this.dataContext);
            this.RepVwAuditLog = new DmoVwAuditLog(this.dataContext);
            this.RepVwSystemLog = new DmoVwSystemLog(this.dataContext);
            this.RepVwUserLog = new DmoVwUserLog(this.dataContext);
            this.RepEmailPool = new DmoEmailPool(this.dataContext);
            this.RepUye = new DmoUye(this.dataContext);
            this.RepUyeGrup = new DmoUyeGrup(this.dataContext);
            this.RepUyeKaraListe = new DmoUyeKaraListe(this.dataContext);
            this.RepMobilBildirim = new DmoMobilBildirim(this.dataContext);
            this.RepTarife = new DmoTarife(this.dataContext);
            this.RepFiyat = new DmoFiyat(this.dataContext);
            this.RepSarjIstasyonuHareket = new DmoSarjIstasyonuHareket(this.dataContext);
            this.RepMobilBildirimUye = new DmoMobilBildirimUye(this.dataContext);
            this.RepSmsBildirim = new DmoSmsBildirim(this.dataContext);
            this.RepSmsBildirimUye = new DmoSmsBildirimUye(this.dataContext);
            this.RepUyeCariHareket = new DmoUyeCariHareket(this.dataContext);
            this.RepUyeCuzdanHareket = new DmoUyeCuzdanHareket(this.dataContext);
            this.RepArac = new DmoArac(this.dataContext);
            this.RepAracHareket = new DmoAracHareket(this.dataContext);
            this.RepAracHareketDetay = new DmoAracHareketDetay(this.dataContext);
            this.RepVwAracStatuLog = new DmoVwAracStatuLog(this.dataContext);
            this.RepVwMobilBildirimLog = new DmoVwMobilBildirimLog(this.dataContext);
            this.RepVwSmsLog = new DmoVwSmsLog(this.dataContext);
            this.RepKampanya = new DmoKampanya(this.dataContext);
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
