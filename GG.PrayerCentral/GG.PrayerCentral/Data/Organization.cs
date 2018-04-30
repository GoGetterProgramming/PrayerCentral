using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Data
{
    public class Organization : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string WebsiteUrl { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string JoinCode { get; set; }
        [Required]
        [ReadOnly(true)]
        public DateTime DateJoined { get; set; }
        List<ServiceTime> ServiceTimes { get; set; } = new List<ServiceTime>();
        
        public string AdminId { get; set; }
        public virtual ApplicationUser Admin { get; set; }

        public Organization()
        {
            DateJoined = DateTime.UtcNow;
        }
    }

    public class ServiceTime
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
