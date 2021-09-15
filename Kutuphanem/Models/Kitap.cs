using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kutuphanem.Models
{
    public class Kitap
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public string YayinEvi { get; set; }
        public string Tur { get; set; }
        public int Sayfa { get; set; }
        public bool Okundu { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string Kullanici { get; set; }
        public bool BenimMi { get; set; }
    }
}
