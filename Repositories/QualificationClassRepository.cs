using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class QualificationClassRepository : IQualificationClasssRepository
    {
        private readonly AppDbContext _context;

        public QualificationClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(QualificationClass qualificationClass)
        {
            _context.QualificationClasses.AddAsync(qualificationClass);
            await _context.SaveChangesAsync();
        }

        public  async Task<IEnumerable<QualificationClass>> GetAllQualificationClassAsync()
        {
            return await _context.QualificationClasses.ToListAsync();
        }

        public async Task<QualificationClass> GetQualificationClassByIdAsync(long Id)
        {
            return await _context.QualificationClasses.FindAsync(Id);
        }
    }
}
