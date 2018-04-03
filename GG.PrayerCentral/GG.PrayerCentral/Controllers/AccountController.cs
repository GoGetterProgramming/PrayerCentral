using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using GG.PrayerCentral.RequestData;
using Google.Apis.Oauth2.v2;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using static Google.Apis.Plus.v1.Data.Person;
using static Google.Apis.Plus.v1.PeopleResource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GG.PrayerCentral.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _DbContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly IConfiguration _Configuration;
        private readonly ILogger _Logger;

        public AccountController(ApplicationDBContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _DbContext = dbContext;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _Configuration = configuration;
            _Logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet(nameof(CheckIsAuthenticated))]
        [AllowAnonymous]
        public IActionResult CheckIsAuthenticated()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

        [HttpPost(nameof(Register))]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationInfo registrationInfo)
        {
            if (ModelState.IsValid)
            {
                var authUser = new ApplicationUser
                {
                    UserName = registrationInfo.Username,
                    Email = registrationInfo.Email,
                    FirstName = registrationInfo.FirstName,
                    LastName = registrationInfo.LastName
                };

                var result = await _UserManager.CreateAsync(authUser, registrationInfo.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest(result);
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInfo loginInfo)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _UserManager.FindByNameAsync(loginInfo.Username);

                if (appUser != null)
                {
                    var result = await _SignInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);

                    if (result.Succeeded)
                    {
                        return Ok(CreateToken(appUser));
                    }
                }

                return Unauthorized();
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            
            _Logger.LogInformation(4, "");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            return Ok();
        }

        [HttpGet("getstring")]
        public IActionResult GetValue()
        {
            foreach (var claim in HttpContext.User.Claims)
            {
                var type = claim.Type;
                var value = claim.Value;
            }

            return Ok("Here is your string");
        }

        #region Token

        private string CreateToken(ApplicationUser user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_Configuration["Jwt:Issuer"], _Configuration["Jwt:Issuer"], claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

        //[HttpGet("googleauth")]
        //public async Task<IActionResult> Get(string code)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        GoogleAuth googleAuth = null;
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GoogleDefaults.TokenEndpoint);
        //        HttpRequestMessage emailRequest = new HttpRequestMessage(HttpMethod.Post, "https://www.googleapis.com/auth/userinfo.profile");

        //        var keyValPair = new List<KeyValuePair<string, string>>
        //        {
        //            new KeyValuePair<string, string>("code", code),
        //            new KeyValuePair<string, string>("client_id", "341901031941-sqsp8qf1886ecbtglshefahbtvde8kom.apps.googleusercontent.com"),
        //            new KeyValuePair<string, string>("client_secret", "CmmsrY2_VYDwn-UCNl55JbEr"),
        //            new KeyValuePair<string, string>("redirect_uri", "http://localhost/GG.PrayerCentral/api/home/googleauth"),
        //            new KeyValuePair<string, string>("grant_type", "authorization_code")
        //        };

        //        request.Content = new FormUrlEncodedContent(keyValPair);

        //        try
        //        {
        //            using (HttpClient httpClient = new HttpClient())
        //            {
        //                using (HttpResponseMessage response = await httpClient.SendAsync(request))
        //                {
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        var content = await response.Content.ReadAsStringAsync();

        //                        googleAuth = JsonConvert.DeserializeObject<GoogleAuth>(content);
        //                    }
        //                    else
        //                    {

        //                    }
        //                }

        //                return await LoginAsync(googleAuth.AccessToken);
        //            }
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //    }

        //    return Ok(ModelState.IsValid);
        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> LoginAsync(string authToken)
        //{
        //    Person person = await GetGoogleUser(authToken);
        //    EmailsData email = person.Emails.FirstOrDefault(); 

        //    var authUser = await _UserManager.FindByEmailAsync(email.Value);

        //    if (authUser == null)
        //    {
        //        authUser = new AuthUser()
        //        {
        //            GoogleId = person.Id,
        //            FirstName = person.Name.GivenName,
        //            LastName = person.Name.FamilyName,
        //            Email = email.Value
        //        };

        //        var result = await _UserManager.CreateAsync(authUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

        //        if (result.Succeeded == false)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        await _DbContext.AppUsers.AddAsync(new ApplicationUser { IdentityId = authUser.Id });
        //        await _DbContext.SaveChangesAsync();
        //    }


        //    var localUser = await _UserManager.FindByNameAsync(authUser.Email);

        //    if (localUser == null)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return Ok(localUser);
        //}

        //private async Task<Person> GetGoogleUser(string authToken)
        //{
        //    Person person = null;
        //    GetRequest request;

        //    using (var service = new PlusService())
        //    {
        //        request = service.People.Get("me");
        //        request.OauthToken = authToken;

        //        try
        //        {
        //            person = await request.ExecuteAsync();
        //        }
        //        catch (Exception)
        //        {
        //            // Ignore
        //        }
        //    }

        //    return person;
        //}
    }
}
