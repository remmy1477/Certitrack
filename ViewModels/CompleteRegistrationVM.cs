namespace Certitrack.ViewModels
{
    public class CompleteRegistrationVM
    {
        public string Email { get; set; }
        public long RoleId { get; set; }
        public string Token { get; set; }

        public long TypeId { get; set; }

        public long InviteId { get; set; }
        // Fields the user needs to fill:
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long Title { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

       // public string OrganizationName { get; set; }
        public string OrganizationName { get; set; }
        public string? OrganizationAddress { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? OrganizationEmail { get; set; }
        public string? RCNo { get; set; }
        public string? ITNo { get; set; }
        public string? BNNo { get; set; }




    }
}
