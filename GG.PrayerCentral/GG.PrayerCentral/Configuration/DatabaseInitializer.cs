using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Configuration
{
    public class DatabaseInitializer
    {
        public static async Task Initialize(ApplicationDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await SeedUserRoles(context, userManager, roleManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await CreateRole(roleManager, "Admin");
            await CreateRole(roleManager, "Pastor");
            await CreateRole(roleManager, "Member");            
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByNameAsync("tdoell") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "tdoell",
                    Email = "doell4nuggets@gmail.com",
                    FirstName = "Taylor",
                    LastName = "Doell"
                };

                await userManager.CreateAsync(user);
            }
        }

        private static async Task SeedUserRoles(ApplicationDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = await userManager.FindByNameAsync("tdoell");
            var role = await roleManager.FindByNameAsync("Admin");

            if (context.UserRoles.Any(e => e.UserId == user.Id && e.RoleId == role.Id) == false)
            {
                await userManager.AddToRoleAsync(user, role.Name);
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }
    }
}
