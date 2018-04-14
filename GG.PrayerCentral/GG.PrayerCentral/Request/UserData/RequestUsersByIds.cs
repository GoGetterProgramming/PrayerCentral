using System.Collections.Generic;

namespace GG.PrayerCentral.Request.UserData
{
    public class RequestUsersByIds
    {
        public List<string> UserIds { get; set; } = new List<string>();
    }
}
