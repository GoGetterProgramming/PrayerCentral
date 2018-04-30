using System.ComponentModel.DataAnnotations;

namespace GG.PrayerCentral.Data
{
    public class UserOrganization
    {
        public bool IsAdmin { get; set; }
        public long OrganizationId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
