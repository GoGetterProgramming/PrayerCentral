﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using GG.PrayerCentral.Data;
using GG.PrayerCentral.DBContext;
using GG.PrayerCentral.RequestData;
using GG.PrayerCentral.Attributes;
using GG.PrayerCentral.EnumsAndConstants;
using System.Collections.Generic;

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

        public AccountController(ApplicationDBContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
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

        [HttpPost(nameof(RegisterUser))]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationModel registrationInfo)
        {
            IdentityResult result = null;

            try
            {
                var authUser = new ApplicationUser
                {
                    UserName = registrationInfo.Username,
                    Email = registrationInfo.Email,
                    FirstName = registrationInfo.FirstName,
                    LastName = registrationInfo.LastName
                };

                result = await _UserManager.CreateAsync(authUser, registrationInfo.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                // Ignore
            }

            return BadRequest(result);
        }

        [HttpPost(nameof(RegisterOrganization))]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterOrganization([FromBody] RegistrationModel registrationInfo)
        {
            return Ok();
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
        public async Task<IActionResult> Login([FromBody] LoginModel loginInfo)
        {
            RefreshToken refreshToken;
            LoginResponse loginResponse;

            if (ModelState.IsValid)
            {
                var appUser = await _UserManager.FindByNameAsync(loginInfo.Username);

                if (appUser != null)
                {
                    var result = await _SignInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);

                    if (result.Succeeded)
                    {
                        refreshToken = await GetRefreshToken(appUser);

                        loginResponse = new LoginResponse
                        {
                            AccessToken = await CreateToken(appUser),
                            RefreshToken = refreshToken,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                        };

                        return Ok(loginResponse);
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

        [HttpPost(nameof(GetString))]
        
        public IActionResult GetString()
        {
            return Ok(HttpContext.User.Identity.IsAuthenticated);
        }

        [HttpPost(nameof(RefreshToken))]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken.RefreshToken))
            {
                return Unauthorized();
            }

            var refreshTokenModel = _DbContext.RefreshTokens.Include(e => e.User).SingleOrDefault(e => e.Token == refreshToken.RefreshToken);

            if (refreshTokenModel == null)
            {
                return Unauthorized();
            }

            if (await _SignInManager.CanSignInAsync(refreshTokenModel.User) == false)
            {
                return Unauthorized();
            }

            if (_UserManager.SupportsUserLockout && await _UserManager.IsLockedOutAsync(refreshTokenModel.User))
            {
                return Unauthorized();
            }

            var user = refreshTokenModel.User;
            var token = GetRefreshToken(user);

            return Ok(token);
        }

        #region Token

        private async Task<string> CreateToken(ApplicationUser user)
        {
            var now = DateTime.UtcNow;
            var token = await GetSecurityToken(user, now, now.AddMinutes(30));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<RefreshToken> GetRefreshToken(ApplicationUser user)
        {
            RefreshToken refreshToken = _DbContext.RefreshTokens.SingleOrDefault(e => e.UserId == user.Id);

            if (refreshToken == null || refreshToken.ExpiresUtc < DateTime.UtcNow)
            {
                if (refreshToken != null)
                {
                    _DbContext.RefreshTokens.Remove(refreshToken);
                }

                refreshToken = new RefreshToken
                {
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddYears(1)
                };

                var token = await GetSecurityToken(user, refreshToken.IssuedUtc, refreshToken.ExpiresUtc);

                refreshToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
                refreshToken.User = user;

                await _DbContext.RefreshTokens.AddAsync(refreshToken);
                await _DbContext.SaveChangesAsync();
            }

            return refreshToken;
        }

        private async Task<JwtSecurityToken> GetSecurityToken(ApplicationUser user, DateTime issuedUtc, DateTime expiresUtc)
        {
            JwtSecurityToken token;
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            foreach (var roleName in await _UserManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            token = new JwtSecurityToken(_Configuration["Jwt:Issuer"], _Configuration["Jwt:Issuer"], claims, issuedUtc, expiresUtc, creds);

            return token;
        }

        #endregion
    }
}
