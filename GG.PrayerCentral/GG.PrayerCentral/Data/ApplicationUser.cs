using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;

namespace GG.PrayerCentral.Data
{
    public enum UserStatus
    {
        Online,
        Offline
    }

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ReadOnly(true)]
        public DateTime DateJoined { get; set; }

        public ApplicationUser()
        {
            DateJoined = DateTime.UtcNow;
        }
    }
}
