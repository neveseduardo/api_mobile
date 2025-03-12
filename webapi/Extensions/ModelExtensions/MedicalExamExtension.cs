using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalExamExtension
{
    public static MedicalExamViewModel ToViewModel(this MedicalExam medicalExam)
    {
        return new MedicalExamViewModel
        {
            Id = medicalExam.Id,
            Name = medicalExam.Name,
            Description = medicalExam.Description,
            CreatedAt = medicalExam.CreatedAt,
            UpdatedAt = medicalExam.UpdatedAt,
        };
    }
}