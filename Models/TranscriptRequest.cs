namespace Certitrack.Models
{
    public class TranscriptRequest
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MatricNumber { get; set; }
        public long InstitutionId { get; set; }
        public long SchoolId { get; set; }
        public long FacultyId { get; set; }
        public string Department { get; set; }
        public string YearOfGraduation { get; set; }
        public string DestinationEmail { get; set; }
        public string Status { get; set; } // Pending, Sent, Rejected
        public string? TranscriptFilePath { get; set; } // Optional

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public string? NonVerificationReason { get; set; } // Optional, for rejected requests
        public string? NonApprovalReason { get; set; }
    }
}
