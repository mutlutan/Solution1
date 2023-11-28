using AppCommon.Business;
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
    var config = app.Services.CreateScope().ServiceProvider.GetRequiredService<IOptions<JobConfig>>().Value;
    var job = new MyJob(config);

    Business business = new(config.MainConnection, config.LogConnection);

    #region Garbage collection collect
    Task.Factory.StartNew(() =>
    {
        while (true)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Thread.Sleep(1000 * 60 * 60); //1 Saat sonra 
            }
            catch { }
        }
    });
    #endregion
});
#endregion

app.Run();
