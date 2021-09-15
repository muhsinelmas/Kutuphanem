using Kutuphanem.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kutuphanem
{
    public partial class App : Application
    {
        public static string DbName { get; set; } = "Kutuphanem.db3";
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new GirisYap());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
