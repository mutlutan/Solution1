using AppCommon;
using AppCommon.Business;
using AppCommon.DataLayer.DataMain.Models;
using CronNET;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using Telerik.DataSource.Extensions;
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

            Business business = new(mainConStr, logConStr);
            var jobList = business.GetJobList();

            foreach (var jobItem in jobList)
            {
                try
                {
                    cron_daemon.AddJob(jobItem.CronExpression, () =>
                    {
                        //dinamik olarak metodu çağırıyoruz
                        MethodInfo methodInfo = typeof(Business).GetMethod(jobItem.MethodName);
                        ParameterInfo[] parameterInfo = methodInfo.GetParameters();//burdan parametre ekleyebilirsin gerekirse
                        methodInfo.Invoke(new Business(mainConStr, logConStr), parameterInfo);
                    });
                }
                catch { }
            }

            cron_daemon.Start();
        }
    }
}
