using AppCommon;
using AppCommon.Business;
using CronNET;
using Microsoft.Extensions.Caching.Memory;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AppJob
{
    public class JobHelper
    {
        public static bool OnActive { get; set; } = true; // Jobların aktif olup olmadığı belirler

        private static readonly CronDaemon cron_daemon = new();

        public JobHelper(JobConfig jobConfig)
        {
            var mainConStr = jobConfig.MainConnection;
            var logConStr = jobConfig.LogConnection;

            cron_daemon.AddJob(jobConfig.JobItems[0].CronExpression, () => { new Business(mainConStr, logConStr).LocalWebRequest(); });
            cron_daemon.AddJob(jobConfig.JobItems[1].CronExpression, () => { new Business(mainConStr, logConStr).SetAuditLogToDbLogFromDbMain(); });
            cron_daemon.AddJob(jobConfig.JobItems[5].CronExpression, () => { new Business(mainConStr, logConStr).MailJobMailHareklerdenBekliyorOlanlariGoder(); });
            cron_daemon.AddJob(jobConfig.JobItems[6].CronExpression, () => { new Business(mainConStr, logConStr).MailJobMailHarekleriTekrarDene(); });
            cron_daemon.Start();
        }
    }
}
