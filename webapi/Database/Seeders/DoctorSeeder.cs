using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class DoctorSeeder
{
    private readonly ApplicationDbContext _context;

    public DoctorSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Doctors.Any())
        {
            var Doctors = new List<Doctor>
            {
                new Doctor {
                    Name = "Antonio Carlos",
                    CPF = "582.423.471-07",
                    Email = "antoniocarlos@example.com",
                    CRM = "123844ZB",
                    EspecializationId = 1,
                },
                new Doctor {
                    Name = "Felipe Camargo",
                    CPF = "020.194.711-08",
                    Email = "felipecamargo@example.com",
                    CRM = "144844ZB",
                    EspecializationId = 1
                },
            };

            _context.Doctors.AddRange(Doctors);
            _context.SaveChanges();
        }
    }
}
