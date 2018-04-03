using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PrayerCentral.Common.Interfaces;
//using Xamarin.Auth;

namespace PrayerCentral.Droid
{
    public class DroidAuthentication : IAuthentication
    {
        private static Context s_Context;

        public static void Init(Context context)
        {
            s_Context = context;
        }

        public DroidAuthentication()
        {
            if (s_Context == null)
            {
                throw new Exception("Init must be called before requesting authentication");
            }
        }

        public void GoogleAuthenticate()
        {
            //var auth = new OAuth2Authenticator(
            //    clientId: "341901031941 - sqsp8qf1886ecbtglshefahbtvde8kom.apps.googleusercontent.com",
            //    clientSecret: "CmmsrY2_VYDwn-UCNl55JbEr",
            //    scope: "openid email",
            //    authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
            //    redirectUrl: new Uri("http://localhost/GG.PrayerCentral/api/home/googleauth"),
            //    accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token")
            //);

            //s_Context.StartActivity(auth.GetUI(s_Context));

            //auth.Completed += (sender, e) => {
            //    Console.WriteLine(e.IsAuthenticated);
            //};
        }
    }
}