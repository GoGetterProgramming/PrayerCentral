using GG.PrayerCentral.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GG.PrayerCentral.DBContext
{
    public partial class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserOrganization>().HasKey(c => new { c.OrganizationId, c.ApplicationUserId });

            base.OnModelCreating(builder);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }
    }
}
