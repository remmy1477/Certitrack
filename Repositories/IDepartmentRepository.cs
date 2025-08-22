using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentAsync();

        Task<Department> GetDepartmentByIdAsync(long Id);

        Task AddAsync(Department department);
    }
}
