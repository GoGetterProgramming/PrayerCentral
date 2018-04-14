using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Data
{
    public class Post : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public long UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public List<long> OrganizationIds { get; set; }
        public virtual List<Organization> Organizations { get; set; }
    }
}
