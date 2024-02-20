using AppCommon;
using AppCommon.Business;
using Microsoft.Extensions.Options;
using System.Reflection;
using CronNET;

namespace WebApp.Job
{
	public class MyJob
    {
        public static bool OnActive { get; set; } = true; // Jobların aktif olup olmadığı belirler
        private static readonly CronDaemon cron_daemon = new();

        public MyJob(IServiceProvider serviceProvider)
        {
			var appConfig = serviceProvider.GetService<IOptions<AppConfig>>().Value ?? new();
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
