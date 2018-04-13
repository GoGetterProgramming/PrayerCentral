using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using GG.PrayerCentral.EnumsAndConstants;
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
            await CreateRole(roleManager, RoleTypes.AdminIdentifier);
            await CreateRole(roleManager, RoleTypes.PastorIdentifier);
            await CreateRole(roleManager, RoleTypes.MemberIdentifier);            
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

                await userManager.CreateAsync(user, "test1234");
            }
        }

        private static async Task SeedUserRoles(ApplicationDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await AddRoleToUser(context, userManager, roleManager, RoleTypes.AdminIdentifier, "tdoell");
            await AddRoleToUser(context, userManager, roleManager, RoleTypes.MemberIdentifier, "tdoell");
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        private static async Task AddRoleToUser(ApplicationDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, string roleName, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            var role = await roleManager.FindByNameAsync(roleName);

            if (context.UserRoles.Any(e => e.UserId == user.Id && e.RoleId == role.Id) == false)
            {
                await userManager.AddToRoleAsync(user, role.Name);
            }
        }
    }
}
