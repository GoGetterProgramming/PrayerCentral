using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Request.PostData
{
    public class RequestCreatePost
    {
        public List<long> OrganizationIds { get; set; } = new List<long>();

        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
