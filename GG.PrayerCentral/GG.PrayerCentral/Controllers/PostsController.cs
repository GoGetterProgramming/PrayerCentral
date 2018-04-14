using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using System.Threading.Tasks;
using GG.PrayerCentral.Request.PostData;
using System;
using System.Linq;

namespace GG.PrayerCentral.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDBContext _DbContext;
        private readonly UserManager<ApplicationUser> _UserManager;

        public PostsController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _DbContext = context;
            _UserManager = userManager;
        }

        public async Task<IActionResult> CreatePost([FromBody] RequestCreatePost request)
        {
            Post post;
            Organization organization;
            ApplicationUser user = await _UserManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (user == null)
            {
                return BadRequest("Unable to find user");
            }

            post = new Post
            {
                DatePosted = DateTime.UtcNow,
                Title = request.Title,
                Message = request.Message
            };

            foreach (long orgId in request.OrganizationIds)
            {
                organization = await _DbContext.Organizations.FindAsync(orgId);

                if (organization != null)
                {
                    post.Organizations.Add(organization);
                }
            }

            return Ok();
        }
    }
}