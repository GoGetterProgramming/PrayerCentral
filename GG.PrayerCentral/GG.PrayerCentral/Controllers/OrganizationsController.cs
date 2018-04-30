using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using GG.PrayerCentral.EnumsAndConstants;
using GG.PrayerCentral.Request.OrganizationData;
using GG.PrayerCentral.ResponseData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private readonly ApplicationDBContext _DbContext;
        private readonly UserManager<ApplicationUser> _UserManager;

        public OrganizationsController(ApplicationDBContext dBContext, UserManager<ApplicationUser> userManager)
        {
            _DbContext = dBContext;
            _UserManager = userManager;
        }

        [HttpPost(nameof(CreateOrganization))]
        [Authorize(Roles = RoleTypes.AdminIdentifier)]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationData request)
        {
            ResponseBundle<Organization> response;

            Organization organization = new Organization
            {
                Name = request.Name,
                ShortName = request.ShortName,
                WebsiteUrl = request.WebsiteUrl,
                Address1 = request.Address1,
                Address2 = request.Address2,
                State = request.State,
                City = request.City,
                ZipCode = request.ZipCode
            };

            await _DbContext.Organizations.AddAsync(organization);

            try
            {
                if (await _DbContext.SaveChangesAsync() > 0)
                {
                    response = new ResponseBundle<Organization>(organization);
                }
                else
                {
                    response = new ResponseBundle<Organization>(null, ResponseStatus.Failed, "Unable to add Organization");
                }
            }
            catch (Exception ex)
            {
                response = new ResponseBundle<Organization>(null, ResponseStatus.Error, ex.ToString());
            }

            return Ok(response);
        }

        [HttpPost(nameof(SetOrganizationAdmin))]
        [Authorize(Roles = RoleTypes.AdminIdentifier)]
        public async Task<IActionResult> SetOrganizationAdmin([FromBody] Organization organization, [FromBody] ApplicationUser applicationUser)
        {
            IActionResult actionResult;

            if (_DbContext.UserOrganizations.SingleOrDefault(e => e.ApplicationUserId == applicationUser.Id && e.OrganizationId == organization.Id) == null)
            {
                UserOrganization userOrganization = new UserOrganization
                {
                    IsAdmin = true,
                    ApplicationUser = applicationUser,
                    Organization = organization
                };

                _DbContext.UserOrganizations.Add(userOrganization);

                if (await _DbContext.SaveChangesAsync() > 0)
                {
                    if (await _UserManager.IsInRoleAsync(applicationUser, RoleTypes.OrganizationAdminIdentifier) == false)
                    {
                        await _UserManager.AddToRoleAsync(applicationUser, RoleTypes.OrganizationAdminIdentifier);
                    }

                    actionResult = Ok();
                }
                else
                {
                    actionResult = BadRequest("Unable to set user as Admin of organization");
                }
            }
            else
            {
                actionResult = BadRequest("User is already admin of Organization");
            }

            return actionResult;
        }

        [HttpPost(nameof(AddUserToOrganization))]
        public async Task<IActionResult> AddUserToOrganization([FromBody] string organizationCode)
        {
            IActionResult actionResult;
            UserOrganization userOrganization;

            if (_DbContext.Organizations.SingleOrDefault(e => e.JoinCode == organizationCode) is Organization organization)
            {
                if (await _UserManager.GetUserAsync(HttpContext.User) is ApplicationUser applicationUser)
                {
                    applicationUser = await _UserManager.GetUserAsync(HttpContext.User);

                    userOrganization = new UserOrganization
                    {
                        IsAdmin = true,
                        ApplicationUser = applicationUser,
                        Organization = organization
                    };

                    _DbContext.UserOrganizations.Add(userOrganization);

                    if (await _DbContext.SaveChangesAsync() > 0)
                    {
                        if (await _UserManager.IsInRoleAsync(applicationUser, RoleTypes.OrganizationAdminIdentifier) == false)
                        {
                            await _UserManager.AddToRoleAsync(applicationUser, RoleTypes.OrganizationAdminIdentifier);
                        }

                        actionResult = Ok();
                    }
                    else
                    {
                        actionResult = BadRequest("Unable to set user as Admin of organization");
                    }
                }
                else
                {
                    actionResult = BadRequest("Unable find user");
                }
            }
            else
            {
                actionResult = BadRequest("Unable to find organization");
            }

            return actionResult;
        }
    }
}