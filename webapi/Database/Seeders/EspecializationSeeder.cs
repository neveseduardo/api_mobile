using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class EspecializationSeeder
{
    private readonly ApplicationDbContext _context;

    public EspecializationSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Especializations.Any())
        {
            var especializations = new List<Especialization>
            {
                new Especialization { Name = "Clínica Médica" },
                new Especialization { Name = "Alergologia" },
                new Especialization { Name = "Anestesiologia" },
                new Especialization { Name = "Cardiologia" },
                new Especialization { Name = "Cirurgia Geral" },
                new Especialization { Name = "Cirurgia Plástica" },
                new Especialization { Name = "Dermatologia" },
                new Especialization { Name = "Endocrinologia" },
                new Especialization { Name = "Gastroenterologia" },
                new Especialization { Name = "Geriatria" },
                new Especialization { Name = "Ginecologia e Obstetrícia" },
                new Especialization { Name = "Hematologia" },
                new Especialization { Name = "Infectologia" },
                new Especialization { Name = "Mastologia" },
                new Especialization { Name = "Nefrologia" },
                new Especialization { Name = "Neurologia" },
                new Especialization { Name = "Nutrologia" },
                new Especialization { Name = "Oftalmologia" },
                new Especialization { Name = "Oncologia" },
                new Especialization { Name = "Ortopedia e Traumatologia" },
                new Especialization { Name = "Otorrinolaringologia" },
                new Especialization { Name = "Pediatria" },
                new Especialization { Name = "Pneumologia" },
                new Especialization { Name = "Psiquiatria" },
                new Especialization { Name = "Radiologia" },
                new Especialization { Name = "Reumatologia" },
                new Especialization { Name = "Urologia" }
            };

            _context.Especializations.AddRange(especializations);
            _context.SaveChanges();
        }
    }
}
