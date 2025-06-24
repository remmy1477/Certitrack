namespace Certitrack.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }

        public long TitleId { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long RoleId { get; set; }

        public long? InviteId { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public bool IsActive { get; set; } = false;
        public string? ApprovalToken { get; set; }  // Used for secure approval links

        
    }
}
