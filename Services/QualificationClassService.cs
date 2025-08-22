using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class QualificationClassService : IQualificationClassService
    {
        private readonly IQualificationClasssRepository _qualificationClassRepository;

        public QualificationClassService(IQualificationClasssRepository qualificationClassRepository)
        {
            _qualificationClassRepository = qualificationClassRepository;
        }

        public async Task<string> AddAsync(QualificationClass qualificationClass)
        {
            try
            {
                await _qualificationClassRepository.AddAsync(qualificationClass);
                return "Qualification Class Detail Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting faculty detail");
                return "error";
            }
        }

        public async Task<IEnumerable<QualificationClass>> GetAllQualificationClassAsync()
        {
            return await _qualificationClassRepository.GetAllQualificationClassAsync();
        }

        public async  Task<QualificationClass> GetQualificationClassByIdAsync(long Id)
        {
            return await _qualificationClassRepository.GetQualificationClassByIdAsync(Id);    
        }

        
    }
}
