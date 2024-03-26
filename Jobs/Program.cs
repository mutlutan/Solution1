using AppCommon;
using AppCommon.Business;
using Microsoft.Extensions.Options;
using Jobs;

var builder = WebApplication.CreateBuilder(args);

//linux için proxy adresi
builder.WebHost.UseUrls("http://localhost:5005/");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(nameof(AppConfig)));
builder.Services.AddScoped<Business>();
builder.Services.AddScoped<MyJob>();
builder.Services.AddScoped<JobHelper>();

var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Job App Activated!");

#region Application...
app.Lifetime.ApplicationStarted.Register(() =>
{
	var business = app.Services.CreateScope().ServiceProvider.GetRequiredService<Business>();
	var config = app.Services.CreateScope().ServiceProvider.GetRequiredService<IOptions<AppConfig>>().Value;

	#region local web request : application stop olmasýný engellemek için 
	Task.Factory.StartNew(() =>
	{
		while (true)
		{
			try
			{
				using var client = new HttpClient() { BaseAddress = new Uri(config.SelfHost) };
				var response = client.GetAsync("").Result;

				Thread.Sleep(1000 * 60 * 15); //15dk sonra 
			}
			catch { }
		}
	});
	#endregion

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
