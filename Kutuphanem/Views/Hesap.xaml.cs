using Kutuphanem.Helper;
using Kutuphanem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kutuphanem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hesap : ContentPage
    {
        readonly SqLiteManager _manager = new SqLiteManager();
        string _eKullAdi;
        public Hesap(int id)
        {
            InitializeComponent();
            Listele(id);
        }
        async void Listele(int k)
        {
            try
            {
                Kullanicilar kullanici = new Kullanicilar();
                kullanici = _manager.KullaniciBilgi(k);

                LblId.Text = kullanici.Id.ToString();
                TxtAd.Text = kullanici.Name;
                TxtSoyad.Text = kullanici.Surname;
                TxtKullaniciAdi.Text = kullanici.UserName;
                TxtSifre.Text = kullanici.Password;
                PkrGSorusu.SelectedItem = kullanici.GSorusu;
                TxtCevap.Text = kullanici.GCevap;
                _eKullAdi = kullanici.UserName;
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private async void btnGuncelle_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtKullaniciAdi.Text) == false && string.IsNullOrWhiteSpace(TxtSifre.Text) == false && string.IsNullOrWhiteSpace(TxtAd.Text) == false && string.IsNullOrWhiteSpace(TxtSoyad.Text) == false && string.IsNullOrWhiteSpace(TxtCevap.Text) == false)
                {
                    Kullanicilar kullanicilar1 = new Kullanicilar();
                    kullanicilar1 = _manager.KullaniciBilgi(Convert.ToInt32(LblId.Text));
                    if (TxtAd.Text != kullanicilar1.Name || TxtSoyad.Text != kullanicilar1.Surname || TxtKullaniciAdi.Text != kullanicilar1.UserName || TxtSifre.Text != kullanicilar1.Password || TxtCevap.Text != kullanicilar1.GCevap || PkrGSorusu.SelectedItem.ToString() != kullanicilar1.GSorusu)
                    {
                        List<Kullanicilar> kullanicilar = new List<Kullanicilar>();
                        if (TxtKullaniciAdi.Text != kullanicilar1.UserName)
                        {
                            kullanicilar = _manager.KullaniciAyniMi(TxtKullaniciAdi.Text.Trim()).ToList();
                        }
                        if (kullanicilar.Count == 0)
                        {
                            Kullanicilar k = new Kullanicilar();
                            k.Id = Convert.ToInt32(LblId.Text);
                            k.Name = TxtAd.Text.Trim();
                            k.Surname = TxtSoyad.Text.Trim();
                            k.UserName = TxtKullaniciAdi.Text.Trim();
                            k.Password = TxtSifre.Text.Trim();
                            k.GCevap = TxtCevap.Text.Trim();
                            k.GSorusu = PkrGSorusu.SelectedItem.ToString();
                            if (kullanicilar1.BeniHatirla == true)
                            {
                                k.BeniHatirla = true;
                            }
                            else
                            {
                                k.BeniHatirla = false;
                            }

                            int kayit = _manager.Duzenle(k);
                            _manager.Duzenle2(TxtKullaniciAdi.Text.Trim(), _eKullAdi);
                            if (kayit > 0)
                            {
                                DependencyService.Get<IToastMessage>().Mesaj("Kayıt düzenlendi.");
                                if (TxtKullaniciAdi.Text != kullanicilar1.UserName || TxtSifre.Text != kullanicilar1.Password)
                                {
                                    await DisplayAlert("Uyarı", "Şifreniz yada kullanıcı adınız değişti\nLütfen tekrar giriş yapınız.", "Tamam");
                                    await Navigation.PopToRootAsync();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Hata", "Kayıt düzenlenmedi.", "Tamam");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Uyarı", "Kullanıcıadı zaten kullanılıyor.", "Tamam");
                            TxtKullaniciAdi.Text = string.Empty;
                        }
                    }
                    else
                    {
                        DependencyService.Get<IToastMessage>().Mesaj("Değişiklik yapmadınız.");
                    }
                }
                else
                {
                    await DisplayAlert("Uyarı", "Lütfen tüm alanları doldurun!", "Tamam");
                    TxtKullaniciAdi.Focus();
                }
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private async void btnSil_Clicked(object sender, EventArgs e)
        {
            bool isOk = await DisplayAlert("Uyarı", "Hesabını ve kayıtlı kitaplarını kalıcı olarak silmek istediğine emin misin?", "Evet", "Hayır");
            if (isOk)
            {
                try
                {
                    int silindi = _manager.KullaniciSil(Convert.ToInt32(LblId.Text), TxtKullaniciAdi.Text);
                    if (silindi > 0)
                    {
                        await DisplayAlert("Bilgi", "Hesabın ve kayıtlı kitapların silindi.", "Tamam");
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Hesabın ve kayıtlı kitapların silinmedi.", "Tamam");
                    }
                }
                catch (Exception a)
                {
                    await DisplayAlert("Hata", a.Message, "Tamam");
                }
            }
        }
        private async void btnCikis_Clicked(object sender, EventArgs e)
        {
            bool isOk = await DisplayAlert("Uyarı", "Çıkış yapmak istediğine emin misin?", "Evet", "Hayır");
            if (isOk)
            {
                await Navigation.PopToRootAsync();
            }
        }
        private async void btnYedekAl_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Bilgi", "Yakında", "Tamam");
        }
    }
}