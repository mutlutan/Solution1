using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AppCommon.DataLayer.DataLog.Migration;

#nullable disable

namespace AppCommon.DataLayer.DataLog.Models
{
	//PM> run //diğer komutlar  -Verbose
	//Scaffold-DbContext "Server=.;Database=solution1_log;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -outputDir DataLayer\DataLog\Models -context LogDataContext -Force -NoPluralize -NoOnConfiguring

	public partial class LogDataContext : DbContext
	{
		//override
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=.;Database=db1;Trusted_Connection=True;");
			}
		}

		#region genel metotlar
		public int GetSemaVersion()
		{
			string sqlCommand = $"Select max(Id) as Id From Version";

			var qVersion = this.RawQuery(sqlCommand).FirstOrDefault();

			return qVersion.Id ?? 0;
		}

		public void SetConnectionString(string conStr)
		{
			SqlConnectionStringBuilder csb = new(conStr)
			{
				ApplicationName = Assembly.GetExecutingAssembly().FullName,
				CurrentLanguage = CultureInfo.GetCultureInfo("tr-TR").Parent.EnglishName,
				MultipleActiveResultSets = true
			};

			this.Database.SetConnectionString(csb.ToString());
		}
		#endregion

		#region RawQuery - Dapper
		public IEnumerable<dynamic> RawQuery(string sqlCommand)
		{
			return this.Database.GetDbConnection().Query<dynamic>(sqlCommand);
		}
		#endregion

		#region MigrateDatabase - Database Create -update - RunScript 

		private StringBuilder CreateDatabase()
		{
			var LogList = new StringBuilder();
			DateTime startTime = DateTime.Now;

			string _sInitialCatalog = this.Database.GetDbConnection().Database;
			string _sCollate = "TURKISH_CI_AS"; //burası seçilebilir birşey olmalı
			string _SQLText = "CREATE DATABASE " + _sInitialCatalog + " COLLATE " + _sCollate + ";";

			using (var Con1 = new SqlConnection("Server=.; Database=master; Trusted_Connection=True;"))
			{
				Con1.Open();
				using (var cmd = new SqlCommand(_SQLText, Con1))
				{
					cmd.ExecuteNonQuery();
					LogList.AppendLine("Veritabanı dosyası " + _sCollate + " olarak oluşturuldu. " + (DateTime.Now - startTime).TotalSeconds.ToString("N2") + " sn");
				}
				Con1.Close();
			}

			return LogList;
		}

		private StringBuilder RunScriptWithGo(string script)
		{
			var LogList = new StringBuilder();
			var dbname = this.Database.GetDbConnection().Database;
			using (var Con1 = new SqlConnection("Server=.; Database=" + dbname + "; Trusted_Connection=True;"))
			{
				// split script on GO command
				IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

				Con1.Open();
				foreach (string sqlText in commandStrings)
				{
					if (!string.IsNullOrEmpty(sqlText))
					{
						if (sqlText.Trim().Length > 5)
						{
							using var cmd = new SqlCommand(sqlText, Con1);
							try
							{
								cmd.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								throw new Exception("Script:(" + sqlText + ")\n" + ex.Message);
							}
						}
					}
				}
				Con1.Close();
			}

			return LogList;
		}

		private StringBuilder RunScript(string _Separators, string _Script)
		{
			var LogList = new StringBuilder();
			var dbname = this.Database.GetDbConnection().Database;
			using (var Con1 = new SqlConnection("Server=.; Database=" + dbname + "; Trusted_Connection=True;"))
			{
				Con1.Open();
				foreach (string s in _Script.Split(new string[] { _Separators }, StringSplitOptions.RemoveEmptyEntries))
				{
					string sqlText = s;

					if (!string.IsNullOrEmpty(sqlText))
					{
						if (sqlText.Trim().Length > 5)
						{
							using var cmd = new SqlCommand(sqlText, Con1);
							try
							{
								cmd.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								throw new Exception("Script:(" + sqlText + ")\n" + ex.Message);
							}
						}
					}
				}
				Con1.Close();
			}

			return LogList;
		}

		private static string GetResourceSqlScript(string fileName)
		{
			string rV = "";

			var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".DataLayer.DataLog.SqlScript." + fileName);
			if (resourceStream != null)
			{
				using var reader = new StreamReader(resourceStream, Encoding.UTF8);
				rV = reader.ReadToEnd();
			}

			return rV;
		}

		private StringBuilder CreateTables()
		{
			var LogList = new System.Text.StringBuilder();
			string sqlScriptText = GetResourceSqlScript("111_Tables.sql");
			string sonucText = this.RunScript(";", sqlScriptText).ToString();
			LogList.Append(sonucText + "Tablolar oluşturuldu. \n");

			return LogList;
		}

		private StringBuilder CreateViews()
		{
			var LogList = new System.Text.StringBuilder();
			string sqlScriptText = GetResourceSqlScript("201_Views.sql");
			string sonucText = this.RunScriptWithGo(sqlScriptText).ToString();
			LogList.Append(sonucText + "Viewlar oluşturuldu & güncellendi. \n");

			return LogList;
		}

		private StringBuilder UpdateQuery()
		{
			var LogList = new System.Text.StringBuilder();

			string description = "";
			string commandText = "";

			try
			{
				int versionId = this.GetSemaVersion();

				// db update
				var dbConnection = this.Database.GetDbConnection();
				foreach (var item in MyMigration.GetMigrationList(versionId))
				{
					// sql ler execute edilecek
					description = item.Description;
					commandText = item.CommandText;

					LogList.AppendLine($"Id : {item.Id}, Description : {description}");

					string sqlText = $@"
                            BEGIN TRY
                                begin tran
                                      {commandText}     
                                      Insert Version(Id, CreateDate, Description) Values({item.Id}, getdate(), '{description}');
                                commit tran
                            END TRY
                            BEGIN CATCH
                                IF @@TRANCOUNT > 0
                                    ROLLBACK TRAN

                                    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
                                    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
                                    DECLARE @ErrorState INT = ERROR_STATE()

                                -- Use RAISERROR inside the CATCH block to return error  
                                -- information about the original error that caused  
                                -- execution to jump to the CATCH block.  
                                RAISERROR (@ErrorMessage, -- Message text.  
                                           @ErrorSeverity, -- Severity.  
                                           @ErrorState -- State.  
                                          );
                            END CATCH 
                        ";

					dbConnection.Execute(sqlText);

					//var qVersion = this.Version.Where(c => c.Id == item.Id).FirstOrDefault();
					//if (qVersion != null)
					//{
					//    qVersion.CommandText = item.CommandText;
					//    this.SaveChanges();
					//}

				}

				LogList.AppendLine("DB Version : " + this.GetSemaVersion());
			}
			catch (Exception ex)
			{
				LogList.AppendLine(description);
				LogList.AppendLine(commandText);
				LogList.AppendLine(ex.Message);
			}

			return LogList;
		}

		public string NewDatabase()
		{
			StringBuilder LogList = new();
			try
			{

				LogList.Append(CreateDatabase());
				LogList.Append(CreateTables());
				LogList.Append(CreateViews());
			}
			catch (Exception ex)
			{
				LogList.AppendLine(ex.Message);
			}

			return LogList.ToString();
		}

		public string MigrateDatabase()
		{
			StringBuilder LogList = new();
			try
			{
				LogList.Append(UpdateQuery());
				LogList.Append(CreateViews());
			}
			catch (Exception ex)
			{
				LogList.AppendLine(ex.Message);
			}
			return LogList.ToString();
		}

		#endregion

		#region log save metotları

		/// <summary>
		/// Kullanıcının son işlem zamanı çıkış bilgisi olarak güncelliyoruz(son çıkış zamanı gerçeğe en yakın çıkış zamanıdır)
		/// </summary>
		/// <param name="userToken"></param>
		public void UserLogSetLogoutDate(string sessionGuid)
		{
			//(bu asenkron olsun bekleme yapmasın)
			var userLog = this.UserLog
				.Where(c => c.UserSessionGuid == sessionGuid)
				.OrderBy(o => o.LoginDate)
				.LastOrDefault();

			if (userLog != null)
			{
				userLog.LogoutDate = DateTime.Now;
				this.SaveChanges();
			}
		}

		/// <summary>
		/// Kullanıcı giriş bilgisinin log kaydını tutuyoruz
		/// </summary>
		/// <param name="response"></param>
		/// <param name="input"></param>
		public void UserLogAdd(UserLog userLog)
		{
			this.UserLog.Add(userLog);
			this.SaveChanges();
		}

		/// <summary>
		/// Sistem log kayıtlarını tutuyoruz
		/// </summary>
		/// <param name="systemLog"></param>
		public void SystemLogAdd(SystemLog systemLog)
		{
			this.SystemLog.Add(systemLog);
			this.SaveChanges();
		}

		#endregion
	}
}