using Kutuphanem.Helper;
using SQLite;
using Kutuphanem.Droid.ConnectionHelper;

[assembly: Xamarin.Forms.Dependency(typeof(GetAndroidConnection))]
namespace Kutuphanem.Droid.ConnectionHelper
{
    public class GetAndroidConnection : ISqLiteConnection
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