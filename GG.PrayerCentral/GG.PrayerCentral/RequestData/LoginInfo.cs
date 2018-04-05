using GG.PrayerCentral.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace GG.PrayerCentral.RequestData
{
    public class LoginInfo
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }

    public class RefreshTokenInfo
    {
        public string RefreshToken { get; set; }
    }
}
