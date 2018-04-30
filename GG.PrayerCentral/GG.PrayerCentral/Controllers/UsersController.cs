using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using GG.PrayerCentral.ResponseData;
using GG.PrayerCentral.EnumsAndConstants;
using GG.PrayerCentral.Request.UserData;

namespace GG.PrayerCentral.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _DbContext;
        private readonly UserManager<ApplicationUser> _UserManager;

        public UsersController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _DbContext = context;
            _UserManager = userManager;
        }

        [HttpPost(nameof(GetUser))]
        public async Task<IActionResult> GetUser([FromBody] RequestUserById requestUser)
        {
            ResponseBundle<ApplicationUser> response;
            ApplicationUser user = await _UserManager.FindByIdAsync(requestUser.Id);

            if (user == null)
            {
                response = new ResponseBundle<ApplicationUser>(user, ResponseStatus.NotFound, "Requested user not found.");
            }
            else
            {
                response = new ResponseBundle<ApplicationUser>(user);
            }

            return Ok(response);
        }

        [HttpPost(nameof(GetUsers))]
        public async Task<IActionResult> GetUsers([FromBody] RequestUsersByIds requestUsers)
        {
            ApplicationUser user;
            List<ApplicationUser> users = new List<ApplicationUser>();

            requestUsers.UserIds.RemoveAll(e => string.IsNullOrWhiteSpace(e));

            foreach (string userId in requestUsers.UserIds)
            {
                user = await _UserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    users.Add(user);
                }
            }
            
            return Ok(new ResponseBundle<List<ApplicationUser>>(users));
        }
    }
}