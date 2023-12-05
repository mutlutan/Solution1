using AppCommon;
using AppCommon.Business;
using AppCommon.DataLayer.DataMain.Models;
using CronNET;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Reflection;
using Telerik.DataSource.Extensions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AppJob
{
    public class MyJob
    {
        public static bool OnActive { get; set; } = true; // Jobların aktif olup olmadığı belirler
        private static readonly CronDaemon cron_daemon = new();

        public MyJob(IServiceProvider serviceProvider)
        {
			var jobConfig = serviceProvider.GetService<IOptions<JobConfig>>().Value ?? new();
            var jobHelper = serviceProvider.GetService<JobHelper>();
            var jobList = jobHelper.GetJobList();

            foreach (var jobItem in jobList)
            {
                try
                {
                    cron_daemon.AddJob(jobItem.CronExpression, () =>
                    {
                        MethodInfo methodInfo = typeof(JobHelper).GetMethod(jobItem.MethodName);
                        ParameterInfo[] parameterInfo = methodInfo.GetParameters();//burdan parametre ekleyebilirsin gerekirse
                        methodInfo.Invoke(jobHelper, parameterInfo);
                    });
                }
                catch { }
            }

            cron_daemon.Start();
        }
    }
}
