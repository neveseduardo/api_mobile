using WebApi.Database;
using WebApi.Repositories;
using WebApi.Extensions;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IAuthenticationRepository<Administrator>, AdminAuthRepository>();
builder.Services.AddTransient<IAuthenticationRepository<User>, UserAuthRepository>();
builder.Services.AddTransient<IRepository<User>, UserRepository>();
builder.Services.AddTransient<IRepository<Address>, AddressRepository>();
builder.Services.AddTransient<IRepository<Administrator>, AdministratorRepository>();
builder.Services.AddTransient<IRepository<Doctor>, DoctorRepository>();
builder.Services.AddTransient<IRepository<Especialization>, EspecializationRepository>();
builder.Services.AddTransient<IRepository<MedicalCenter>, MedicalCenterRepository>();
builder.Services.AddTransient<IRepository<Appointment>, AppointmentRepository>();
builder.Services.AddTransient<IRepository<AppointmentRating>, AppointmentRatingRepository>();
builder.Services.AddCustomSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapCustomControllerRoutes();
app.UseHttpsRedirection();
app.MapControllers();
app.MapRazorPages();
app.SeedDatabase();
app.UseStaticFiles();

app.Run();
