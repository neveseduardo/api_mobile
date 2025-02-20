using WebApi.Database;
using WebApi.Repositories.Api;
using WebApi.Repositories.Web;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IApiAuthenticationRepository, ApiAuthenticationRepository>();
builder.Services.AddTransient<IWebAuthenticationRepository, WebAuthenticationRepository>();
builder.Services.AddScoped<IApiUserRepository, ApiUserRepository>();
builder.Services.AddScoped<IApiAddressRepository, ApiAddressRepository>();
builder.Services.AddScoped<IApiAdministratorRepository, ApiAdministratorRepository>();
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
