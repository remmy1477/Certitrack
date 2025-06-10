using Certitrack.Models;

namespace Certitrack.ViewModels
{
    public class CredentialDetailIndexVM
    {
        public long Id { get; set; }
        public string HolderFirstName { get; set; }

        public string HolderLastName { get; set; }

        public string HolderMiddleName { get; set; }
        public string HolderEmail { get; set; }
        public string HolderPhoneNumber { get; set; }
        public string HolderAddress { get; set; }
        public string CertificateNo { get; set; }

        public long Degree { get; set; }
        public string DegreeNm { get; set; }

        public long DegreeClass { get; set; }
        public string DegreeClassNm { get; set; }

        public long Faculty { get; set; }
        public string FacultyNm { get; set; }

        public string Department { get; set; }
        public string YearOfGraduation { get; set; }
        public long SchoolId { get; set; }
        public long InstitutionId { get; set; }

        public string SchoolNm { get; set; }
        public string InstitutionNm { get; set; }

        public bool IsVerified { get; set; }
        public string Status { get; set; }
    }
}
