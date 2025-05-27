namespace Certitrack.Models
{
    public class Institution
    {
        public long Id { get; set; }
        public long InstitutionTypeId { get; set; }
        public string Name { get; set; }
        public string VerifiedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "PENDING";

        public string CACNumber { get; set; } = string.Empty;
        public DateOnly? DateVerified { get; set; }
        public string Comment { get; set; } = string.Empty;

        public string EmailDomain { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
