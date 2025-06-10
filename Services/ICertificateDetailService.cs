using Certitrack.Models;

namespace Certitrack.Services
{
    public interface ICertificateDetailService
    {
        Task<IEnumerable<CertificateDetail>> GetAllCertificateDetailAsync();

        Task<CertificateDetail> GetCertificateDetailByIdAsync(long Id);

        Task<IEnumerable<CertificateDetail>> GetCertificateDetailByInstitutionIdAsync(long Id);

        Task<string> InsertCredenttialDetailAsync(CertificateDetail certificateDetail);

        Task<string> UpdateCredenttialDetailAsync(CertificateDetail certificateDetail);
    }
}
