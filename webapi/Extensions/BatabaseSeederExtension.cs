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
            var administratorSeeder = new AdministratorSeeder(context);
            var especializationSeeder = new EspecializationSeeder(context);
            var doctorSeeder = new DoctorSeeder(context);
            var addressSeeder = new AddressSeeder(context);
            var medicalCenterSeeder = new MedicalCenterSeeder(context);
            var appointmentSeeder = new AppointmentSeeder(context);

            addressSeeder.Seed();
            userSeeder.Seed();
            administratorSeeder.Seed();
            especializationSeeder.Seed();
            doctorSeeder.Seed();
            medicalCenterSeeder.Seed();
            appointmentSeeder.Seed();
        }
    }
}