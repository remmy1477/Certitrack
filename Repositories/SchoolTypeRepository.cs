using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class SchoolTypeRepository : ISchoolTypeRepository
    {
        private readonly AppDbContext _context;
        public SchoolTypeRepository(AppDbContext context) 
        {
            _context = context; 
        }
        public async Task AddAsync(SchoolType schoolType)
        {
            _context.SchoolTypes.Add(schoolType);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SchoolType>> GetAllSchoolTypesAsync()
        {
            return await _context.SchoolTypes.ToListAsync();
        }

        public async Task<SchoolType> GetSchoolTypeByIdAsync(long Id)
        {
            return await _context.SchoolTypes.FindAsync(Id);
        }
    }
}
