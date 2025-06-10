using Certitrack.Models;
 
namespace Certitrack.Services
{
    public interface ITranscriptRequestService
    {
        Task<IEnumerable<TranscriptRequest>> GetAllTranscriptRequestAsync();

        Task<TranscriptRequest> GetTranscriptRequestByIdAsync(long Id);

        Task<IEnumerable<TranscriptRequest>> GetTranscriptRequestByInstitutionIdAsync(long Id);

        Task<string> InsertTranscriptRequestAsync(TranscriptRequest transcriptRequest);

        Task<string> UpdateTranscriptRequestAsync(TranscriptRequest transcriptRequest);
    }
}
