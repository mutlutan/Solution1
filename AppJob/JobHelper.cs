using AppCommon;
using AppCommon.Business;
using CronNET;
using Microsoft.Extensions.Caching.Memory;

namespace AppJob
{
    public class JobHelper
    {
        public static bool OnActive { get; set; } = true; // Jobların aktif olup olmadığı belirler

        private static readonly CronDaemon cron_daemon = new();

        public JobHelper(AppConfig appConfig, JobConfig jobConfig)
        {
            cron_daemon.AddJob(jobConfig.JobItems[0].CronExpression, () => { new Business(appConfig).LocalWebRequest(); });
            cron_daemon.AddJob(jobConfig.JobItems[1].CronExpression, () => { new Business(appConfig).SetAuditLogToDbLogFromDbMain(); });
            cron_daemon.AddJob(jobConfig.JobItems[5].CronExpression, () => { new Business(appConfig).MailJobMailHareklerdenBekliyorOlanlariGoder(); });
            cron_daemon.AddJob(jobConfig.JobItems[6].CronExpression, () => { new Business(appConfig).MailJobMailHarekleriTekrarDene(); });
            cron_daemon.Start();
        }
    }
}
