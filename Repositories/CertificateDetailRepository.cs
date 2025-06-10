using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Certitrack.Repositories
{
    public class CertificateDetailRepository : ICertificateDetailRepository
    {
        private readonly AppDbContext _context;
        public CertificateDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CertificateDetail certificateDetail)
        {

            _context.CertificateDetails.Add(certificateDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CertificateDetail>> GetAllCertificateDetailAsync()
        {
            return await _context.CertificateDetails.ToListAsync();
        }

        public async Task<CertificateDetail> GetCertificateDetailByIdAsync(long Id)
        {
            return await _context.CertificateDetails.FindAsync(Id);
        }

        public async Task<IEnumerable<CertificateDetail>> GetCertificateDetailByInstitutionIdAsync(long InstitutionId)
        {
            return await _context.CertificateDetails
                .Where(cd => cd.InstitutionId == InstitutionId)
                .ToListAsync(); 
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CertificateDetail certificateDetail)
        {
            _context.CertificateDetails.Update(certificateDetail);
            await _context.SaveChangesAsync();
        }
    }
}
