using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<string> AddAsync(Department department)
        {
            try 
            {
                await _departmentRepository.AddAsync(department);
                return "Department Detail Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting faculty detail");
                return "error";
            }
               
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentAsync()
        {
            return await _departmentRepository.GetAllDepartmentAsync();
        }
        public async Task<Department> GetDepartmentByIdAsync(long Id)
        {
            return await _departmentRepository.GetDepartmentByIdAsync(Id);
        }
    }
  
}
