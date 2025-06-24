using Certitrack.Models;

namespace Certitrack.ViewModels
{
    public class CredentialDetailVM
    {
        public string HolderFirstName { get; set; }

        public string HolderLastName { get; set; }

        public string HolderMiddleName { get; set; }
        public string HolderEmail { get; set; }
        public string HolderPhoneNumber { get; set; }
        public string HolderAddress { get; set; }
        public string CertificateNo { get; set; }

        public long Degree { get; set; }

        public long DegreeClass { get; set; }

        public long Faculty { get; set; }

        public string Department { get; set; }
        public string YearOfGraduation { get; set; }
        public long SchoolId { get; set; }
        public long InstitutionId { get; set; }

        public bool? IsVerified { get; set; } = false;
        public bool Ispaid { get; set; } = false ;
        public string? Status { get; set; }

    }
}
