using AppCommon;
using AppCommon.Business;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApp.Panel.Middlewares;

var builder = WebApplication.CreateBuilder(args);
//linux için proxy adresi
builder.WebHost.UseUrls("http://localhost:5001/");

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();  //Aktif

#region Swagger builder
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

builder.Services.AddCors(o => o.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    })
);

builder.Services.AddControllers().AddNewtonsoftJson();
// newtonsoft ise
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new NetTopologySuite.IO.Converters.GeometryConverter());
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(); // Use the default property (Pascal) casing
}
).AddJsonOptions((options) =>
{
    //options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeometryConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // system json ise camle case ayarý
}
);

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(nameof(AppConfig)));
builder.Services.AddScoped<Business>();
builder.Services.AddScoped<MailHelper>();

builder.Services.AddBrowserDetection(); //Install-Package Shyjus.BrowserDetector

var app = builder.Build();

WebApp.Panel.Codes.MyApp.Env = app.Environment;

#region Swagger app
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHttpsRedirection();
    //app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAnyOrigin");
app.UseMiddleware<HttpLogMiddleware>();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

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
