using Newtonsoft.Json;

namespace GG.PrayerCentral.Data
{
    public class GoogleAuth : BaseEntity
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("expires_in")]
        public short ExpiresIn { get; set; }
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        public long UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
