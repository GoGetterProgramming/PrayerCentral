using System.ComponentModel.DataAnnotations;

namespace GG.PrayerCentral.Request.UserData
{
    public class RequestUserById
    {
        [Required]
        public string Id { get; set; }
    }
}
