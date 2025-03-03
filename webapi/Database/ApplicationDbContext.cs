using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApi.Models;

namespace WebApi.Database;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Administrator> Administrators { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Doctor> Doctors { get; set; }

    public DbSet<Especialization> Especializations { get; set; }

    public DbSet<Appointment> Appointments { get; set; }

    public DbSet<AppointmentRating> AppointmentRatings { get; set; }

    public DbSet<MedicalCenter> MedicalCenters { get; set; }

    public DbSet<HealthPlan> HealthPlans { get; set; }

    public DbSet<MedicalAgreement> MedicalAgreements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider?.GetService<IConfiguration>();

            if (configuration == null)
            {
                throw new Exception("Arquivo de configuração não encontrado.");
            }

            optionsBuilder.UseSqlite(_configuration.GetConnectionString("SQLiteConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Administrator>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Doctor>().HasIndex(u => u.Email).IsUnique();
    }
}