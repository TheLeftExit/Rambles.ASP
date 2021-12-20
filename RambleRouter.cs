using Microsoft.AspNetCore.Mvc.Routing;
using Rambles.Data;
using System.Runtime.InteropServices;

public class RambleRouter : DynamicRouteValueTransformer {
    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values) {
        var newValues = new RouteValueDictionary(values.Values);

        var rambleService = httpContext.RequestServices.GetService<RambleService>();
        if (rambleService is not null) {
            await rambleService.Refresh();
            string path = httpContext.Request.Path;

            if (rambleService.Contains(path))
                newValues["page"] = "/RamblePage";

            
        }

        return newValues;
    }
}

public static class RambleRouterExtensions {
    public static void ConfigureRambles(this WebApplicationBuilder builder, string rootPath) {
        var rambleServicePrototype = new RambleService(rootPath);
        builder.Services.AddSingleton(rambleServicePrototype);

        builder.Services.AddSingleton<RambleRouter>();
    }

    public static void MapRambles(this WebApplication builder) {
        builder.MapDynamicPageRoute<RambleRouter>("{path?}");
    }
}