using Kutuphanem.Helper;
using Kutuphanem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kutuphanem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GirisYap : ContentPage
    {
        SqLiteManager _manager = new SqLiteManager();
        List<Kullanicilar> _kullanicilar1 = new List<Kullanicilar>();
        public GirisYap()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            PkrGSorusu.SelectedItem = PkrGSorusu.Items[0];

            var lblSifremiUnuttumClick = new TapGestureRecognizer();
            lblSifremiUnuttumClick.Tapped += async (s, e) =>
            {
                LblSifremiUnuttum.TextColor = Color.White;
                await LblSifremiUnuttum.ScaleTo(0.86, 100, Easing.Linear);
                await LblSifremiUnuttum.ScaleTo(1, 100, Easing.Linear);
                LblSifremiUnuttum.TextColor = Color.Black;
                if (string.IsNullOrWhiteSpace(TxtKullaniciAdi.Text) == false)
                {
                    _kullanicilar1 = _manager.KullaniciAyniMi(TxtKullaniciAdi.Text.Trim()).ToList();
                    if (_kullanicilar1.Count > 0)
                    {
                        await ScrView.FadeTo(0, 300, Easing.Linear);
                        ScrView.IsVisible = false;
                        ViewSifremiUnuttum.IsVisible = true;

                        LblSoru.Text = "Soru: " + _kullanicilar1[0].GSorusu;

                        ViewSifremiUnuttum.Opacity = 0;
                        await Task.WhenAny<bool>(ViewSifremiUnuttum.FadeTo(1, 700, Easing.Linear), ViewSifremiUnuttum.ScaleTo(1.3, 100, Easing.CubicIn));
                        await ViewSifremiUnuttum.ScaleTo(1, 100, Easing.CubicOut);
                    }
                    else
                    {
                        await DisplayAlert("Uyarı", "Kullanıcı adı hatalı.", "Tamam");
                        TxtKullaniciAdi.Focus();
                    }
                }
                else
                {
                    await TxtKullaniciAdi.RotateTo(5, 10);
                    await TxtKullaniciAdi.RotateTo(-5, 10);
                    await TxtKullaniciAdi.RotateTo(0, 10);
                    TxtKullaniciAdi.Text = string.Empty;
                    TxtKullaniciAdi.Focus();
                    TxtKullaniciAdi.PlaceholderColor = Color.Red;
                    await Task.Delay(1000);
                    TxtKullaniciAdi.PlaceholderColor = default;
                }
            };
            LblSifremiUnuttum.GestureRecognizers.Add(lblSifremiUnuttumClick);

            var lblGSorusuClick = new TapGestureRecognizer();
            lblGSorusuClick.Tapped += async (s, e) =>
            {
                await DisplayAlert("Bilgi", "Şifrenizi unutmanız durumunda şifrenizi bu sorunun cevabıyla öğreneceksiniz.", "Tamam");
            };
            LblGSorusu.GestureRecognizers.Add(lblGSorusuClick);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                BeniHatirlaGelen();

                int sayi = _manager.KullaniciSayisi();
                if (sayi == 0)
                {
                    BtnGirisYap.IsEnabled = false;
                }
                else
                {
                    BtnGirisYap.IsEnabled = true;
                }

                TxtKullaniciAdi.Text = string.Empty;
                TxtSifre.Text = string.Empty;
                ChkBeniHatirla.IsChecked = false;
                TxtKullaniciAdi.Focus();
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private async void btnKaydol_Clicked(object sender, EventArgs e)
        {
            if (_manager.KullaniciSayisi() < 4)
            {
                Temizle();
                BeniHatirlaButon.IsVisible = false;
                await GrdGiris.FadeTo(0, 500, Easing.Linear);
                GrdGiris.IsVisible = false;
                GrdKayit.IsVisible = true;
                await GrdKayit.FadeTo(1, 500, Easing.Linear);
                BeniHatirlaButon.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Uyarı", "Maksimum kullanıcı sayısına ulaşıldı.", "Tamam");
            }
        }
        private async void btnGirisYap_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtKullaniciAdi.Text) == false && string.IsNullOrWhiteSpace(TxtSifre.Text) == false)
                {
                    List<Kullanicilar> kull = new List<Kullanicilar>();
                    kull = _manager.GirisYaptiMi(TxtKullaniciAdi.Text.Trim(), TxtSifre.Text.Trim()).ToList();

                    if (kull.Count > 0)
                    {
                        if (ChkBeniHatirla.IsChecked == true)
                        {
                            _manager.BeniHatirla(TxtKullaniciAdi.Text.Trim(), true);
                        }
                        else
                        {
                            _manager.BeniHatirla(TxtKullaniciAdi.Text.Trim(), false);
                        }
                        Kullanicilar kullanici = new Kullanicilar();
                        kullanici = _manager.KullaniciBilgiKullaniciadi(TxtKullaniciAdi.Text.Trim());
                        await Navigation.PushAsync(new Listele(kullanici.Id, TxtKullaniciAdi.Text.Trim()));
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Kullanıcı adı veya şifre hatalı.", "Tamam");
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(TxtKullaniciAdi.Text) && !string.IsNullOrWhiteSpace(TxtSifre.Text))
                    {
                        await TxtKullaniciAdi.RotateTo(5, 10);
                        await TxtKullaniciAdi.RotateTo(-5, 20);
                        await TxtKullaniciAdi.RotateTo(0, 10);
                        TxtKullaniciAdi.Text = string.Empty;
                        TxtKullaniciAdi.Focus();
                        TxtKullaniciAdi.PlaceholderColor = Color.Red;
                        await Task.Delay(1000);
                        TxtKullaniciAdi.PlaceholderColor = default;
                    }
                    else if (string.IsNullOrWhiteSpace(TxtSifre.Text) && !string.IsNullOrWhiteSpace(TxtKullaniciAdi.Text))
                    {
                        await TxtSifre.RotateTo(5, 10);
                        await TxtSifre.RotateTo(-5, 20);
                        await TxtSifre.RotateTo(0, 10);
                        TxtSifre.Text = string.Empty;
                        TxtSifre.Focus();
                        TxtSifre.PlaceholderColor = Color.Red;
                        await Task.Delay(1000);
                        TxtSifre.PlaceholderColor = default;
                    }
                    else
                    {
                        await TxtKullaniciAdi.RotateTo(5, 10);
                        await TxtKullaniciAdi.RotateTo(-5, 20);
                        await TxtKullaniciAdi.RotateTo(0, 10);
                        await TxtSifre.RotateTo(5, 10);
                        await TxtSifre.RotateTo(-5, 20);
                        await TxtSifre.RotateTo(0, 10);
                        TxtKullaniciAdi.Text = string.Empty;
                        TxtSifre.Text = string.Empty;
                        TxtKullaniciAdi.Focus();
                        TxtKullaniciAdi.PlaceholderColor = Color.Red;
                        TxtSifre.PlaceholderColor = Color.Red;
                        await Task.Delay(1000);
                        TxtKullaniciAdi.PlaceholderColor = default;
                        TxtSifre.PlaceholderColor = default;
                    }
                }
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private async void btnKaydol1_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtKullaniciAdi1.Text) == false && string.IsNullOrWhiteSpace(TxtSifre1.Text) == false && string.IsNullOrWhiteSpace(TxtAd.Text) == false && string.IsNullOrWhiteSpace(TxtSoyad.Text) == false && string.IsNullOrWhiteSpace(TxtCevap.Text) == false)
                {
                    List<Kullanicilar> kullanicilar = new List<Kullanicilar>();

                    if (_manager.KullaniciSayisi() == 0)
                    {
                        _manager.Duzenle1(TxtKullaniciAdi1.Text);
                    }

                    kullanicilar = _manager.KullaniciAyniMi(TxtKullaniciAdi1.Text.Trim()).ToList();
                    if (kullanicilar.Count == 0)
                    {
                        Kullanicilar k = new Kullanicilar();
                        k.Name = TxtAd.Text.Trim();
                        k.Surname = TxtSoyad.Text.Trim();
                        k.UserName = TxtKullaniciAdi1.Text.Trim();
                        k.Password = TxtSifre1.Text.Trim();
                        k.GSorusu = PkrGSorusu.SelectedItem.ToString();
                        k.GCevap = TxtCevap.Text;

                        int kayit = _manager.KullaniciEkle(k);
                        if (kayit > 0)
                        {
                            DependencyService.Get<IToastMessage>().Mesaj(TxtKullaniciAdi1.Text + " eklendi.");
                            await GrdKayit.FadeTo(0, 300, Easing.Linear);
                            GrdKayit.IsVisible = false;
                            GrdGiris.IsVisible = true;
                            await GrdGiris.FadeTo(1, 300, Easing.Linear);
                            BeniHatirlaButon.IsVisible = true;
                            BtnGirisYap.IsEnabled = true;
                        }
                        else
                        {
                            await DisplayAlert("Hata", "Kayıt eklenemedi.", "Tamam");
                        }
                    }
                    else
                    {
                        await TxtKullaniciAdi1.RotateTo(5, 10);
                        await TxtKullaniciAdi1.RotateTo(-5, 10);
                        await TxtKullaniciAdi1.RotateTo(5, 10);
                        await TxtKullaniciAdi1.RotateTo(0, 10);
                        TxtKullaniciAdi1.Text = string.Empty;
                    }
                }
                else
                {
                    KaydolAnimasyon();
                }
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private void Temizle()
        {
            TxtAd.Text = string.Empty;
            TxtSoyad.Text = string.Empty;
            TxtKullaniciAdi1.Text = string.Empty;
            TxtSifre1.Text = string.Empty;
            TxtCevap.Text = string.Empty;
            PkrGSorusu.SelectedItem = PkrGSorusu.Items[0];
        }
        private async void btnVazgec_Clicked(object sender, EventArgs e)
        {
            await GrdKayit.FadeTo(0, 300, Easing.Linear);
            GrdKayit.IsVisible = false;
            GrdGiris.IsVisible = true;
            BeniHatirlaButon.IsVisible = true;
            await GrdGiris.FadeTo(1, 300, Easing.Linear);
            BeniHatirlaButon.IsVisible = true;
        }
        private void imgSifre_Clicked(object sender, EventArgs e)
        {
            if (TxtSifre.IsPassword == true)
            {
                ImgSifre.Source = "hidePassword";
                TxtSifre.IsPassword = false;
            }
            else
            {
                ImgSifre.Source = "showPassword";
                TxtSifre.IsPassword = true;
            }
        }
        private void imgSifre1_Clicked(object sender, EventArgs e)
        {
            if (TxtSifre1.IsPassword == true)
            {
                ImgSifre1.Source = "hidePassword";
                TxtSifre1.IsPassword = false;
            }
            else
            {
                ImgSifre1.Source = "showPassword";
                TxtSifre1.IsPassword = true;
            }
        }
        private void txtSifre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSifre.Text.Length > 0)
            {
                ImgSifre.IsVisible = true;
            }
            else
            {
                ImgSifre.IsVisible = false;
            }
        }
        private void txtSifre1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSifre1.Text.Length > 0)
            {
                ImgSifre1.IsVisible = true;
            }
            else
            {
                ImgSifre1.IsVisible = false;
            }
        }
        private async void BeniHatirlaGelen()
        {
            try
            {
                _kullanicilar1 = _manager.BeniHatirlaGetir().ToList();

                if (_kullanicilar1.Count == 0)
                {
                    Stk1.IsVisible = false;
                    Stk2.IsVisible = false;
                    Stk3.IsVisible = false;
                    Stk4.IsVisible = false;
                }
                else if (_kullanicilar1.Count == 1)
                {
                    Stk1.IsVisible = true;
                    BhButon1.Text = _kullanicilar1[0].Name.Substring(0, 1) + _kullanicilar1[0].Surname.Substring(0, 1);
                    Stk1.BindingContext = _kullanicilar1[0];
                    Stk2.IsVisible = false;
                    Stk3.IsVisible = false;
                    Stk4.IsVisible = false;
                }
                else if (_kullanicilar1.Count == 2)
                {
                    Stk1.IsVisible = true;
                    Stk2.IsVisible = true;
                    BhButon1.Text = _kullanicilar1[0].Name.Substring(0, 1) + _kullanicilar1[0].Surname.Substring(0, 1);
                    BhButon2.Text = _kullanicilar1[1].Name.Substring(0, 1) + _kullanicilar1[1].Surname.Substring(0, 1);
                    Stk1.BindingContext = _kullanicilar1[0];
                    Stk2.BindingContext = _kullanicilar1[1];
                    Stk3.IsVisible = false;
                    Stk4.IsVisible = false;
                }
                else if (_kullanicilar1.Count == 3)
                {
                    Stk1.IsVisible = true;
                    Stk2.IsVisible = true;
                    Stk3.IsVisible = true;
                    BhButon1.Text = _kullanicilar1[0].Name.Substring(0, 1) + _kullanicilar1[0].Surname.Substring(0, 1);
                    BhButon2.Text = _kullanicilar1[1].Name.Substring(0, 1) + _kullanicilar1[1].Surname.Substring(0, 1);
                    BhButon3.Text = _kullanicilar1[2].Name.Substring(0, 1) + _kullanicilar1[2].Surname.Substring(0, 1);
                    Stk1.BindingContext = _kullanicilar1[0];
                    Stk2.BindingContext = _kullanicilar1[1];
                    Stk3.BindingContext = _kullanicilar1[2];
                    Stk4.IsVisible = false;
                }
                else if (_kullanicilar1.Count == 4)
                {
                    Stk1.IsVisible = true;
                    Stk2.IsVisible = true;
                    Stk3.IsVisible = true;
                    Stk4.IsVisible = true;
                    BhButon1.Text = _kullanicilar1[0].Name.Substring(0, 1) + _kullanicilar1[0].Surname.Substring(0, 1);
                    BhButon2.Text = _kullanicilar1[1].Name.Substring(0, 1) + _kullanicilar1[1].Surname.Substring(0, 1);
                    BhButon3.Text = _kullanicilar1[2].Name.Substring(0, 1) + _kullanicilar1[2].Surname.Substring(0, 1);
                    BhButon4.Text = _kullanicilar1[3].Name.Substring(0, 1) + _kullanicilar1[3].Surname.Substring(0, 1);
                    Stk1.BindingContext = _kullanicilar1[0];
                    Stk2.BindingContext = _kullanicilar1[1];
                    Stk3.BindingContext = _kullanicilar1[2];
                    Stk4.BindingContext = _kullanicilar1[3];
                }
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private async void BHButon1_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Kullanicilar kullanici = new Kullanicilar();
            kullanici = _manager.KullaniciBilgiKullaniciadi(LblButon1.Text);
            await Navigation.PushAsync(new Listele(kullanici.Id, LblButon1.Text));
        }
        private async void BHButon2_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Kullanicilar kullanici = new Kullanicilar();
            kullanici = _manager.KullaniciBilgiKullaniciadi(LblButon2.Text);
            await Navigation.PushAsync(new Listele(kullanici.Id, LblButon2.Text));
        }
        private async void BHButon3_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Kullanicilar kullanici = new Kullanicilar();
            kullanici = _manager.KullaniciBilgiKullaniciadi(LblButon3.Text);
            await Navigation.PushAsync(new Listele(kullanici.Id, LblButon3.Text));
        }
        private async void BHButon4_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Kullanicilar kullanici = new Kullanicilar();
            kullanici = _manager.KullaniciBilgiKullaniciadi(LblButon4.Text);
            await Navigation.PushAsync(new Listele(kullanici.Id, LblButon4.Text));
        }
        private async void btnTamam_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtCevap1.Text))
            {
                if (TxtCevap1.Text == _kullanicilar1[0].GCevap)
                {
                    LblSonuc.Text = "Şifreniz: " + _kullanicilar1[0].Password;
                }
                else
                {
                    LblSonuc.Text = "Cevabınız hatalı!";
                }
            }
            else
            {
                await TxtCevap1.RotateTo(5, 10);
                await TxtCevap1.RotateTo(-5, 10);
                await TxtCevap1.RotateTo(0, 10);
            }
        }
        private async void btnIptal_Clicked(object sender, EventArgs e)
        {
            await ViewSifremiUnuttum.FadeTo(0, 300, Easing.Linear);
            ViewSifremiUnuttum.IsVisible = false;
            ScrView.IsVisible = true;
            await ScrView.FadeTo(1, 400, Easing.Linear);
        }
        async void KaydolAnimasyon()
        {
            //await Task.WhenAny<bool>(
            //    txtAd.RotateTo(2, 5), txtAd.RotateTo(-2, 10), txtAd.RotateTo(2, 5),
            //    txtSoyad.RotateTo(2, 5), txtSoyad.RotateTo(-2, 10), txtSoyad.RotateTo(2, 5),
            //    txtKullaniciAdi1.RotateTo(2, 5), txtKullaniciAdi1.RotateTo(-2, 10), txtKullaniciAdi1.RotateTo(2, 5),
            //    txtSifre1.RotateTo(2, 5), txtSifre1.RotateTo(-2, 10), txtSifre1.RotateTo(2, 5),
            //    txtCevap.RotateTo(2, 5), txtCevap.RotateTo(-2, 10), txtCevap.RotateTo(2, 5));

            //await txtAd.RotateTo(0, 5);
            //await txtSoyad.RotateTo(0, 5);
            //await txtKullaniciAdi1.RotateTo(0, 5);
            //await txtSifre1.RotateTo(0, 5);
            //await txtCevap.RotateTo(0, 5);

            TxtAd.PlaceholderColor = Color.Red;
            TxtSoyad.PlaceholderColor = Color.Red;
            TxtKullaniciAdi1.PlaceholderColor = Color.Red;
            TxtSifre1.PlaceholderColor = Color.Red;
            TxtCevap.PlaceholderColor = Color.Red;
            await Task.Delay(1000);
            TxtAd.PlaceholderColor = Color.Black;
            TxtSoyad.PlaceholderColor = Color.Black;
            TxtKullaniciAdi1.PlaceholderColor = Color.Black;
            TxtSifre1.PlaceholderColor = Color.Black;
            TxtCevap.PlaceholderColor = Color.Black;
        }
    }
}