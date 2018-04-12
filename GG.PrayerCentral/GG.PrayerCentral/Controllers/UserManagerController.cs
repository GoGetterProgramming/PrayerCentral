using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GG.PrayerCentral.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GG.PrayerCentral.Controllers
{
    [Route("api/[controller]")]
    public class UserManagerController : ControllerBase
    {
        private readonly ApplicationDBContext _DbContext;

        public UserManagerController(ApplicationDBContext context)
        {
            _DbContext = context;
        }


    }
}