using Certitrack.Models;
using System.Threading.Tasks;

namespace Certitrack.Services
{
    public interface IFacultyService
    {
        Task<IEnumerable<Faculty>> GetAllFacultyAsync();
        Task<Faculty> GetFacultyByIdAsync(long Id);
        Task<string> AddAsync(Faculty faculty);
    }
}
