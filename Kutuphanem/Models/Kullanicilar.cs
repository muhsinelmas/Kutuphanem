using SQLite;

namespace Kutuphanem.Models
{
    public class Kullanicilar
    {
        [PrimaryKey][AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool BeniHatirla { get; set; } = false;
        public string GSorusu { get; set; }
        public string GCevap { get; set; }
    }
}
