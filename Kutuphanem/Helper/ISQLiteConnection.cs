using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kutuphanem.Helper
{
    public interface ISqLiteConnection
    {
        SQLiteConnection GetConnection();
    }
}
