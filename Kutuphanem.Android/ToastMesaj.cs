using Android.Widget;
using Kutuphanem.Droid;
using Kutuphanem.Helper;
using System;


[assembly: Xamarin.Forms.Dependency(typeof(ToastMesaj))]
namespace Kutuphanem.Droid
{
    public class ToastMesaj : IToastMessage
    {
        public void Mesaj(string msg)
        {
            Toast.MakeText(Android.App.Application.Context, msg, ToastLength.Short)?.Show();
        }
    }
}