using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class EspecializationExtension
{
    public static EspecializationViewModel ToViewModel(this Especialization especialization)
    {
        return new EspecializationViewModel
        {
            Id = especialization.Id,
            Name = especialization.Name,
            Description = especialization.Description,
            CreatedAt = especialization.CreatedAt,
            UpdatedAt = especialization.UpdatedAt,
        };
    }
}