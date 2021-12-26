using Rambles.Data;

string root = Directory.GetCurrentDirectory();
string header = args.FirstOrDefault() ?? "Rambles";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = root });
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new RambleService(root, header));
var app = builder.Build();
// app.MapFallbackToPage("/sitemap.xml", "/_Sitemap"); // Disabled until I find a way to retrieve the URL requested behind reverse proxy.
app.MapFallbackToPage("{path}.md", "/_Host");
app.UseStaticFiles(new StaticFileOptions() { ServeUnknownFileTypes = true });
app.MapFallbackToPage("/_Host");
app.Run();