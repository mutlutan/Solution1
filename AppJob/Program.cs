using AppCommon.Business;
using AppJob;

var builder = WebApplication.CreateBuilder(args);
//linux için proxy adresi
builder.WebHost.UseUrls("http://localhost:5005/");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JobConfig>(builder.Configuration.GetSection(nameof(JobConfig)));
builder.Services.AddScoped<MyJob>();
builder.Services.AddScoped<JobHelper>();

var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Job App Activated!");

#region Application...
app.Lifetime.ApplicationStarted.Register(() =>
{
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
