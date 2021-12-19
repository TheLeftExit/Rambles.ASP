using Microsoft.AspNetCore.Mvc.Routing;
using Rambles.Data;

public class RambleRouter : DynamicRouteValueTransformer {
    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values) {
        var newValues = new RouteValueDictionary(values.Values);

        var rambleService = httpContext.RequestServices.GetService<RambleService>();
        if (rambleService is not null) {
            await rambleService.Refresh();
            string path = httpContext.Request.Path;
            if (rambleService.Contains(path))
                newValues["page"] = "/Ramble";
        }

        return newValues;
    }
}

public static class RambleRouterExtensions {
    public static void ConfigureRambles(this WebApplicationBuilder builder) {
        string webRootPath = builder.Environment.WebRootPath;
        var rambleServicePrototype = new RambleService(webRootPath);
        builder.Services.AddSingleton(rambleServicePrototype);

        builder.Services.AddSingleton<RambleRouter>();
    }

    public static void MapRambles(this IEndpointRouteBuilder builder) {
        builder.MapDynamicPageRoute<RambleRouter>("{path?}");
    }
}