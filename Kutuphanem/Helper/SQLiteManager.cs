using Kutuphanem.Models;
using SQLite;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Kutuphanem.Helper
{
    public class SqLiteManager
    {
        readonly SQLiteConnection _sQLiteConnection;

        public SqLiteManager()
        {
            _sQLiteConnection = DependencyService.Get<ISqLiteConnection>().GetConnection();
            _sQLiteConnection.CreateTable<Kitap>();
            _sQLiteConnection.CreateTable<Kullanicilar>();
        }
        #region CRUD

        // Yeni kitap ekler.
        public int Insert(Kitap k)
        {
            return _sQLiteConnection.Insert(k);
        }

        // Yeni kullanıcı ekler.
        public int KullaniciEkle(Kullanicilar k)
        {
            return _sQLiteConnection.Insert(k);
        }

        // Kitap bilgilerini günceller.
        public int Update(Kitap k)
        {
            //string sql = $"Update Kitap Set Ad='{k.Ad}',Yazar='{k.Yazar}',YayinEvi='{k.YayinEvi}',Tur='{k.Tur}',Sayfa='{k.Sayfa}',Okundu='{k.Okundu}',Tarih='{k.Tarih}',Aciklama='{k.Aciklama}' Where Id='{k.Id}'";
            //return sQLiteConnection.Execute(sql);

            try
            {
                return _sQLiteConnection.Update(k);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        // Kullanıcı bilgilerini günceller.
        public int Duzenle(Kullanicilar k)
        {
            return _sQLiteConnection.Update(k);
        }

        // Kitap kullanıcı adına göre güncellenir.
        public void Duzenle1(string kullAdi)
        {
            string sql = "Update Kitap Set Kullanici='" + kullAdi + "'";
            _sQLiteConnection.Query<Kitap>(sql);
        }

        // Kullanıcı adı güncellendiğinde kitapta yeni kullanıcı adına göre göncellenir.
        public void Duzenle2(string yKullAdi, string eKullAdi)
        {
            string sql = "Update Kitap Set Kullanici='" + yKullAdi + "' Where Kullanici='" + eKullAdi + "'";
            _sQLiteConnection.Query<Kitap>(sql);
        }

        // id'ye göre kitap siler.
        public int Delete(int id)
        {
            return _sQLiteConnection.Delete<Kitap>(id);
        }

        // Kullanıcı bilgilerini siler
        public int KullaniciSil(int id, string kullanici)
        {
            string sql = "Delete From Kitap Where Kullanici='" + kullanici + "'";
            _sQLiteConnection.Query<Kitap>(sql);
            return _sQLiteConnection.Delete<Kullanicilar>(id);
        }

        // Bütün kitapları getirir.
        public IEnumerable<Kitap> GetAll(string kullAdi)
        {
            string sql = "Select * From Kitap Where Kullanici='" + kullAdi + "'";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Türe göre bütün kitapları getirir.
        public IEnumerable<Kitap> TureGoreButunKitaplariGetir(string kullAdi, string tur)
        {
            string sql = "Select * From Kitap Where Kullanici='" + kullAdi + "' And Tur='" + tur + "'";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Türe göre okunan-okunmayan kitapları getirir.
        public IEnumerable<Kitap> TureGoreOkunanKitaplariGetir(string kullAdi, string tur, bool okundu)
        {
            string sql = "Select * From Kitap Where Kullanici='" + kullAdi + "' And Tur='" + tur + "' And Okundu=" + okundu + "";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Seçilen filtreye göre kitapları getirir.
        public IEnumerable<Kitap> FiltreyeGoreGetir(bool okundu, string kullAdi)
        {
            string sql = "Select * From Kitap Where Okundu=" + okundu + " And Kullanici='" + kullAdi + "'";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Kullanıcı sayısını getirir.
        public int KullaniciSayisi()
        {
            string sql = "Select Count(*) From Kullanicilar";
            return _sQLiteConnection.ExecuteScalar<int>(sql);
        }

        // Kitap id'sine göre kitap bilgilerini getirir.
        public Kitap Get(int id)
        {
            return _sQLiteConnection.Table<Kitap>().Where(t => t.Id == id).FirstOrDefault();
        }

        // Kullanıcı id'sine göre kullanıcının bilgilerini getirir. 
        public Kullanicilar KullaniciBilgi(int id)
        {
            return _sQLiteConnection.Table<Kullanicilar>().Where(a => a.Id == id).FirstOrDefault();
        }

        // Kullanıcı adına göre kullanıcının bilgilerini getirir. 
        public Kullanicilar KullaniciBilgiKullaniciadi(string kulladi)
        {
            return _sQLiteConnection.Table<Kullanicilar>().Where(a => a.UserName == kulladi).FirstOrDefault();
        }

        // Kullanıcı adına göre kullanıcının ismini getirir.
        public Kullanicilar HesapAd(string kulladi)
        {
            return _sQLiteConnection.Table<Kullanicilar>().Where(a => a.UserName == kulladi).FirstOrDefault();
        }

        // Toplam kitap sayısını getirir.
        public int TopKitapSayisi(string kulladi)
        {
            string sql = "Select Count(*) From Kitap Where Kullanici='" + kulladi + "'";
            return _sQLiteConnection.ExecuteScalar<int>(sql);
        }

        // Okunmamış kitap sayısını getirir.
        public int OkunmamisKitapSayisi(string kulladi)
        {
            string sql = "Select Count(*) From Kitap Where Okundu=False And Kullanici='" + kulladi + "'";
            return _sQLiteConnection.ExecuteScalar<int>(sql);
        }

        // Okunmuş kitap sayısını getirir.
        public int OkunmusKitapSayisi(string kulladi)
        {
            string sql = "Select Count(*) From Kitap Where Okundu=True And Kullanici='" + kulladi + "'";
            return _sQLiteConnection.ExecuteScalar<int>(sql);
        }

        // Okunmamış sayfa sayısını getirir.
        public int OkunmamisSayfaSayisi(string kulladi)
        {
            string sql = "SELECT SUM(Sayfa) FROM Kitap WHERE Okundu = False And Kullanici='" + kulladi + "'";
            return _sQLiteConnection.ExecuteScalar<int>(sql);
        }

        // Okunmuş sayfa sayısını getirir.
        public int OkunmusSayfaSayisi(string kulladi)
        {
            string sql = "SELECT SUM(Sayfa) FROM Kitap WHERE Okundu = True And Kullanici='" + kulladi + "'";
            return _sQLiteConnection.ExecuteScalar<int>(sql);
        }

        // Kaç farklı tür olduğunu getirir.
        public IEnumerable<Kitap> Turler(string kulladi)
        {
            string sql = "SELECT Tur FROM Kitap GROUP BY Tur HAVING Kullanici = '" + kulladi + "'";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Her türden kaç tane olduğunu getirir.
        public float TurSayisi(string tur)
        {
            string sql = "SELECT Count(Tur) FROM Kitap Where Tur='" + tur + "'";
            return _sQLiteConnection.ExecuteScalar<float>(sql);
        }

        // Kitap tablosunda arama yapar
        public IEnumerable<Kitap> Search(string text, string filter, string kulladi)
        {
            var textReplace = text.Replace("'", "''");
            string sql = "Select * From Kitap Where " + filter + " LIKE '%" + textReplace + "%' And Kullanici='" + kulladi + "'";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Eklenen kitabın kayıtlı olup olmadığını bulur.
        public IEnumerable<Kitap> AyniMi(string ad, string yazar, string kulladi)
        {
            var adReplace = ad.Replace("'", "''");
            var yazarReplace = yazar.Replace("'", "''");
            string sql = "Select * From Kitap Where Ad='" + adReplace + "' And Yazar='" + yazarReplace + "' And Kullanici='" + kulladi + "'";
            return _sQLiteConnection.Query<Kitap>(sql);
        }

        // Eklenen kullanıcı adının aynı olup olmadığını bulur.
        public IEnumerable<Kullanicilar> KullaniciAyniMi(string kulAdi)
        {
            var adReplace = kulAdi.Replace("'", "''");
            string sql = "Select * From Kullanicilar Where UserName='" + adReplace + "'";
            return _sQLiteConnection.Query<Kullanicilar>(sql);
        }

        // Girilen kullanıcı adının ve şifrenin doğru olup olmadığını bulur.
        public IEnumerable<Kullanicilar> GirisYaptiMi(string ad, string sifre)
        {
            var adReplace = ad.Replace("'", "''");
            var sifreReplace = sifre.Replace("'", "''");
            string sql = "Select * From Kullanicilar Where UserName='" + adReplace + "' And Password='" + sifreReplace + "'";
            return _sQLiteConnection.Query<Kullanicilar>(sql);
        }

        // Kullanıcı "Beni hatırla"ı seçtiyse tabloyu ona göre günceller.
        public void BeniHatirla(string kulladi, bool benihatirla)
        {
            string sql = "Update Kullanicilar Set BeniHatirla=" + benihatirla + " Where UserName='" + kulladi + "'";
            _sQLiteConnection.Query<Kullanicilar>(sql);
        }

        // Beni hatırla diyen kullanıcılar getirilir.
        public IEnumerable<Kullanicilar> BeniHatirlaGetir()
        {
            string sql = "Select * From Kullanicilar Where BeniHatirla = True";
            return _sQLiteConnection.Query<Kullanicilar>(sql);
        }
        public void Dispose()
        {
            _sQLiteConnection.Dispose();
        }
        #endregion
    }
}