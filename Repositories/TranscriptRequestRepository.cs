using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class TranscriptRequestRepository : ITranscriptRequestRepository
    {
        private readonly AppDbContext _context;
        public TranscriptRequestRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TranscriptRequest transcriptRequest)
        {
            _context.TranscriptRequests.AddAsync(transcriptRequest);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<TranscriptRequest> transcriptRequests)
        {
            _context.TranscriptRequests.AddRange(transcriptRequests);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TranscriptRequest>> GetAllTranscriptRequestAsync()
        {
            return await _context.TranscriptRequests.ToListAsync();
        }

        public async Task<TranscriptRequest> GetTranscriptRequestByIdAsync(long Id)
        {
            return await _context.TranscriptRequests.FindAsync(Id);
        }

        public async Task<IEnumerable<TranscriptRequest>> GetTranscriptRequestByInstitutionIdAsync(long InstitutionId)
        {
            return await _context.TranscriptRequests
                .Where(tr => tr.InstitutionId == InstitutionId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TranscriptRequest transcriptRequest)
        {
            _context.TranscriptRequests.Update(transcriptRequest);
            await _context.SaveChangesAsync();
        }
    }
}
