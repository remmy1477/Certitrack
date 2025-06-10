using System.ComponentModel.DataAnnotations;

namespace Certitrack.Models
{
    public class School
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long SchoolTypeId { get; set; }

        public string Country { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

    }
}
