using System.Linq;
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

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ApplicationUser> AppUsers { get; set; }
    }
}
