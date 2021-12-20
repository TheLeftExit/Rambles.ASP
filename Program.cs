string? path = args.FirstOrDefault();

WebApplicationBuilder builder;

if (path is not null) {
    Directory.SetCurrentDirectory(path);
    var options = new WebApplicationOptions {
        ContentRootPath = path,
        WebRootPath = path
    };
    builder = WebApplication.CreateBuilder(options);
} else {
    builder = WebApplication.CreateBuilder();
}

builder.Services.AddRazorPages();
builder.ConfigureRambles(path ?? builder.Environment.WebRootPath);

var app = builder.Build();

app.MapRambles();

app.MapRazorPages();

app.UseStaticFiles();

app.MapFallbackToPage("/NotFound");

app.Run();