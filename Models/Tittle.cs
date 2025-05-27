namespace Certitrack.Models
{
    public class Tittle
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }


    }
}
