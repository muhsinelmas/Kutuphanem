using Kutuphanem.Helper;
using Kutuphanem.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kutuphanem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detaylar : ContentPage
    {
        Kitap _kitap1 = new Kitap();
        readonly SqLiteManager _manager = new SqLiteManager();
        readonly int _id;
        public Detaylar(Kitap kitap)
        {
            InitializeComponent();

            _id = kitap.Id;

            Styles();
        }
        protected override void OnAppearing()
        {
            Listele(_id);
            base.OnAppearing();
        }
        void Listele(int id)
        {
            _kitap1 = _manager.Get(id);
            LblAd.Text = _kitap1.Ad;
            LblYazar.Text = _kitap1.Yazar;
            LblYayinEvi.Text = _kitap1.YayinEvi;
            LblTur.Text = _kitap1.Tur;
            LblSayfa.Text = _kitap1.Sayfa.ToString();
            Okundu.IsChecked = _kitap1.Okundu;
            if (_kitap1.Okundu)
            {
                LblOkundu.Text = "Okundu";
            }
            else
            {
                LblOkundu.Text = "Okunmadı";
            }
            Tarih.Date = _kitap1.Tarih;
            LblNot.Text = _kitap1.Aciklama;
            ChkVar.IsChecked = _kitap1.BenimMi;
            if (_kitap1.BenimMi)
            {
                LblVar.Text = "Kütüphanede Var";
            }
            else
            {
                LblVar.Text = "Kütüphanede Yok";
            }
        }
        private void Styles()
        {
            LblAd.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblYazar.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblYayinEvi.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblTur.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblSayfa.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblNot.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

            LblAd1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblYazar1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblYayinEvi1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblTur1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblSayfa1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblNot1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            Tarih1.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblOkundu.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            LblVar.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

            Tarih.IsEnabled = false;
        }
        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Duzenle(_kitap1));
        }
    }
}