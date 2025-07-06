using Certitrack.Models;
using Certitrack.ViewModels;

namespace Certitrack.Services
{
    public interface ICertificateDetailService
    {
        Task<IEnumerable<CertificateDetail>> GetAllCertificateDetailAsync();

        Task<CertificateDetail> GetCertificateDetailByIdAsync(long Id);

        Task<IEnumerable<CertificateDetail>> GetCertificateDetailByInstitutionIdAsync(long Id);

        Task<string> InsertCredenttialDetailAsync(CertificateDetail certificateDetail);
        Task<string> BatchInsertAsync(List<CredentialDetailVM> credentialDetails);

        Task<string> UpdateCredenttialDetailAsync(CertificateDetail certificateDetail);
    }
}
