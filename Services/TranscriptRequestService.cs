using Certitrack.Data;
using Certitrack.Models;
using Certitrack.Repositories;
using Certitrack.ViewModels;
using System.Runtime.InteropServices;

namespace Certitrack.Services
{
    public class TranscriptRequestService : ITranscriptRequestService
    {
        private readonly ITranscriptRequestRepository _transcriptRequestRepository;
        private readonly AppDbContext _context;
        public TranscriptRequestService(ITranscriptRequestRepository transcriptRequestRepository, AppDbContext context)
        {
            _transcriptRequestRepository = transcriptRequestRepository;
            _context = context;
        }

        public async Task<string> BatchInsertAsync(List<TranscriptRequestVM> transcriptRequests)
        {
            if (transcriptRequests == null || !transcriptRequests.Any())
                return "No valid records provided.";

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;

                var transcriptReqs = transcriptRequests.Select(tr => new TranscriptRequest
                {
                    FirstName = tr.FirstName,
                    LastName = tr.LastName,
                    MiddleName = tr.MiddleName,
                    MatricNumber = tr.MatricNumber,
                    InstitutionId = tr.InstitutionId,
                    SchoolId = tr.SchoolId,
                    FacultyId = tr.FacultyId,
                    Department = tr.Department,
                    YearOfGraduation = tr.YearOfGraduation,
                    DestinationEmail = tr.DestinationEmail,
                    IsPaid = false,
                    Status = VerificationStatus.Pending.ToString(),
                    TranscriptFilePath = "", // Optional, can be set later
                    Created = now,
                    Updated = now,
                    CreatedBy = "System", // or pass this from controller
                    LastModifiedBy = "System"
                }).ToList();

                await _transcriptRequestRepository.AddRangeAsync(transcriptReqs);

                // await _credentialRepository.SaveChangesAsync();

                await transaction.CommitAsync();
                return $"{transcriptRequests.Count} transcript records inserted successfully.";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return "Error occurred during batch insert.";
            }
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
