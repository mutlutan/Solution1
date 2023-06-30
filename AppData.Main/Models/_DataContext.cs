using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AppData.Main.Migration;
using System.Text.Json;
using AppCommon;

#nullable disable

namespace AppData.Main.Models
{
    //PM> run //diğer komutlar  -Verbose
    //Scaffold-DbContext "Server=.;Database=smart_bike_main;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -outputDir Models -context DataContext -Force -NoPluralize -NoOnConfiguring

    public partial class DataContext : DbContext
    {
        #region new prop
        public int UserId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string IPAddress { get; set; } = "";
        public CultureInfo Culture { get; set; } = new("tr-TR");
        public string Language => this.Culture.TwoLetterISOLanguageName;
        #endregion

        //override
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=db1;Trusted_Connection=True;", x => x.UseNetTopologySuite());
            }
        }

        #region genel metotlar
        public string[] GetVeriDiliTablolari()
        {
            List<string> result = new();
            var tablolar = this.RawQuery("select TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME='VeriDili'");

            foreach (var item in tablolar)
            {
                result.Add(item.TABLE_NAME);
            }

            return result.ToArray();
        }

        public int GetSemaVersion()
        {
            string sqlCommand = $"Select max(Id) as Id From Version";

            var qVersion = this.RawQuery(sqlCommand).FirstOrDefault();

            return qVersion.Id ?? 0;
        }

        public string GetConnectionString()
        {
            return this.Database.GetConnectionString();
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

        public void RefreshConnectionString()
        {
            // base controllerda dil değişi olduğunda, constr nin dbye hangi dilde bağlanacağını söyler
            this.SetConnectionString(this.Database.GetConnectionString());
        }

        public string ColumnDataType(string tableName, string columnName)
        {
            string sqlCommand = $"select DATA_TYPE from INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME='{tableName}' and COLUMN_NAME='{columnName}'";

            return this.RawQuery(sqlCommand).FirstOrDefault().DATA_TYPE;
        }
        #endregion

        #region ApiRequestToSqlText - ApiRequestToResponseData

        public MoSql ApiRequestToSqlText(Type table, ApiRequest apiRequest)
        {
            //var query = this.defaultBusiness.dataContext.ApiRequestToSqlText(typeof(PersonUser), request);
            MoSql sql = new()
            {
                SelectText = "Select * ",
                FromText = $"From {table.Name} WITH (NOLOCK)",
                WhereText = "",
                OrderByText = "",
                PagingText = ""
            };

            string sqlWhereLine = "";
            string sqlOrderLine = "";

            //filters
            if (apiRequest.Filter != null)
            {
                foreach (var filterLine in apiRequest.Filter.Filters)
                {
                    if (sqlWhereLine.Length > 0)
                    {
                        sqlWhereLine += " and ";
                    }

                    var valueType = this.ColumnDataType(table.Name, filterLine.Field);

                    if (valueType == "int" | valueType == "decimal" | valueType == "float" | valueType == "bit")
                    {
                        sqlWhereLine += $" {filterLine.Field} {filterLine.Operator} {filterLine.Value}";
                    }

                    if (valueType == "char" | valueType == "varchar" | valueType == "nvarchar" | valueType == "date" | valueType == "time" | valueType == "datetime")
                    {
                        sqlWhereLine += $" {filterLine.Field} {filterLine.Operator} '{filterLine.Value}'";
                    }
                }

                if (sqlWhereLine.Length > 0)
                {
                    sql.WhereText += "Where " + sqlWhereLine;
                }
            }

            //sort
            if (apiRequest.Sort != null)
            {
                foreach (var sortLine in apiRequest.Sort)
                {
                    if (sqlOrderLine.Length > 0)
                    {
                        sqlOrderLine += ", ";
                    }

                    if (sortLine.Field.MyToTrim().Length > 0)
                    {
                        sqlOrderLine += sortLine.Field.MyToTrim() + " " + sortLine.Dir.MyToTrim();
                    }
                }

                if (sqlOrderLine.MyToTrim().Length > 0)
                {
                    sql.OrderByText = "Order By " + sqlOrderLine;
                }
            }

            #region paging
            //OFFSET     10 ROWS-- skip 10 rows
            //FETCH NEXT 10 ROWS ONLY; --take 10 rows
            sql.PagingText = $"OFFSET {(apiRequest.Page - 1) * apiRequest.PageSize} ROWS FETCH NEXT {apiRequest.PageSize} ROWS ONLY";

            #endregion

            return sql;

        }

        public MoResponse<object> ApiRequestToMoResponse<T>(ApiRequest request)
        {
            //var res = this.defaultBusiness.dataContext.ApiRequestToMoResponse<PersonUser>(request);
            MoResponse<object> response = new();

            var query = this.ApiRequestToSqlText(typeof(T), request);

            var data = this.RawQuery<T>(query.ToCommandText());
            response.Data = data;
            response.Total = data.Count();

            return response;
        }
        #endregion

        #region RawQuery - Dapper
        public T RawQueryExecuteScalar<T>(string sqlCommand)
        {
            return this.Database.GetDbConnection().ExecuteScalar<T>(sqlCommand);
        }

        public IEnumerable<dynamic> RawQuery(string sqlCommand)
        {
            return this.Database.GetDbConnection().Query<dynamic>(sqlCommand);
        }

        public IEnumerable<T> RawQuery<T>(string sqlCommand)
        {
            return this.Database.GetDbConnection().Query<T>(sqlCommand);
        }

        public IEnumerable<T> RawQuery<T>(string sqlCommand, object param)
        {
            return this.Database.GetDbConnection().Query<T>(sqlCommand, param);
        }
        #endregion

        #region sequence
        public int GetNextSequenceValue(string _SequenceName)
        {
            int rV = 0;
            var connection = this.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT NEXT VALUE FOR " + _SequenceName;
            //command.CommandText = "SELECT " + _SequenceName + ".NEXTVAL FROM DUAL"; //Oracle

            try
            {
                connection.Open();
                rV = Convert.ToInt32(command.ExecuteScalar());
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return rV;
        }

        #endregion

        #region Table methods
        public List<string> GetTableCollumns(string tableName)
        {
            List<string> rV = new();

            try
            {
                rV = this.RawQuery<string>($"Select COLUMN_NAME as ColumnName From INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME='{tableName}'").ToList();
            }
            catch { }

            return rV;
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

            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".SqlScript." + fileName);
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

            LogList.Append(this.RunScript(";", GetResourceSqlScript("111_Tables.sql")).ToString() + "Tablolar oluşturuldu. \n");
            LogList.Append(this.RunScriptWithGo(GetResourceSqlScript("121_Insert.sql")).ToString() + "Varsayılan kayıtlar eklendi(121). \n");
            LogList.Append(this.RunScriptWithGo(GetResourceSqlScript("122_Insert.sql")).ToString() + "Varsayılan kayıtlar eklendi(122). \n");
            LogList.Append(this.RunScriptWithGo(GetResourceSqlScript("123_Insert.sql")).ToString() + "Varsayılan kayıtlar eklendi(123). \n");

            return LogList;
        }

        private StringBuilder CreateViews()
        {
            var LogList = new System.Text.StringBuilder();

            LogList.Append(this.RunScriptWithGo(GetResourceSqlScript("201_Views.sql")).ToString() + "Viewlar oluşturuldu & güncellendi. \n");

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

                    var qVersion = this.Version.Where(c => c.Id == item.Id).FirstOrDefault();
                    if (qVersion != null)
                    {
                        qVersion.CommandText = item.CommandText;
                        this.SaveChanges();
                    }

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

        #region Audit Logs
        private void BeforeSaveChanges(string[] _AuditTableList)
        {
            var changeTrack = this.ChangeTracker.Entries()
                .Where(c => c.State == EntityState.Added | c.State == EntityState.Modified | c.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changeTrack)
            {
                string tableName = entry.Entity.GetType().Name;
                Boolean logKaydet = _AuditTableList.Where(c => c.Contains(tableName)).Any();

                if (logKaydet)
                {
                    var newTemAuditLog = new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        OperationDate = DateTime.Now,
                        UserId = this.UserId,
                        UserIp = this.IPAddress,
                        TableName = tableName,
                        PrimaryKeyField = entry.Properties.FirstOrDefault(c => c.Metadata.IsPrimaryKey())?.Metadata.Name
                    };

                    if (entry.State == EntityState.Added)
                    {
                        newTemAuditLog.OperationType = "C";
                        newTemAuditLog.PrimaryKeyValue = Convert.ToString(entry.CurrentValues[newTemAuditLog.PrimaryKeyField ?? ""]);
                        newTemAuditLog.CurrentValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        newTemAuditLog.OperationType = "U";
                        newTemAuditLog.PrimaryKeyValue = Convert.ToString(entry.OriginalValues[newTemAuditLog.PrimaryKeyField ?? ""]);
                        newTemAuditLog.CurrentValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        newTemAuditLog.OriginalValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    }
                    if (entry.State == EntityState.Deleted)
                    {
                        newTemAuditLog.OperationType = "D";
                        newTemAuditLog.PrimaryKeyValue = Convert.ToString(entry.OriginalValues[newTemAuditLog.PrimaryKeyField ?? ""]);
                        newTemAuditLog.OriginalValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    }

                    this.AuditLog.Add(newTemAuditLog);
                }
            }
        }

        public override int SaveChanges()
        {
            var parametre = this.Parameter.FirstOrDefault();
            if (parametre != null)
            {
                if (parametre.AuditLog == true && parametre.AuditLogTables != null && parametre.AuditLogTables.Length > 0)
                {
                    this.BeforeSaveChanges(parametre.AuditLogTables.Split(','));
                }
            }

            return base.SaveChanges(true);
        }
        #endregion

        #region Translate


        //public string TranslateToForPrms(string key, string lang, string[] prms)
        //{
        //    string rValue = key;

        //    if (this.AppDictionary.ContainsKey(key))
        //    {
        //        Dictionary<string, string> row = this.AppDictionary[key];
        //        rValue = row["tr"]; //default tr value
        //        if (row.ContainsKey(lang))
        //        {
        //            rValue = row[lang];
        //        }
        //    }

        //    try
        //    {
        //        if (prms != null && prms.Length > 0)
        //        {
        //            rValue = string.Format(rValue, prms);
        //        }
        //    }
        //    catch { }

        //    return rValue;
        //}

        //public string TranslateTo(string key, string lang)
        //{
        //    return this.TranslateToForPrms(key, lang, null);
        //}

        //public string TranslateTo(string key)
        //{
        //    return this.TranslateTo(key, this.Language);
        //}

        #endregion

        #region translate
        public List<MoWord> AppDictionary { get; set; } = new();

        public string TranslateToForPrms(string key, string lang, string[] prms)
        {
            string rValue = key;
            var dictionary = this.AppDictionary
                .Where(c => c.Key == key).FirstOrDefault();

            if (dictionary != null)
            {
                rValue = dictionary.Value.Tr; //default tr value
                if (lang == "en")
                {
                    rValue = dictionary.Value.En;
                }
            }

            try
            {
                if (prms != null && prms.Length > 0)
                {
                    rValue = string.Format(rValue, prms);
                }
            }
            catch { }

            return rValue;
        }

        public string TranslateTo(string key, string lang)
        {
            return this.TranslateToForPrms(key, lang, null);
        }

        public string TranslateTo(string key)
        {
            return this.TranslateTo(key, this.Language);
        }
        #endregion

    }
}