using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class InstitutionTypeRepository : IInstitutionTypeRepository
    {
        private readonly AppDbContext _context;
        public InstitutionTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InstitutionType>> GetAllIInstitutionTypeAsync()
        {
            return await _context.InstitutionTypes.ToListAsync();
        }
              
        public async Task<InstitutionType> GetIInstitutionTypeByIdAsync(long Id)
        {
            return await _context.InstitutionTypes.FindAsync(Id);
        }

    }   
    
}
