using Kutuphanem.Helper;
using SQLite;
using Kutuphanem.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetiOSConnection))]
namespace Kutuphanem.iOS
{
    public class GetiOSConnection : ISqLiteConnection
    {
        public SQLiteConnection GetConnection()
        {
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(documentPath, App.DbName);

            var connection = new SQLiteConnection(path, false);
            return connection;
        }
    }
}