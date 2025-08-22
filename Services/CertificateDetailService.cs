using Certitrack.Data;
using Certitrack.Models;
using Certitrack.Repositories;
using Certitrack.ViewModels;

namespace Certitrack.Services
{
    public class CertificateDetailService : ICertificateDetailService
    {
        private readonly ICertificateDetailRepository _certificateDetailRepository;
        private readonly AppDbContext _context;

        public CertificateDetailService(ICertificateDetailRepository certificateDetailRepository,AppDbContext context)
        {
            _certificateDetailRepository = certificateDetailRepository;
            _context = context;
        }

        public async Task<string> BatchInsertAsync(List<CredentialDetailVM> credentialDetails)
        {
            if (credentialDetails == null || !credentialDetails.Any())
                return "No valid records provided.";

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;

                var certificateDetails = credentialDetails.Select(vm => new CertificateDetail
                {
                    HolderFirstName = vm.HolderFirstName,
                    HolderLastName = vm.HolderLastName,
                    HolderMiddleName = vm.HolderMiddleName,
                    HolderEmail = vm.HolderEmail,
                    HolderPhoneNumber = vm.HolderPhoneNumber,
                    HolderAddress = vm.HolderAddress,
                    CertificateNo = vm.CertificateNo,
                    DegreeId = vm.Degree,
                    DegreeClassId = vm.DegreeClass,
                    FacultyId = vm.Faculty,
                    Department = vm.Department,
                    YearOfGraduation = vm.YearOfGraduation,
                    SchoolId = vm.SchoolId,
                    InstitutionId = vm.InstitutionId,
                    IsVerified = false,
                    Ispaid = false,
                    Status = VerificationStatus.Pending.ToString(),
                    Created = now,
                    Updated = now,
                    CreatedBy = "System", // or pass this from controller
                    LastModifiedBy = "System"
                }).ToList();

                await _certificateDetailRepository.AddRangeAsync(certificateDetails);    
                    
               // await _credentialRepository.SaveChangesAsync();

                await transaction.CommitAsync();
                return $"{certificateDetails.Count} certificate records inserted successfully.";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return "Error occurred during batch insert.";
            }
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
                return "Certificate Detail Inserted ";
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
                return "Certificate Detail Updated";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error updating credential detail");
                return "error";
            }   
        }
    }
}
