namespace Certitrack.Models
{
    public class UserRegistrationInvite
    {
        public long Id { get; set; }
        public long RoleId { get; set; }

        public long? TypeId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        // public string OrganizationName { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationAddress { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? OrganizationEmail { get; set; }
        public string? RCNo { get; set; }
        public string? ITNo { get; set; }

        public string? BNNo { get; set; }
    }
}
