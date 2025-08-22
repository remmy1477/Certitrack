using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Department department)
        {
                _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Department>> GetAllDepartmentAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(long Id)
        {
            return await _context.Departments.FindAsync(Id);
        }

       
    }
}
