var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseStaticFiles();

#region index.html render
var html = File.ReadAllText("wwwroot\\index.html");
var files = new DirectoryInfo("wwwroot\\components").EnumerateFiles("*.js", SearchOption.AllDirectories);
foreach (var file in files.Where(c=> c.Name != "base-comp.js"))
{
	string old = "<!--components-imports-->";
	string fileName = file.FullName.Replace(app.Environment.WebRootPath, "").Replace("\\", "/");
	string link = old + Environment.NewLine + $"<script>document.write('<script src=\"{fileName}?v=' + getVersion() + '\"><\\/script>');</script>";

	html = html.Replace(old, link);
}
app.MapGet("/", () => Results.Content(html, "text/html"));
#endregion


app.Run();
