using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class InstitutionService : IIInstitutionService
    {
        private readonly IInstitutionRepository _institutionRepository;

        public InstitutionService(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }   
        public async Task<IEnumerable<Institution>> GetAllInstitutionsAsync()
        {
            return await _institutionRepository.GetAllInstitutionAsync();
        }

        public async Task<Institution> GetInstitutionByIdAsync(long Id)
        {
            return await _institutionRepository.GetInstitutionByIdAsync(Id);    
        }

        public async Task<string> InsertInstitutionAsync(Institution institution)
        {
            try
            {
                await _institutionRepository.AddAsync(institution);
                return "Organisation Detail Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting credential detail");
                return "error";
            }
        }
    }
}
