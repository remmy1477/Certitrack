using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly AppDbContext _context;
        public InstitutionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Institution>> GetAllInstitutionAsync()
        {
            return await _context.Institutions.ToListAsync();
        }

        public async Task<Institution> GetInstitutionByIdAsync(long Id)
        {
            return await _context.Institutions.FindAsync(Id);
        }
    }
}
