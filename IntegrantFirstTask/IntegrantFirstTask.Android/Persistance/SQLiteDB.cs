using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using IntegrantFirstTask.Interfaces;
using SQLite;
using IntegrantFirstTask.Droid.Persistance;
using System.IO;

[assembly: Dependency(typeof(SQLiteDB))] // Register the class implementation Service in Dependency Service
namespace IntegrantFirstTask.Droid.Persistance
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