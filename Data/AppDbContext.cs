using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        public DbSet<InstitutionType> InstitutionTypes { get; set; }
        public DbSet<Institution> Institutions { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Tittle> Tittles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRegistrationInvite> UserRegistrationInvites { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var now = new DateTime(2025, 1, 1); // use a fixed time for consistency

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Role { Id = 2, Name = "AdminUser", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Role { Id = 3, Name = "Agent", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Role { Id = 4, Name = "Requester", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Role { Id = 5, Name = "VerifierUser", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Role { Id = 6, Name = "VerifierApproval", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" }
            );

            modelBuilder.Entity<Tittle>().HasData(
                new Tittle { Id = 1, Name = "Mr", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Tittle { Id = 2, Name = "Mrs", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Tittle { Id = 3, Name = "Miss", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Tittle { Id = 4, Name = "Dr", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" },
                new Tittle { Id = 5, Name = "Prof", Created = now, Updated = now, CreatedBy = "System", LastModifiedBy = "System" }
            );

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   Email = "superadmin@certitrack.com",
                   TitleId = 1,
                   PasswordHash = "0192023a7bbd73250516f069df18b500", // Replace with a real hash
                   FirstName = "Super",
                   MiddleName = "",
                   LastName = "Admin",
                   RoleId = 1,
                   PhoneNumber = "08000000000",
                   Address = "HQ Office",
                   Created = now,
                   Updated = now,
                   CreatedBy = "System",
                   LastModifiedBy = "System"
               }
           );
            modelBuilder.Entity<UserRole>().HasData(
                  new UserRole
                  {
                      Id = 1,
                      UserId = 1,
                     
                      RoleId = 1,
                     
                      Created = now,
                      Updated = now,
                      CreatedBy = "System",
                      LastModifiedBy = "System"
                  }
              );
        }
    }
}
