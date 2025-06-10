using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class CertificateDetailService : ICertificateDetailService
    {
        private readonly ICertificateDetailRepository _certificateDetailRepository;

        public CertificateDetailService(ICertificateDetailRepository certificateDetailRepository)
        {
            _certificateDetailRepository = certificateDetailRepository;
        }
        public async Task<IEnumerable<CertificateDetail>> GetAllCertificateDetailAsync()
        {
            return await _certificateDetailRepository.GetAllCertificateDetailAsync();
        }

        public async Task<CertificateDetail> GetCertificateDetailByIdAsync(long Id)
        {
            return await _certificateDetailRepository.GetCertificateDetailByIdAsync(Id);
        }

        public async Task<IEnumerable<CertificateDetail>> GetCertificateDetailByInstitutionIdAsync(long Id)
        {
            return await _certificateDetailRepository.GetCertificateDetailByInstitutionIdAsync(Id);
        }

        public async Task<string> InsertCredenttialDetailAsync(CertificateDetail certificateDetail)
        {
            try
            {
                await _certificateDetailRepository.AddAsync(certificateDetail);
                return "Creadential Detail Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting credential detail");
                return "error";
            }
        }

        public async Task<string> UpdateCredenttialDetailAsync(CertificateDetail certificateDetail)
        {
            try
            {
                await _certificateDetailRepository.UpdateAsync(certificateDetail);
                return "Creadential Detail Updated";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error updating credential detail");
                return "error";
            }   
        }
    }
}
