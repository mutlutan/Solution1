using AppCommon;
using AppCommon.Business;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApp.Panel.Codes;


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
builder.Services.AddScoped<Business>(opt =>
{
    var config = opt.GetService(typeof(IOptions<AppConfig>)) as IOptions<AppConfig>;
    var mainConnectionString = config.Value.MainConnection;
    var logConnectionString = config.Value.LogConnection;

    return ActivatorUtilities.CreateInstance<Business>(opt, mainConnectionString, logConnectionString);
});

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

app.UseDeveloperExceptionPage(); //canlýda kapatalým
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHttpsRedirection();
    //app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAnyOrigin");

app.UseMiddleware<RequestResponseLogMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
