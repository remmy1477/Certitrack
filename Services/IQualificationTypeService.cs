using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IQualificationTypeService
    {
        Task<IEnumerable<QualificationType>> GetAllQualificationTypeAsync();
        Task<QualificationType> GetQualificationTypeByIdAsync(long Id);

        Task<string> AddAsync(QualificationType qualificationType);

    }
}
