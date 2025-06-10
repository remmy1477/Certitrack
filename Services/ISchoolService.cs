using Certitrack.Models;

namespace Certitrack.Services
{
    public interface ISchoolService
    {
        Task<IEnumerable<School>> GetAllSchoolsAsync();
        Task<School> GetSchoolByIdAsync(long Id);
    }
}
