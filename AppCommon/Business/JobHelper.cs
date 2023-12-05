using AppCommon.DataLayer.DataMain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
		private readonly MainDataContext dataContext;
		public MailHelper? mailHelper;

		public JobHelper(IServiceProvider serviceProvider)
		{
			var appConfig = serviceProvider.GetService<IOptions<AppConfig>>()?.Value ?? new();
			this.dataContext = new();
			this.dataContext.SetConnectionString(appConfig.MainConnection);

			this.mailHelper = serviceProvider.GetService<MailHelper>();
		}

		public List<Job> GetJobList()
		{
			List<Job> rV = new();
			try
			{
				var data = dataContext.Job.AsNoTracking()
					.Where(c => c.IsActive == true)
					.ToList();

				if (data != null)
				{
					rV = data;
				}
			}
			catch //(Exception ex)
			{
				//WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return rV;
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
            catch //(Exception ex)
            {
                //WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

		/// <summary>
		/// Main Dbden AuditLog'ları alıp Log Db deki AuditLog tablosuna yazar. Main Dbdeki aktarılan logları siler.
		/// </summary>      
		public void SetAuditLogToDbLogFromDbMain()
		{
			try
			{
				var mainAuditLogList = this.dataContext.AuditLog.OrderBy(x => x.OperationDate).Take(200).ToList();

				if (mainAuditLogList.Count > 0)
				{
					foreach (var item in mainAuditLogList)
					{
						string sqlText = $@"
                           BEGIN TRY
                                begin tran                                          
                        			  Insert Into smart_bike_log.dbo.AuditLog Select * From smart_bike_main.dbo.AuditLog Where Id='{item.Id}';
                        			  Delete From smart_bike_main.dbo.AuditLog Where Id='{item.Id}';
                                commit tran
                            END TRY
                            BEGIN CATCH
                                IF @@TRANCOUNT > 0
                                    ROLLBACK TRAN  
                        			
                        		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
                                DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
                                DECLARE @ErrorState INT = ERROR_STATE();
                        
                        		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
                        
                            END CATCH 

                    ";
						this.dataContext.Database.ExecuteSqlRaw(sqlText);
					}
				}
			}
			catch /*(Exception ex)*/
			{
				//WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}
		}

		//30 snde bir çalışabilir
		public bool MailJobMailHareklerdenBekliyorOlanlariGoder()
		{
			bool rV = false;
			try
			{
				//bekliyor olan kayıtların ilk 10 adetini çeker
				//bu kayıtları SendMailForMailHareket e gönderir

				var mailHareketList = dataContext.EmailPool
					.Where(c => c.TryQuantity <= 3 && c.EmailPoolStatusId == (int)EnmEmailPoolStatus.Waiting)
					.Take(50);

				foreach (var mailHareket in mailHareketList)
				{
					this.mailHelper?.SendMailForMailHareket(mailHareket.Id);
				}

			}
			catch /*(Exception ex)*/
			{
				//WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return rV;
		}

		//2dk da bir çalışabilir
		public bool MailJobMailHarekleriTekrarDene()
		{
			bool rV = false;
			try
			{
				//hata olan kayıtların ilk 50 adetini çeker
				//bu kayıtları SendMailForMailHareket e gönderir

				var mailHareketList = dataContext.EmailPool
					.Where(c => c.TryQuantity <= 3 && c.EmailPoolStatusId == (int)EnmEmailPoolStatus.Error)
					.Take(50);

				foreach (var mailHareket in mailHareketList)
				{
					this.mailHelper?.SendMailForMailHareket(mailHareket.Id);
				}

			}
			catch /*(Exception ex)*/
			{
				//WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
			}

			return rV;

		}



		public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
