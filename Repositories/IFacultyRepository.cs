using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculty>> GetAllFacultyAsync();

        Task<Faculty> GetFacultyByIdAsync(long Id);

        Task AddAsync(Faculty faculty);
    }
}
