using GG.PrayerCentral.Data;
using System.ComponentModel.DataAnnotations;

namespace GG.PrayerCentral.Request.OrganizationData
{
    public class JoinOrganizationData
    {
        [Required]
        public string Code { get; set; }
    }
}
