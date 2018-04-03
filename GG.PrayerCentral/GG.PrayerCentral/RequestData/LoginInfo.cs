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
}
