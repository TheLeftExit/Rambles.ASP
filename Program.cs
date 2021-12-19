var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.ConfigureRambles();

var app = builder.Build();

app.UseHttpsRedirection();

// Uncomment to map non-Rambles pages.
// app.MapRazorPages();

app.MapRambles();

app.UseStaticFiles();

app.MapFallbackToPage("/NotFound");

app.Run();