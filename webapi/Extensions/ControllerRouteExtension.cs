using Microsoft.AspNetCore.Builder;

namespace WebApi.Extensions;
public static class RouteExtensions
{
    public static void MapCustomControllerRoutes(this IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "api",
                pattern: "api/v1/{controller=ApiHome}/{action=Index}/{id?}"
            );

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=WebHome}/{action=Index}/{id?}"
            );
        });
    }
}
