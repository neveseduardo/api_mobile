using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class DoctorExtension
{
    public static DoctorViewModel ToViewModel(this Doctor doctor)
    {
        return new DoctorViewModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Email = doctor.Email,
            CRM = doctor.CRM,
            Especialization = doctor.Especialization?.ToViewModel(),
            CreatedAt = doctor.CreatedAt,
            UpdatedAt = doctor.UpdatedAt,
        };
    }
}