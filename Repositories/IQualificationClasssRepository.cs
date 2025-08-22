using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IQualificationClasssRepository
    {
        Task<IEnumerable<QualificationClass>> GetAllQualificationClassAsync();
        Task<QualificationClass> GetQualificationClassByIdAsync(long Id);

        Task AddAsync(QualificationClass qualificationClass);
    }
}
