using PrayerCentral.Model;
using System;
using System.Threading.Tasks;

namespace PrayerCentral.ViewModel
{
    public class LoginViewModel
    {
        public async Task GetValue()
        {
            HttpContext httpContext = ModelFactory.Container.Resolve<HttpContext>();

            string httpContent = await httpContext.GetAsync("home");

            Console.WriteLine(httpContent);
        }
    }
}
