using Kutuphanem.Helper;
using Kutuphanem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Kutuphanem
{
    public partial class MainPage : ContentPage
    {
        public MainPage(string kullAdi)
        {
            InitializeComponent();
            Tarih.MaximumDate = DateTime.Now.Date;
            LblKullaniciAdi.Text = kullAdi;
        }
        protected override void OnAppearing()
        {
            TxtAd.Focus();
        }
        private async void OnClicked(object sender, EventArgs e)
        {
            SqLiteManager manager = new SqLiteManager();

            if (String.IsNullOrWhiteSpace(TxtAd.Text) == false && String.IsNullOrWhiteSpace(TxtYazar.Text) == false)
            {
                List<Kitap> kitapList = new List<Kitap>();
                kitapList = manager.AyniMi(TxtAd.Text.Trim(), TxtYazar.Text.Trim(), LblKullaniciAdi.Text).ToList();
                if (kitapList.Count == 0)
                {
                    Kitap kitap = new Kitap();
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
                    if (String.IsNullOrWhiteSpace(TxtSayfa.Text) == false)
                    {
                        kitap.Sayfa = Convert.ToInt32(TxtSayfa.Text);
                    }
                    else
                    {
                        kitap.Sayfa = 0;
                    }
                    kitap.Okundu = Okundu.IsChecked;
                    kitap.Tarih = Tarih.Date;
                    kitap.Aciklama = TxtNot.Text;
                    kitap.Kullanici = LblKullaniciAdi.Text;
                    if (Okundu.IsChecked == true)
                    {
                        kitap.BenimMi = ChkVar.IsChecked;
                    }
                    else
                    {
                        kitap.BenimMi = true;
                    }

                    int isInserted = manager.Insert(kitap);
                    if (isInserted > 0)
                    {
                        string mesaj = kitap.Ad + " eklendi.";
                        DependencyService.Get<IToastMessage>().Mesaj(mesaj);
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Kaydedilmedi.", "Tamam");
                    }
                    Temizle();
                }
                else
                {
                    await DisplayAlert("Uyarı", "Aynı kayıt mevcut. Farklı bir kayıt girin.", "Tamam");
                    Temizle();
                }
            }
            else
            {
                await DisplayAlert("Uyarı", "Kitap adı ve yazarı dolu olmalı!", "Tamam");
                Temizle();
            }
        }
        private void Okundu_Toggled(object sender, ToggledEventArgs e)
        {
            ChkVar.IsVisible = e.Value;
            LblVar.IsVisible = e.Value;
            if (LblOkundu.Text == "Okunmadı")
            {
                LblOkundu.Text = "Okundu";
            }
            else if (LblOkundu.Text == "Okundu")
            {
                LblOkundu.Text = "Okunmadı";
            }
        }
        void Temizle()
        {
            TxtAd.Text = string.Empty;
            TxtYazar.Text = string.Empty;
            TxtYayinevi.Text = string.Empty;
            TxtTur.Text = string.Empty;
            TxtSayfa.Text = string.Empty;
            Okundu.IsChecked = false;
            LblOkundu.Text = "Okunmadı";
            Tarih.Date = DateTime.Now.Date;
            TxtNot.Text = string.Empty;
            TxtAd.Focus();
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
