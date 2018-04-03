using PrayerCentral.Common.Interfaces;
using PrayerCentral.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace PrayerCentral
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
         
        public async void BtnClick_Clicked(object sender, EventArgs e)
        {
            LoginViewModel viewModel = new LoginViewModel();

            await viewModel.GetValue();
        }

        public void BtnLogin_Clicked(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
                clientId: "341901031941-gqbj2lqc6e5qn5u6asv1gsb48n1r0a5h.apps.googleusercontent.com",
                clientSecret: null,
                scope: "https://www.googleapis.com/auth/userinfo.email",
                authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                redirectUrl: new Uri("http://localhost/GG.PrayerCentral/api/home/googleauth"),
                accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token")
            );

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(auth);
        }
    }
}
