using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Request.OrganizationData
{
    public class CreateOrganizationData
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
    }
}
