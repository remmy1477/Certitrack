using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface ITranscriptRequestRepository
    {
        Task<IEnumerable<TranscriptRequest>> GetAllTranscriptRequestAsync();

        Task<TranscriptRequest> GetTranscriptRequestByIdAsync(long Id);

        Task<IEnumerable<TranscriptRequest>> GetTranscriptRequestByInstitutionIdAsync(long Id);

        Task AddAsync(TranscriptRequest transcriptRequest);

        Task SaveChangesAsync();

        Task UpdateAsync(TranscriptRequest transcriptRequest);
    }
}
