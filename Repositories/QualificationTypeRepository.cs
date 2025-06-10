using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class QualificationTypeRepository : IQualificationTypesRepository
    {
        private readonly AppDbContext _context;

        public QualificationTypeRepository(AppDbContext context)
        {
            _context = context;
        }   
        public  async Task<IEnumerable<QualificationType>> GetAllQualificationTypeAsync()
        {
            return await _context.QualificationTypes.ToListAsync();
        }

        public async Task<QualificationType> GetQualificationTypeByIdAsync(long Id)
        {
            return await _context.QualificationTypes.FindAsync(Id);
        }
    }
}
