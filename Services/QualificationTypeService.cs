using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class QualificationTypeService : IQualificationTypeService
    {
        private readonly IQualificationTypesRepository _qualificationTypeRepository;

        public QualificationTypeService(IQualificationTypesRepository qualificationTypeRepository)
        {
            _qualificationTypeRepository = qualificationTypeRepository;
        }
        public async Task<IEnumerable<QualificationType>> GetAllQualificationTypeAsync()
        {
            return await _qualificationTypeRepository.GetAllQualificationTypeAsync();
        }

        public async Task<QualificationType> GetQualificationTypeByIdAsync(long Id)
        {
            return await _qualificationTypeRepository.GetQualificationTypeByIdAsync(Id);    
        }
    }
}
