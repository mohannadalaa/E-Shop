using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.iOS.Persistance;
using SQLite;
using System.IO;

[assembly: Dependency(typeof(SQLiteDB))] // Register the class implementation Service in Dependency Service
namespace IntegrantFirstTask.iOS.Persistance
{
    class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            //Get Folder path on the device to store the db file 
            var docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //Concat db file name on the Path
            var path = Path.Combine(docPath, "TaskDB3.db");
            return new SQLiteAsyncConnection(path);
        }
    }
}