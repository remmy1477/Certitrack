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
        public string? OrganizationName { get; set; } = string.Empty;
        public string? OrganizationAddress { get; set; } = string.Empty;
        public string? OrganizationPhone { get; set; } = string.Empty;
        public string? OrganizationEmail { get; set; } = string.Empty;
        public string? RCNo { get; set; } = string.Empty;
        public string? ITNo { get; set; } = string.Empty;

        public string? BNNo { get; set; } = string.Empty;

        public long? SchoolId { get; set; } = 0;

        public string? activationlink { get; set; }
    }
}
