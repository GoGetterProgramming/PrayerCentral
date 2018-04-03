using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GG.PrayerCentral.Data
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Website { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
