using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrantFirstTask.Interfaces
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
