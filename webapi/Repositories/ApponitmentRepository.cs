using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.ViewModels;

namespace WebApi.Repositories;

public class AppointmentRepository : IRepository<Appointment>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(ApplicationDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments.ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Appointment?> AddAsync(Appointment appointment)
    {
        try
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        try
        {
            appointment.UpdatedAt = DateTime.UtcNow;
            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");
            throw;
        }
    }

    public async Task DeleteAsync(Appointment appointment)
    {
        try
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao deletar item");
            throw;
        }
    }

    public AppointmentViewModel? GetViewModel(Appointment? appointment)
    {
        if (appointment != null)
        {
            UserViewModel? user = null;
            DoctorViewModel? doctor = null;

            if (appointment?.User != null)
            {
                user = new UserViewModel
                {
                    Id = appointment.User.Id,
                    Name = appointment.User.Name,
                    Email = appointment.User.Email,
                };
            }

            if (appointment?.Doctor != null)
            {
                doctor = new DoctorViewModel
                {
                    Id = appointment.Doctor.Id,
                    Name = appointment.Doctor.Name,
                    Email = appointment.Doctor.Email,
                };
            }

            var viewModel = new AppointmentViewModel
            {
                Id = appointment!.Id,
                Date = appointment.Date,
                Notes = appointment.Notes,
                Status = appointment.Status,
                User = user,
                Doctor = doctor,
                CreatedAt = appointment.CreatedAt ?? DateTime.Now,
                UpdatedAt = appointment.UpdatedAt ?? DateTime.Now,
            };

            return viewModel;
        }
        return null;
    }
}