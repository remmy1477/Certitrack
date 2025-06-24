namespace Certitrack.ViewModels
{
    public class TranscriptRequestIndexVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MatricNumber { get; set; }

        public long SchoolId { get; set; }
        public string SchoolName { get; set; }
        public long InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public long FacultyId { get; set; }

        public string FacultyName { get; set; }
        public string Department { get; set; }
        public string YearOfGraduation { get; set; }
        public string DestinationEmail { get; set; }

        public bool IsPaid { get; set; }
        public string? Status { get; set; } // Pending, Sent, Rejected
        public string? TranscriptFilePath { get; set; } // Optional
        public string? NonVerificationReason { get; set; }

        public string? NonApprovalReason { get; set; }
    }
}
