using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalCenterExtension
{
    public static MedicalCenterViewModel ToViewModel(this MedicalCenter medicalCenter)
    {
        return new MedicalCenterViewModel
        {
            Id = medicalCenter.Id,
            Name = medicalCenter.Name,
            PhoneNumber = medicalCenter.PhoneNumber,
            Email = medicalCenter.Email,
            address = medicalCenter.Address?.ToViewModel(),
        };
    }
}