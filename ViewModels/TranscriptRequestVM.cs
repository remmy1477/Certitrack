namespace Certitrack.ViewModels
{
    public class TranscriptRequestVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MatricNumber { get; set; }
        public long InstitutionId { get; set; }

        public long SchoolId { get; set; }
        public long FacultyId { get; set; }
        public string Department { get; set; }
        public string YearOfGraduation { get; set; }
        public string StudentEmail { get; set; } = string.Empty; // Optional, can be empty if not provided
        public string DestinationEmail { get; set; }
        public string? Status { get; set; } // Pending, Sent, Rejected

        public bool Ispaid { get; set; }
        public string? TranscriptFilePath { get; set; } // Optional
        public string? NonVerificationReason { get; set; }
        public string? NonApprovalReason { get; set; }

    }
}
