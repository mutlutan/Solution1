var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseStaticFiles();

#region index.html render
var html = File.ReadAllText("wwwroot\\index.html");
#region components-imports
if (false)
{
	var files = new DirectoryInfo("wwwroot\\components").EnumerateFiles("*.js", SearchOption.AllDirectories);
	foreach (var file in files.Where(c => c.Name != "base-comp.js"))
	{
		string old = "<!--components-imports-->";
		string fileName = file.FullName.Replace(app.Environment.WebRootPath, "").Replace("\\", "/");
		string link = old + Environment.NewLine + $"<script src='{fileName}?replace-version'></script>";
		html = html.Replace(old, link);
	}
}
#endregion

#region replace-version
string version = "v.0.1";
if (app.Environment.IsDevelopment())
{
	version = "v." + DateTime.Now.Ticks.ToString();
}
html = html.Replace("?replace-version", "?" + version);
#endregion

app.MapGet("/", () => Results.Content(html, "text/html"));
#endregion


app.Run();
