using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using GG.DependencyInjection;
using PrayerCentral.Common.Interfaces;

namespace PrayerCentral.Droid
{
    [Activity(Label = "PrayerCentral", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            DroidAuthentication.Init(this);

            ITypeFactory typeFactory = new TypeFactory();
            typeFactory.Register<IAuthentication>(() => new DroidAuthentication());

            global::Xamarin.Forms.Forms.Init(this, bundle);

            try
            {
                LoadApplication(new App(typeFactory));
            }
            catch (Exception e)
            {

            }
        }
    }
}

