using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class FacultyRepository : IFacultyRepository 
    {
        private readonly AppDbContext _context;

        public FacultyRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Faculty>> GetAllFacultyAsync()
        {
            return await _context.Faculties.ToListAsync();
        }
        public async Task<Faculty> GetFacultyByIdAsync(long Id)
        {
            return await _context.Faculties.FindAsync(Id);
        }
    }
}
