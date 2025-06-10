using Certitrack.Models;
using Certitrack.Repositories;
using System.Runtime.InteropServices;

namespace Certitrack.Services
{
    public class TranscriptRequestService : ITranscriptRequestService
    {
        private readonly ITranscriptRequestRepository _transcriptRequestRepository;
        public TranscriptRequestService(ITranscriptRequestRepository transcriptRequestRepository)
        {
            _transcriptRequestRepository = transcriptRequestRepository;
        }
        public async Task<IEnumerable<TranscriptRequest>> GetAllTranscriptRequestAsync()
        {
            return await _transcriptRequestRepository.GetAllTranscriptRequestAsync();
        }

        public async Task<TranscriptRequest> GetTranscriptRequestByIdAsync(long Id)
        {
            return await _transcriptRequestRepository.GetTranscriptRequestByIdAsync(Id);
        }

        public async Task<IEnumerable<TranscriptRequest>> GetTranscriptRequestByInstitutionIdAsync(long Id)
        {
            return await _transcriptRequestRepository.GetTranscriptRequestByInstitutionIdAsync(Id);
        }

        public async Task<string> InsertTranscriptRequestAsync(TranscriptRequest transcriptRequest)
        {
            try
            {
                await _transcriptRequestRepository.AddAsync(transcriptRequest);
                return "Transcript Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting credential detail");
                return "error";
            }
        }

        public async Task<string> UpdateTranscriptRequestAsync(TranscriptRequest transcripRequest)
        {
            try
            { 
                await _transcriptRequestRepository.UpdateAsync(transcripRequest);
                return "Transcript Updated";    
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting credential detail");
                return "error";
            }
        }
    }
}
