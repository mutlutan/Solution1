using AppJob;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JobConfig>(builder.Configuration.GetSection(nameof(JobConfig)));
var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Job App Activated!");

#region Application...
app.Lifetime.ApplicationStarted.Register(() =>
{
	var jobConfig = app.Services.GetService(typeof(IOptions<JobConfig>)) as IOptions<JobConfig>;
    var job = new MyJob(jobConfig.Value);
});
#endregion

app.Run();
