using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface ICertificateDetailRepository
    {
        Task<IEnumerable<CertificateDetail>> GetAllCertificateDetailAsync();

        Task<CertificateDetail> GetCertificateDetailByIdAsync(long Id);

        Task<IEnumerable<CertificateDetail>> GetCertificateDetailByInstitutionIdAsync(long Id);

        Task AddAsync(CertificateDetail certificateDetail);
        
        Task SaveChangesAsync();

        Task UpdateAsync(CertificateDetail certificateDetail);
    }
}
