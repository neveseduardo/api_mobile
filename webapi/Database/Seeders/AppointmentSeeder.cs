using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class AppointmentSeeder
{
    private readonly ApplicationDbContext _context;

    public AppointmentSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Appointments.Any())
        {
            var appointments = new List<Appointment>
            {
                new Appointment {
                    Date = DateTime.Parse("2025-03-02T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-03T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-04T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-05T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-06T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-07T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-08T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-09T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-10T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                },
                new Appointment {
                    Date = DateTime.Parse("2025-03-11T19:00"),
                    Notes = "Adicionado no seeder",
                    UserId = 1,
                    DoctorId = 1,
                }
            };



            _context.Appointments.AddRange(appointments);
            _context.SaveChanges();
        }
    }
}
