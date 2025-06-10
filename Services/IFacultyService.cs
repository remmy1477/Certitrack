using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IFacultyService
    {
        Task<IEnumerable<Faculty>> GetAllFacultyAsync();
        Task<Faculty> GetFacultyByIdAsync(long Id);
    }
}
