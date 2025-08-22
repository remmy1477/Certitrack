using System.ComponentModel.DataAnnotations;

namespace Certitrack.Models
{
    public class Department
    {
        [Key]   
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
