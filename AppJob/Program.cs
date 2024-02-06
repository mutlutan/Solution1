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

app.Run();
