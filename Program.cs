using Rambles.Data;

string root = Directory.GetCurrentDirectory();
string header = args.FirstOrDefault() ?? "Rambles";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = root });
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new RambleService(root, header));
var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions() { ServeUnknownFileTypes = true });
app.MapFallbackToPage("{*path}", "/_Host");
app.Run();