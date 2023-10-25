namespace AppJob
{
    public class JobItem
    {
        public string MethodName { get; set; } = "";
        public string CronExpression { get; set; } = "";
    }
    public class JobConfig
    {
        public string MainConnection { get; set; } = "";
        public string LogConnection { get; set; } = "";
        public List<JobItem> JobItems { get; set; } = new List<JobItem>();
    }
}
