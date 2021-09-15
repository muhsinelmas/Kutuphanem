using Kutuphanem.Helper;
using Kutuphanem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Kutuphanem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listele : ContentPage
    {
        private SqLiteManager _manager;
        private string _filtre;
        private IList<Kitap> _kitapList;

        public Listele(int id, string kullaniciAdi)
        {
            InitializeComponent();

            PkrAra.SelectedItem = "Ad";
            PkrFiltre.SelectedItem = PkrFiltre.Items[0];
            LblId.Text = id.ToString();
            LblKullaniciAdi.Text = kullaniciAdi;

            LstKitap.RefreshCommand = new Command(() =>
            {
                Yenile();
                PkrTurFiltreDoldur();
                LstKitap.IsRefreshing = false;
            });
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Çıkış", "Çıkmak istediğine emin misin?", "Evet", "Hayır"))
                {
                    base.OnBackButtonPressed();

                    await Navigation.PopToRootAsync();
                }
            });

            return true;
        }
        protected override void OnAppearing()
        {
            Yenile();
            KulAdiGetir();
            PkrTurFiltreDoldur();
            PkrFiltre.SelectedItem = PkrFiltre.Items[0];
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void Eklediginde(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(LblKullaniciAdi.Text));
        }
        private void Yenile(object sender, EventArgs e)
        {
            Yenile();
            PkrTurFiltreDoldur();
        }
        private async void Sil(object sender, EventArgs e)
        {
            var selected = (MenuItem)sender;

            bool isOk = await DisplayAlert("Uyarı", "Silmek istediğine emin misin?", "Evet", "Hayır");
            if (isOk)
            {
                int isDeleted = _manager.Delete(Convert.ToInt32(selected.CommandParameter.ToString()));
                if (isDeleted > 0)
                {
                    //await DisplayAlert("Başarılı", "Kayıt silindi.", "Tamam");
                    Yenile();
                    PkrTurFiltre.SelectedItem = PkrTurFiltre.Items[0];
                    PkrTurFiltreDoldur();
                }
                else
                {
                    await DisplayAlert("Başarısız", "Kayıt silinemedi.", "Tamam");
                }
            }
        }
        private void Yenile()
        {
            _kitapList = new ObservableCollection<Kitap>();
            _kitapList.Clear();
            _manager = new SqLiteManager();
            _kitapList = _manager.GetAll(LblKullaniciAdi.Text).ToList();

            if (_kitapList.Count == 0)
            {
                LstKitap.IsVisible = false;
                LblUyari.IsVisible = true;
                Kutu1.IsVisible = true;
                Bos.Padding = new Thickness(10, 5);
                Kutu2.IsVisible = true;
            }
            else
            {
                LstKitap.IsVisible = true;
                LblUyari.IsVisible = false;
                Kutu1.IsVisible = false;
                Kutu2.IsVisible = false;
                Bos.Padding = 0;
                LstKitap.BindingContext = _kitapList;

                TxtAra.Text = "";
            }
            PkrFiltre.SelectedItem = PkrFiltre.Items[0];
        }
        private async void Duzenle(object sender, EventArgs e)
        {
            var selected = (MenuItem)sender;
            var kitap = _manager.Get(Convert.ToInt32(selected.CommandParameter.ToString()));

            await Navigation.PushAsync(new Duzenle(kitap));
        }
        private async void Istatistikler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Istatistikler(LblKullaniciAdi.Text));
        }
        private void txtAra_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                Yenile();
            }
            else
            {
                _kitapList = new List<Kitap>();
                _manager = new SqLiteManager();
                _kitapList.Clear();
                _kitapList = _manager.Search(e.NewTextValue, _filtre, LblKullaniciAdi.Text).ToList();

                LstKitap.BindingContext = _kitapList;
            }
        }
        private void txtAra_Completed(object sender, EventArgs e)
        {
            if (TxtAra.Text == "")
            {
                Yenile();
            }
            else
            {
                _kitapList = new List<Kitap>();
                _manager = new SqLiteManager();
                _kitapList.Clear();
                _kitapList = _manager.Search(TxtAra.Text, _filtre, LblKullaniciAdi.Text).ToList();

                LstKitap.BindingContext = _kitapList;
            }
        }
        private void pkrAra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PkrAra.SelectedItem.ToString() == "Yayınevi")
            {
                _filtre = "YayinEvi";
            }
            else if (PkrAra.SelectedItem.ToString() == "Tür")
            {
                _filtre = "Tur";
            }
            else
            {
                _filtre = PkrAra.SelectedItem.ToString();
            }
        }
        private async void CikisYap_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Hesap(Convert.ToInt32(LblId.Text)));
        }
        private async void KulAdiGetir()
        {
            try
            {
                Kullanicilar kullanicilar = new Kullanicilar();
                kullanicilar = _manager.HesapAd(LblKullaniciAdi.Text);
                BtnHesap.Text = kullanicilar.Name;
            }
            catch (Exception a)
            {
                await DisplayAlert("Hata", a.Message, "Tamam");
            }
        }
        private async void lstKitap_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = (Kitap)e.Item;
            await Navigation.PushAsync(new Detaylar(selected));
        }
        private void pkrFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            PkrTurFiltre.SelectedItem = "Tümü";

            if (PkrFiltre.SelectedItem.ToString() == "Tümü")
            {
                _kitapList = new List<Kitap>();
                _manager = new SqLiteManager();
                _kitapList.Clear();
                _kitapList = _manager.GetAll(LblKullaniciAdi.Text).ToList();

                if (_kitapList.Count == 0)
                {
                    LstKitap.IsVisible = false;
                    LblUyari.IsVisible = true;
                    LblUyari.Text = "Kütüphanen boş";
                    Kutu1.IsVisible = true;
                    Bos.Padding = new Thickness(10, 5);
                    Kutu2.IsVisible = true;
                }
                else
                {
                    LstKitap.IsVisible = true;
                    LblUyari.IsVisible = false;
                    Kutu1.IsVisible = false;
                    Kutu2.IsVisible = false;
                    Bos.Padding = 0;
                    LstKitap.BindingContext = _kitapList;
                }
            }
            else if (PkrFiltre.SelectedItem.ToString() == "Okundu")
            {
                _kitapList = new List<Kitap>();
                _manager = new SqLiteManager();
                _kitapList.Clear();
                _kitapList = _manager.FiltreyeGoreGetir(true, LblKullaniciAdi.Text).ToList();

                if (_kitapList.Count == 0)
                {
                    LstKitap.IsVisible = false;
                    LblUyari.IsVisible = true;
                    LblUyari.Text = "Okunmuş kitabın yok.";
                    Kutu1.IsVisible = true;
                    Bos.Padding = new Thickness(10, 5);
                    Kutu2.IsVisible = true;
                }
                else
                {
                    LstKitap.IsVisible = true;
                    LblUyari.IsVisible = false;
                    Kutu1.IsVisible = false;
                    Kutu2.IsVisible = false;
                    Bos.Padding = 0;
                    LstKitap.BindingContext = _kitapList;
                }
            }
            else if (PkrFiltre.SelectedItem.ToString() == "Okunmadı")
            {
                _kitapList = new List<Kitap>();
                _manager = new SqLiteManager();
                _kitapList.Clear();
                _kitapList = _manager.FiltreyeGoreGetir(false, LblKullaniciAdi.Text).ToList();

                if (_kitapList.Count == 0)
                {
                    LstKitap.IsVisible = false;
                    LblUyari.IsVisible = true;
                    LblUyari.Text = "Okunmamış kitabın yok.";
                    Kutu1.IsVisible = true;
                    Bos.Padding = new Thickness(10, 5);
                    Kutu2.IsVisible = true;
                }
                else
                {
                    LstKitap.IsVisible = true;
                    LblUyari.IsVisible = false;
                    Kutu1.IsVisible = false;
                    Kutu2.IsVisible = false;
                    Bos.Padding = 0;
                    LstKitap.BindingContext = _kitapList;
                }
            }
        }
        private void PkrTurFiltreDoldur()
        {
            _kitapList = new ObservableCollection<Kitap>();
            _kitapList.Clear();
            PkrTurFiltre.Items.Clear();
            PkrTurFiltre.Items.Add("Tümü");
            _manager = new SqLiteManager();
            _kitapList = _manager.Turler(LblKullaniciAdi.Text).ToList();

            PkrTurFiltre.SelectedItem = PkrTurFiltre.Items[0];

            foreach (var item in _kitapList)
            {
                if (item.Tur != " ")
                {
                    PkrTurFiltre.Items.Add(item.Tur);
                }
                else
                {
                    PkrTurFiltre.Items.Add("Kategorilendirilmemiş");
                }
            }
        }
        private void pkrTurFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PkrTurFiltre.Items.Count > 1)
            {
                if (PkrTurFiltre.SelectedItem.ToString() != "Kategorilendirilmemiş")
                {
                    if (PkrFiltre.SelectedItem.ToString() == "Tümü" && PkrTurFiltre.SelectedItem.ToString() == "Tümü")
                    {
                        _kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        _kitapList.Clear();
                        _kitapList = _manager.GetAll(LblKullaniciAdi.Text).ToList();

                        if (_kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Kütüphanen boş";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = _kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Tümü" && PkrTurFiltre.SelectedItem.ToString() != "Tümü")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.TureGoreButunKitaplariGetir(LblKullaniciAdi.Text, PkrTurFiltre.SelectedItem.ToString()).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Kütüphanen boş";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Okundu" && PkrTurFiltre.SelectedItem.ToString() == "Tümü")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.FiltreyeGoreGetir(true, LblKullaniciAdi.Text).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Okunmuş " + PkrTurFiltre.SelectedItem.ToString() + " kitabın yok.";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Okundu" && PkrTurFiltre.SelectedItem.ToString() != "Tümü")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.TureGoreOkunanKitaplariGetir(LblKullaniciAdi.Text, PkrTurFiltre.SelectedItem.ToString(), true).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Okunmuş " + PkrTurFiltre.SelectedItem.ToString() + " kitabın yok.";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Okunmadı" && PkrTurFiltre.SelectedItem.ToString() == "Tümü")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.FiltreyeGoreGetir(false, LblKullaniciAdi.Text).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Okunmamış " + PkrTurFiltre.SelectedItem.ToString() + " kitabın yok.";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Okunmadı" && PkrTurFiltre.SelectedItem.ToString() != "Tümü")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.TureGoreOkunanKitaplariGetir(LblKullaniciAdi.Text, PkrTurFiltre.SelectedItem.ToString(), false).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Okunmamış " + PkrTurFiltre.SelectedItem.ToString() + " kitabın yok.";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                }
                else
                {
                    if (PkrFiltre.SelectedItem.ToString() == "Tümü" && PkrTurFiltre.SelectedItem.ToString() != "Tümü")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.TureGoreButunKitaplariGetir(LblKullaniciAdi.Text, " ").ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Kütüphanen boş";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Okundu")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.TureGoreOkunanKitaplariGetir(LblKullaniciAdi.Text, " ", true).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Okunmuş Kategorilendirilmemiş kitabın yok.";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                    if (PkrFiltre.SelectedItem.ToString() == "Okunmadı")
                    {
                        List<Kitap> kitapList = new List<Kitap>();
                        _manager = new SqLiteManager();
                        kitapList = _manager.TureGoreOkunanKitaplariGetir(LblKullaniciAdi.Text, " ", false).ToList();

                        if (kitapList.Count == 0)
                        {
                            LstKitap.IsVisible = false;
                            LblUyari.IsVisible = true;
                            LblUyari.Text = "Okunmamış Kategorilendirilmemiş kitabın yok.";
                            Kutu1.IsVisible = true;
                            Bos.Padding = new Thickness(10, 5);
                            Kutu2.IsVisible = true;
                        }
                        else
                        {
                            LstKitap.IsVisible = true;
                            LblUyari.IsVisible = false;
                            Kutu1.IsVisible = false;
                            Kutu2.IsVisible = false;
                            Bos.Padding = 0;
                            LstKitap.BindingContext = kitapList;
                        }
                    }
                }
            }
        }
        private async void Expndr_Tapped(object sender, EventArgs e)
        {
            if (Expndr.IsExpanded == true)
            {
                await ImgGenislet.RotateTo(180, 200, Easing.Linear);
            }
            else
            {
                await ImgGenislet.RotateTo(0, 200, Easing.Linear);
            }
        }
    }
}