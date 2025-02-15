using WebApi.Database;
using WebApi.Repositories;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IApiAuthenticationRepository, ApiAuthenticationRepository>();
builder.Services.AddTransient<IWebAuthenticationRepository, WebAuthenticationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddCustomSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapCustomControllerRoutes();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.MapControllers();
app.MapRazorPages();
app.SeedDatabase();
app.UseStaticFiles();

app.Run();
