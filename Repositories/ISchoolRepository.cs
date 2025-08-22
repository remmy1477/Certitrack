using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface ISchoolRepository
    {
        Task<IEnumerable<School>> GetAllSchoolsAsync();
        Task<School> GetSchoolByIdAsync(long Id);

        Task AddAsync(School school);
    }
}
