using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IQualificationClassService
    {
        Task<IEnumerable<QualificationClass>> GetAllQualificationClassAsync();
        Task<QualificationClass> GetQualificationClassByIdAsync(long Id);

        Task<string> AddAsync(QualificationClass qualificationClass);

    }
}
