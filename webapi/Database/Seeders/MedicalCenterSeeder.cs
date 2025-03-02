using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class MedicalCenterSeeder
{
    private readonly ApplicationDbContext _context;

    public MedicalCenterSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.MedicalCenters.Any())
        {
            var medicalCenters = new List<MedicalCenter>
            {
                new MedicalCenter {
                    Name = "Unidade Médica do Andes CM.",
                    PhoneNumber = "(11) 98765-4321",
                    Email = "unidade1@unidades.com",
                    AddressId = 1
                },
                new MedicalCenter {
                    Name = "Unidade Saúde Integral.",
                    PhoneNumber = "(21) 97654-3210",
                    Email = "unidade2@unidades.com",
                    AddressId = 2
                },
                new MedicalCenter {
                    Name = "Centro Médico Vida Plena.",
                    PhoneNumber = "(31) 96543-2109",
                    Email = "unidade3@unidades.com",
                    AddressId = 3
                },
                new MedicalCenter {
                    Name = "Clínica Bem Estar.",
                    PhoneNumber = "(41) 95432-1098",
                    Email = "unidade4@unidades.com",
                    AddressId = 4
                },
                new MedicalCenter {
                    Name = "Hospital São Lucas.",
                    PhoneNumber = "(51) 94321-0987",
                    Email = "unidade5@unidades.com",
                    AddressId = 5
                },
                new MedicalCenter {
                    Name = "Unidade Médica Santa Clara.",
                    PhoneNumber = "(61) 93210-9876",
                    Email = "unidade6@unidades.com",
                    AddressId = 6
                },
                new MedicalCenter {
                    Name = "Centro de Saúde Popular.",
                    PhoneNumber = "(71) 92109-8765",
                    Email = "unidade7@unidades.com",
                    AddressId = 7
                },
                new MedicalCenter {
                    Name = "Unidade Médica Primavera.",
                    PhoneNumber = "(81) 91098-7654",
                    Email = "unidade8@unidades.com",
                    AddressId = 8
                },
                new MedicalCenter {
                    Name = "Clínica Médica Esperança.",
                    PhoneNumber = "(91) 90987-6543",
                    Email = "unidade9@unidades.com",
                    AddressId = 9
                },
                new MedicalCenter {
                    Name = "Hospital Vida Nova.",
                    PhoneNumber = "(31) 89876-5432",
                    Email = "unidade10@unidades.com",
                    AddressId = 10
                }
            };


            _context.MedicalCenters.AddRange(medicalCenters);
            _context.SaveChanges();
        }
    }
}
