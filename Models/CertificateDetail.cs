using System.ComponentModel.DataAnnotations;

namespace Certitrack.Models
{
    public class CertificateDetail
    {
        [Key]
        public long Id { get; set; }
        public string HolderFirstName { get; set; }

        public string HolderLastName { get; set; }

        public string HolderMiddleName { get; set; }
        public string HolderEmail { get; set; }
        public string HolderPhoneNumber { get; set; }
        public string HolderAddress { get; set; }
        public string CertificateNo { get; set; }

        public long DegreeId { get; set; }

        public long DegreeClassId { get; set; }

        public long FacultyId { get; set; }

        public string Department { get; set; }
        public string YearOfGraduation { get; set; }
        public long SchoolId { get; set; }
        public long InstitutionId { get; set; }

        public bool IsVerified { get; set; } = false;
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

    }
}
