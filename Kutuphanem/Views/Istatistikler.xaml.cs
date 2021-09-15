using Kutuphanem.Helper;
using Microcharts;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using System.Collections.Generic;
using SkiaSharp;
using Kutuphanem.Models;
using System.Linq;
using System.Collections;

namespace Kutuphanem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Istatistikler : ContentPage
    {
        readonly SqLiteManager _manager = new SqLiteManager();
        readonly List<float> _turSayisi = new List<float>();
        List<Kitap> _kitapTurList = new List<Kitap>();
        readonly string[] _renkler = new string[] { "#df7096", "#1b4332", "#83c5be", "#00b4d8", "#14213d", "#ffafcc", "#fdf1e6", "#1b4332", "#f9844a", "#f5cac3", "#d90429", "#e36414", "#55a630", "#6f1d1b", "#ff87ab", "#8206c7", "#da5552", "#c0b9dd", "#602437", "#f9c74f", "#023e8a" };
        string _grafikTuru = "Sütun Grafik";
        public Istatistikler(string kulladi)
        {
            InitializeComponent();

            LblKullaniciAdi.Text = kulladi;
            PkrTur.SelectedItem = PkrTur.Items[0];

            LblToplamKitap.Text = _manager.TopKitapSayisi(LblKullaniciAdi.Text).ToString();
            LblToplamSayfa.Text = (_manager.OkunmamisSayfaSayisi(LblKullaniciAdi.Text) + _manager.OkunmusSayfaSayisi(LblKullaniciAdi.Text)).ToString();
            LblToplamTur.Text = _kitapTurList.Count.ToString();

            if (_manager.OkunmamisSayfaSayisi(LblKullaniciAdi.Text) >= _manager.OkunmusSayfaSayisi(LblKullaniciAdi.Text))
            {
                LblOneri.Text = "Daha çok kitap okumalısın.";
                LblOneri.TextColor = Color.Red;
            }
            else
            {
                LblOneri.Text = "Aynen böyle devam.";
                LblOneri.TextColor = Color.Green;
            }
        }
        void GrafikTurleri()
        {
            List<ChartEntry> entries = new List<ChartEntry>();
            _kitapTurList = _manager.Turler(LblKullaniciAdi.Text).ToList();

            foreach (var item in _kitapTurList)
            {
                _turSayisi.Add(_manager.TurSayisi(item.Tur));
            }
            int i = 0;
            foreach (var item in _kitapTurList)
            {
                if (item.Tur != " ")
                {
                    entries.Add(new ChartEntry(_turSayisi[i])
                    {
                        Label = item.Tur,
                        ValueLabel = _turSayisi[i].ToString(),
                        ValueLabelColor = SKColor.Parse(_renkler[i]),
                        Color = SKColor.Parse(_renkler[i]),
                        TextColor = SKColor.Parse("#000000")
                    });
                }
                else
                {
                    entries.Add(new ChartEntry(_turSayisi[i])
                    {
                        Label = "Boş",
                        ValueLabel = _turSayisi[i].ToString(),
                        ValueLabelColor = SKColor.Parse(_renkler[i]),
                        Color = SKColor.Parse(_renkler[i]),
                        TextColor = SKColor.Parse("#000000")
                    });
                }
                i++;
            }
            if (_grafikTuru == "Sütun Grafik")
            {
                var grafik = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikTur.Chart = grafik;
            }
            else if (_grafikTuru == "Çizgi Grafik")
            {
                var grafik = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikTur.Chart = grafik;
            }
            else if (_grafikTuru == "Nokta Grafik")
            {
                var grafik = new PointChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikTur.Chart = grafik;
            }
            else if (_grafikTuru == "Dairesel Grafik")
            {
                var grafik = new RadialGaugeChart() { Entries = entries, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikTur.Chart = grafik;
            }
            else if (_grafikTuru == "Pasta Grafik")
            {
                var grafik = new DonutChart() { Entries = entries, LabelTextSize = 35f, HoleRadius = 0f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000"), GraphPosition = GraphPosition.AutoFill };
                GrafikTur.Chart = grafik;
            }
            else if (_grafikTuru == "Radar Grafik")
            {
                var grafik = new RadarChart() { Entries = entries, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikTur.Chart = grafik;
            }
        }
        void GrafikKitapOkunduOkunmadi()
        {
            var entries = new List<ChartEntry>()
            {
                new ChartEntry(Convert.ToSingle(_manager.OkunmusKitapSayisi(LblKullaniciAdi.Text)))
                {
                    Label="Okunmuş Kitap",
                    ValueLabel= _manager.OkunmusKitapSayisi(LblKullaniciAdi.Text).ToString(),
                    ValueLabelColor=SKColor.Parse("#6a994e"),
                    Color=SKColor.Parse("#6a994e"),
                    TextColor = SKColor.Parse("#000000")
                },
                new ChartEntry(Convert.ToSingle(_manager.OkunmamisKitapSayisi(LblKullaniciAdi.Text)))
                {
                    Label="Okunmamış Kitap",
                    ValueLabel= _manager.OkunmamisKitapSayisi(LblKullaniciAdi.Text).ToString(),
                    ValueLabelColor=SKColor.Parse("#d62828"),
                    Color=SKColor.Parse("#d62828"),
                    TextColor = SKColor.Parse("#000000")
                },
            };

            if (_grafikTuru == "Sütun Grafik")
            {
                var grafik = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikKitap.Chart = grafik;
            }
            else if (_grafikTuru == "Çizgi Grafik")
            {
                var grafik = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikKitap.Chart = grafik;
            }
            else if (_grafikTuru == "Nokta Grafik")
            {
                var grafik = new PointChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikKitap.Chart = grafik;
            }
            else if (_grafikTuru == "Dairesel Grafik")
            {
                var grafik = new RadialGaugeChart() { Entries = entries, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikKitap.Chart = grafik;
            }
            else if (_grafikTuru == "Pasta Grafik")
            {
                var grafik = new DonutChart() { Entries = entries, LabelTextSize = 35f, HoleRadius = 0f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000"), GraphPosition = GraphPosition.AutoFill };
                GrafikKitap.Chart = grafik;
            }
            else if (_grafikTuru == "Radar Grafik")
            {
                var grafik = new RadarChart() { Entries = entries, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikKitap.Chart = grafik;
            }
        }
        void GrafikSayfaOkunduOkunmadi()
        {
            var entries1 = new List<ChartEntry>()
            {
                new ChartEntry(Convert.ToSingle(_manager.OkunmusSayfaSayisi(LblKullaniciAdi.Text)))
                {
                    Label="Okunmuş Sayfa",
                    ValueLabel= _manager.OkunmusSayfaSayisi(LblKullaniciAdi.Text).ToString(),
                    ValueLabelColor=SKColor.Parse("#a7c957"),
                    Color=SKColor.Parse("#a7c957"),
                    TextColor = SKColor.Parse("#000000")
                },
                new ChartEntry(Convert.ToSingle(_manager.OkunmamisSayfaSayisi(LblKullaniciAdi.Text)))
                {
                    Label="Okunmamış Sayfa",
                    ValueLabel= _manager.OkunmamisSayfaSayisi(LblKullaniciAdi.Text).ToString(),
                    ValueLabelColor=SKColor.Parse("#d00000"),
                    Color=SKColor.Parse("#d00000"),
                    TextColor = SKColor.Parse("#000000")
                },
            };

            if (_grafikTuru == "Sütun Grafik")
            {
                var grafik1 = new BarChart() { Entries = entries1, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikSayfa.Chart = grafik1;
            }
            else if (_grafikTuru == "Çizgi Grafik")
            {
                var grafik1 = new LineChart() { Entries = entries1, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikSayfa.Chart = grafik1;
            }
            else if (_grafikTuru == "Nokta Grafik")
            {
                var grafik1 = new PointChart() { Entries = entries1, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikSayfa.Chart = grafik1;
            }
            else if (_grafikTuru == "Dairesel Grafik")
            {
                var grafik1 = new RadialGaugeChart() { Entries = entries1, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikSayfa.Chart = grafik1;
            }
            else if (_grafikTuru == "Pasta Grafik")
            {
                var grafik1 = new DonutChart() { Entries = entries1, LabelTextSize = 35f, HoleRadius = 0f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000"), GraphPosition = GraphPosition.Center };
                GrafikSayfa.Chart = grafik1;
            }
            else if (_grafikTuru == "Radar Grafik")
            {
                var grafik1 = new RadarChart() { Entries = entries1, LabelTextSize = 35f, BackgroundColor = SKColor.Empty, LabelColor = SKColor.Parse("#000000") };
                GrafikSayfa.Chart = grafik1;
            }
        }
        private void pkrTur_SelectedIndexChanged(object sender, EventArgs e)
        {
            _grafikTuru = PkrTur.SelectedItem.ToString();
            GrafikTurleri();
            GrafikKitapOkunduOkunmadi();
            GrafikSayfaOkunduOkunmadi();
        }
    }
}