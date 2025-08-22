using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IQualificationTypesRepository
    {
        Task<IEnumerable<QualificationType>> GetAllQualificationTypeAsync();
        Task<QualificationType> GetQualificationTypeByIdAsync(long Id);

        Task AddAsync(QualificationType qualificationType);
    }
}
