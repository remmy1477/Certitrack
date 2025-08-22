using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentAsync();
        Task<Department> GetDepartmentByIdAsync(long Id);
        Task<string> AddAsync(Department department);
    }
}
