using AppCommon;
using AppCommon.Business;
using AppJob;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(nameof(AppConfig)));
builder.Services.Configure<JobConfig>(builder.Configuration.GetSection(nameof(JobConfig)));
builder.Services.AddScoped<Business>(opt =>
{
	var config = opt.GetService(typeof(IOptions<AppConfig>)) as IOptions<AppConfig>;

	return ActivatorUtilities.CreateInstance<Business>(opt, config?.Value ?? new());
});

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Job Sunucu Aktif!");


#region Application...
app.Lifetime.ApplicationStarted.Register(() =>
{
	var business = app.Services.CreateScope().ServiceProvider.GetRequiredService<Business>();
	var jobConfig = app.Services.GetService(typeof(IOptions<JobConfig>)) as IOptions<JobConfig>;
    var appConfig = app.Services.GetService(typeof(IOptions<AppConfig>)) as IOptions<AppConfig>;

    var job = new JobHelper(appConfig.Value, jobConfig.Value);
});
#endregion

app.Run();
