using WebApi.Database.Seeders;
using WebApi.Database;

namespace WebApi.Extensions;

static class DatabaseSeederExtension
{
    public static void SeedDatabase(this IHost app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();

            var userSeeder = new UserSeeder(context);
            var admionistratorSeeder = new AdministratorSeeder(context);
            var especializationSeeder = new EspecializationSeeder(context);

            userSeeder.Seed();
            admionistratorSeeder.Seed();
            especializationSeeder.Seed();
        }
    }
}