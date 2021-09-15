using Kutuphanem.Helper;
using Kutuphanem.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kutuphanem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Duzenle : ContentPage
    {
        public Duzenle(Kitap kitap)
        {
            InitializeComponent();

            LblId.Text = kitap.Id.ToString();
            TxtAd.Text = kitap.Ad;
            TxtYazar.Text = kitap.Yazar;
            TxtYayinevi.Text = kitap.YayinEvi;
            TxtTur.Text = kitap.Tur;
            TxtSayfa.Text = kitap.Sayfa.ToString();
            LblSahip.Text = kitap.Kullanici;
            Okundu.IsChecked = kitap.Okundu;
            if (kitap.Okundu)
            {
                LblOkundu.Text = "Okundu";
            }
            else
            {
                LblOkundu.Text = "Okunmadı";
            }
            Tarih.Date = kitap.Tarih;
            TxtNot.Text = kitap.Aciklama;
            ChkVar.IsChecked = kitap.BenimMi;
            if (kitap.BenimMi)
            {
                LblVar.Text = "Kütüphanede Var";
            }
            else
            {
                LblVar.Text = "Kütüphanede Yok";
            }
        }
        private async void OnClicked(object sender, EventArgs e)
        {
            SqLiteManager manager = new SqLiteManager();
            Kitap kitap = new Kitap();
            kitap.Id = Convert.ToInt32(LblId.Text);
            kitap.Ad = TxtAd.Text.Trim();
            kitap.Yazar = TxtYazar.Text.Trim();
            kitap.YayinEvi = TxtYayinevi.Text.Trim();
            if (!string.IsNullOrWhiteSpace(TxtTur.Text))
            {
                kitap.Tur = TxtTur.Text.Trim();
            }
            else
            {
                kitap.Tur = " ";
            }
            kitap.Sayfa = Convert.ToInt32(TxtSayfa.Text);
            kitap.Okundu = Okundu.IsChecked;
            kitap.Tarih = Tarih.Date;
            kitap.Aciklama = TxtNot.Text;
            kitap.Kullanici = LblSahip.Text;
            kitap.BenimMi = ChkVar.IsChecked;

            int isUpdated = manager.Update(kitap);
            if (isUpdated > 0)
            {
                await DisplayAlert("Bilgi", "Düzenlendi.", "Tamam");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Hata", "Düzenlenemedi", "Tamam");
            }
        }
        private void Okundu_Toggled(object sender, ToggledEventArgs e)
        {
            if (LblOkundu.Text == "Okunmadı")
            {
                LblOkundu.Text = "Okundu";
            }
            else if (LblOkundu.Text == "Okundu")
            {
                LblOkundu.Text = "Okunmadı";
            }
        }
        private void ChkVar_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (LblVar.Text == "Kütüphanede Yok")
            {
                LblVar.Text = "Kütüphanede Var";
            }
            else if (LblVar.Text == "Kütüphanede Var")
            {
                LblVar.Text = "Kütüphanede Yok";
            }
        }
    }
}