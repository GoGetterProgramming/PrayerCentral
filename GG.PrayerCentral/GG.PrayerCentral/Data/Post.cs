using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Data
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DatePosted { get; set; }

        public long UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public long OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
