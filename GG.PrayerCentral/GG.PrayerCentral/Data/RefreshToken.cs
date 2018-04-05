using System;

namespace GG.PrayerCentral.Data
{
    public class RefreshToken : BaseEntity
    {
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string Token { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
