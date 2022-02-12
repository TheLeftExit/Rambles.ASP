using Rambles.Data;

string configPath = args.FirstOrDefault() ?? "rambles.cfg";
string configText = await File.ReadAllTextAsync(configPath);
RambleSettings.ApplyDefault(configText);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() {
    WebRootPath = RambleSettings.Default.RootDirectory,
    //ContentRootPath = RambleSettings.Default.RootDirectory
});

builder.Services.AddRazorPages();
builder.Services.AddSingleton<RambleService>();
var app = builder.Build();
app.MapFallbackToPage("/sitemap.xml", "/_Sitemap");
app.MapFallbackToPage("{path}.md", "/_Host");
app.UseStaticFiles(new StaticFileOptions() { ServeUnknownFileTypes = true });
app.MapFallbackToPage("/_Host");
await app.RunAsync();