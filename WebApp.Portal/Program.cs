using AppCommon;

var builder = WebApplication.CreateBuilder(args);
//linux için proxy adresi
//builder.WebHost.UseUrls("http://localhost:5001/");

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();  //Aktif

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddNewtonsoftJson();
// newtonsoft ise
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    //options.SerializerSettings.Converters.Add(new NetTopologySuite.IO.Converters.GeometryConverter());
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(); // Use the default property (Pascal) casing
}
).AddJsonOptions((options) =>
{
    //options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeometryConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // system json ise camle case ayarý
}
);

builder.Services.AddBrowserDetection(); //Install-Package Shyjus.BrowserDetector

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

